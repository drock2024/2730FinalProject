using System;
using System.Collections;
using System.Collections.Generic;
using KinematicCharacterController;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(KinematicCharacterMotor))]
public class PlayerController : MonoBehaviour, ICharacterController
{
    public KinematicCharacterMotor motor;
    public FirstPersonCamera fpsCamera;

    [Header("Movement")] 
    public float maxStableMoveSpeed = 5f;
    public float stableMovementSharpness = 15f;
    public float orientationSharpness = 10f;

    [Header("Collisions")] 
    public List<Collider> ignoredColliders = new List<Collider>();

    public static Action OnRequestTimeSwap;

    private const string MouseXInput = "Mouse X";
    private const string MouseYInput = "Mouse Y";
    private const string MouseScrollInput = "Mouse ScrollWheel";
    private const string HorizontalInput = "Horizontal";
    private const string VerticalInput = "Vertical";

    private Vector3 _moveInputVector;
    private Vector3 _lookInputVector;
    private bool _interactDown;
    private bool _journalDown;
    private bool _timeSwapDown;
    private Vector3 _gravity = new Vector3(0, -30f, 0);
    

    private void Awake()
    {
        if (!motor) motor = GetComponent<KinematicCharacterMotor>();
        motor.CharacterController = this;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        HandleCharacterInput();
    }

    private void LateUpdate()
    {
        HandleCameraInput();
    }

    void HandleCharacterInput()
    {
        //Gets a vector with the same rotation as the camera, project it onto the character's xz plane and normalize.
        var cameraDirectionOnCharPlane =
            Vector3.ProjectOnPlane(fpsCamera.Transform.rotation * Vector3.forward, Vector3.up).normalized;
        //The rotation to look at the projected camera direction
        if (cameraDirectionOnCharPlane.sqrMagnitude == 0f)
        {
            cameraDirectionOnCharPlane = Vector3.ProjectOnPlane(fpsCamera.Transform.rotation * Vector3.up, Vector3.up)
                .normalized;
        }
        Quaternion cameraRotationOnCharPlane = Quaternion.LookRotation(cameraDirectionOnCharPlane);
        //Clamp the magnitude of the input axes, and rotate the vector point in the direction of the camera (in xz)
        _moveInputVector = cameraRotationOnCharPlane * 
            Vector3.ClampMagnitude(new Vector3(Input.GetAxisRaw(HorizontalInput), 0f, Input.GetAxisRaw(VerticalInput)),
                1f);
        
        //The character faces the camera direction, always, on its xz plane.
        _lookInputVector = cameraDirectionOnCharPlane;

        _interactDown = Input.GetKeyDown(KeyCode.E);
        _journalDown = Input.GetKeyDown(KeyCode.J);
        _timeSwapDown = Input.GetKeyDown(KeyCode.Q);

        if (_timeSwapDown)
        {
            OnRequestTimeSwap?.Invoke();
        }
    }
    
    private void HandleCameraInput()
    {
        var lookInputVector = new Vector2(Input.GetAxisRaw(MouseXInput), Input.GetAxisRaw(MouseYInput));
        if (Cursor.lockState != CursorLockMode.Locked) lookInputVector = Vector3.zero;
        fpsCamera.UpdateWithInput(Time.deltaTime, lookInputVector);
    }

    public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
    {
        if (_lookInputVector.sqrMagnitude > 0f && orientationSharpness > 0f)
        {
            // Smoothly interpolate from current to target look direction
            Vector3 smoothedLookInputDirection = Vector3.Slerp(motor.CharacterForward, _lookInputVector, 1 - Mathf.Exp(-orientationSharpness * deltaTime)).normalized;
        
            // Set the current rotation (which will be used by the KinematicCharacterMotor)
            currentRotation = Quaternion.LookRotation(smoothedLookInputDirection);
        }
    }

    public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
    {
        if (motor.GroundingStatus.IsStableOnGround)
        {
            var currentVelocityMagnitude = currentVelocity.magnitude;
            var effectiveGroundNormal = motor.GroundingStatus.GroundNormal;
            currentVelocity = motor.GetDirectionTangentToSurface(currentVelocity, effectiveGroundNormal) *
                              currentVelocityMagnitude;
            
            //Reorients the inputVector to also be in relation to the groundNormal
            var inputRight = Vector3.Cross(_moveInputVector, motor.CharacterUp);
            var reorientedInput = Vector3.Cross(effectiveGroundNormal, inputRight).normalized *
                                  _moveInputVector.magnitude;
            var targetVelocity = reorientedInput * maxStableMoveSpeed;
            currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity,
                1f - Mathf.Exp(-stableMovementSharpness * deltaTime));
        }
        else
        {
            currentVelocity += _gravity * deltaTime;
        }
    }

    public void BeforeCharacterUpdate(float deltaTime)
    {
       
    }

    public void PostGroundingUpdate(float deltaTime)
    {
        
    }

    public void AfterCharacterUpdate(float deltaTime)
    {
       
    }

    public bool IsColliderValidForCollisions(Collider coll)
    {
        if (ignoredColliders.Count == 0)
        {
            return true;
        }

        if (ignoredColliders.Contains(coll))
        {
            return false;
        }

        return true;
    }

    public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
    {
        
    }

    public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint,
        ref HitStabilityReport hitStabilityReport)
    {
        
    }

    public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition,
        Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
    {
        
    }

    public void OnDiscreteCollisionDetected(Collider hitCollider)
    {
        
    }
}
