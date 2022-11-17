using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    [Header("Framing")] public Camera camera;

    [Header("Rotation")] 
    [Range(-90f, 90f)]
    public float minVerticalAngle = -90f;
    [Range(-90f, 90f)]
    public float maxVerticalAngle = 90f;
    public float rotationSpeed = 1f;
    public float rotationSharpness = 10000f;

    public Transform Transform { get; private set; }
    public Vector3 PlanarDirection { get; private set; }

    private float _targetVerticalAngle;

    private void Awake()
    {
        Transform = this.transform;
        PlanarDirection = Vector3.forward;
        _targetVerticalAngle = 0f;
    }

    public void UpdateWithInput(float deltaTime, Vector2 rotationInput)
    {
        //HORIZONTAL
        //How much to rotate around the Y axis?
        Quaternion rotationFromInput = Quaternion.Euler(Vector3.up * (rotationInput.x * rotationSpeed));
        //Applies this rotation to the vector planarDirection
        PlanarDirection = rotationFromInput * PlanarDirection;
        //The Quaternion that represents the rotation needed to look at the planarDirection we've just calculated.
        Quaternion planarRot = Quaternion.LookRotation(PlanarDirection);
        
        //VERTICAL
        //Reduce the angle when the mouse moves up and vice-versa.
        _targetVerticalAngle -= (rotationInput.y * rotationSpeed);
        //Ensure the min and max vertical angles are not violated.
        _targetVerticalAngle = Mathf.Clamp(_targetVerticalAngle, minVerticalAngle, maxVerticalAngle);
        //Rotation that represents some number of degrees on the X axis (local ofc)
        Quaternion verticalRot = Quaternion.Euler(_targetVerticalAngle, 0, 0);
        
        //Calculate the final desired rotation. Multiply the vert and horiz rotation. Slerp to it according to an exponential function.
        //Higher sharpness returns lower numbers from the e function, in turn raising t.
        Quaternion targetRotation = Quaternion.Slerp(Transform.rotation, planarRot * verticalRot, 1f - Mathf.Exp(-rotationSharpness) * deltaTime);
        
        //Apply the rotation
        Transform.rotation = targetRotation;
    }
    
    
}
