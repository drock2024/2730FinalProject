using System;
using System.Collections;
using System.Collections.Generic;
using KinematicCharacterController;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public enum TimePeriod
{
    Past,
    Present
}
public class TimeSwitcher : MonoBehaviour
{
    [SerializeField] private float transitionDuration = 3f;
    [SerializeField] private float maxLensDistortion;
    [SerializeField] private float maxChromaticAberration;
    [SerializeField] private float maxScale;
    [SerializeField] private float maxBloomThreshold;
    [SerializeField] private float maxBloomIntensity;

    [SerializeField] private VolumeProfile volumeProfile;

    private TimePeriod _timePeriod = TimePeriod.Present;
    private bool _swapping = false;
    private PlayerController _playerController;
    private AudioSource _audioSource;
    private ChromaticAberration _chromaticAberration;
    private LensDistortion _lensDistortion;
    private Bloom _bloom;
    private float _swapTimer;
    private bool _entering;

    private float _defaultBloomThreshold;
    private float _defaultBloomIntensity;

    private void OnEnable()
    {
        _playerController = GetComponent<PlayerController>();
        _audioSource = GetComponent<AudioSource>();
        if (volumeProfile)
        {
            volumeProfile.TryGet(out _chromaticAberration);
            volumeProfile.TryGet(out _lensDistortion);
            volumeProfile.TryGet(out _bloom);
            if (_bloom)
            {
                _defaultBloomThreshold = _bloom.threshold.value;
                _defaultBloomIntensity = _bloom.intensity.value;
            }
        }
        PlayerController.OnRequestTimeSwap += SwapTime;
    }
    
    private void Update()
    {
        if (!_swapping || !volumeProfile || !_lensDistortion || !_chromaticAberration || !_bloom) return;
        _swapTimer += Time.deltaTime;
        _lensDistortion.intensity.value =
            _entering
                ? Mathf.Lerp(0f, maxLensDistortion, _swapTimer / (transitionDuration / 2f))
                : Mathf.Lerp(maxLensDistortion, 0f, (_swapTimer - transitionDuration / 2f) / (transitionDuration / 2f));
        _chromaticAberration.intensity.value =
            _entering
                ? Mathf.Lerp(0f, maxChromaticAberration, _swapTimer / (transitionDuration / 2f))
                : Mathf.Lerp(maxChromaticAberration, 0f, (_swapTimer - transitionDuration / 2f) / (transitionDuration / 2f));
        _lensDistortion.scale.value =
            _entering
                ? Mathf.Lerp(1f, maxScale, _swapTimer / (transitionDuration / 2f))
                : Mathf.Lerp(maxScale, 1f, (_swapTimer - transitionDuration / 2f) / (transitionDuration / 2f));
        _bloom.threshold.value =
            _entering
                ? Mathf.Lerp(_defaultBloomThreshold, maxBloomThreshold, _swapTimer / (transitionDuration / 2f))
                : Mathf.Lerp(maxBloomThreshold, _defaultBloomThreshold, (_swapTimer - transitionDuration / 2f) / (transitionDuration / 2f));
        _bloom.intensity.value =
            _entering
                ? Mathf.Lerp(_defaultBloomIntensity, maxBloomIntensity, _swapTimer / (transitionDuration / 2f))
                : Mathf.Lerp(maxBloomIntensity, _defaultBloomIntensity, (_swapTimer - transitionDuration / 2f) / (transitionDuration / 2f));
    }

    bool IsSwapValid()
    {
        if (_swapping || !_playerController) return false;
        return true;
    }

    void SwapTime()
    {
        if (!IsSwapValid()) return;
        _swapping = true;
        var cachedPosition = _playerController.motor.GetState().Position;
        var offset = _timePeriod == TimePeriod.Present ? 100f : -100f;
        _playerController.motor.SetPosition(new Vector3(cachedPosition.x, cachedPosition.y + offset, cachedPosition.z));
        KinematicCharacterSystem.Simulate(Time.deltaTime, KinematicCharacterSystem.CharacterMotors, KinematicCharacterSystem.PhysicsMovers);
        if (_playerController.motor.OverlapsCount != 0)
        {
            Debug.Log("Overlapped, cannot swap time.");
            _playerController.motor.SetPosition(cachedPosition);
            _swapping = false;
            return;
        }

        _playerController.inputsLocked = true;
        _playerController.motor.SetPosition(cachedPosition);
        _swapTimer = 0f;
        if (_audioSource) _audioSource.Play();
        _entering = true;
        StartCoroutine(TeleportDurationWaiter(transitionDuration, new Vector3(cachedPosition.x, cachedPosition.y + offset, cachedPosition.z)));
    }

    private IEnumerator TeleportDurationWaiter(float seconds, Vector3 position)
    {
        yield return new WaitForSeconds(seconds / 2f);
        _entering = false;
        _timePeriod = _timePeriod == TimePeriod.Present ? TimePeriod.Past : TimePeriod.Present;
        _playerController.motor.SetPosition(position);
        _playerController.inputsLocked = false;
        yield return new WaitForSeconds(seconds / 2f);
        _swapping = false;
    }
    
}
