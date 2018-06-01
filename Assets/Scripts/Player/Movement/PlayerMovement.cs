using System;
using UnityEngine;

/// <summary>
///  UpdateTargetPosition gets updated everytime the method gets called with the event: LookPositionUpdateEvent in LookPositionPlane.
///  It Smoothly moves the player when moving around in the game.
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    public static Action<Vector3> OnMoved;

    [SerializeField] [Range(0, 20)] private float vrSpeed = 10f;
    [SerializeField] [Range(0, 20)] private float nonVRSpeed = 10f;
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -3);

    private float currentSpeed;

    private void UpdateTargetPosition(Vector3 _targetPosition)
    {
        Vector3 _targetPositionWithOffset = _targetPosition + offset;

        if (transform.position == _targetPositionWithOffset) { return; }

        Vector3 _delta = _targetPositionWithOffset - transform.position;
        Vector3 _direction = _delta.normalized;
        float _distance = Vector3.Distance(_targetPositionWithOffset, transform.position);
        float _deltaSpeed = currentSpeed     * Time.deltaTime;

        Vector3 _positionIncrement = _direction * (_distance * _deltaSpeed);
        transform.position += _positionIncrement;

        if (OnMoved != null)
        {
            OnMoved(transform.position);
        }
    }

    private void UpdateSpeedToVRSpeed()
    {
        if(VRSwitch.VRState)
        {
            currentSpeed = vrSpeed;
        }
        else
        {
            currentSpeed = nonVRSpeed;
        }
    }

    private void Awake()
    {
        UpdateSpeedToVRSpeed();
    }

    private void OnEnable()
    {
        PlayerInputController.InputEvent += UpdateTargetPosition;
        VRSwitch.SwitchedEvent += UpdateSpeedToVRSpeed;
    }

    private void OnDisable()
    {
        PlayerInputController.InputEvent -= UpdateTargetPosition;
        VRSwitch.SwitchedEvent += UpdateSpeedToVRSpeed;
    }
}