using System;
using System.Collections;
using System.Collections.Generic;
using KinematicCharacterController;
using UnityEngine;

public enum TimePeriod
{
    Past,
    Present
}
public class TimeSwitcher : MonoBehaviour
{
    
    private TimePeriod _timePeriod = TimePeriod.Present;
    private bool _swapping = false;
    private PlayerController _playerController;

    private void OnEnable()
    {
        _playerController = GetComponent<PlayerController>();
        PlayerController.OnRequestTimeSwap += SwapTime;
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
        _timePeriod = _timePeriod == TimePeriod.Present ? TimePeriod.Past : TimePeriod.Present;
        _swapping = false;
    }
}
