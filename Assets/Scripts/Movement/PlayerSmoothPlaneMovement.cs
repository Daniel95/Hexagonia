using System;
using UnityEngine;

/// <summary>
///  UpdateTargetPosition gets updated everytime the method gets called with the event: LookPositionUpdateEvent in LookPositionPlane.
///  It Smoothly moves the player when moving around in the game.
/// </summary>
public class PlayerSmoothPlaneMovement : MonoBehaviour
{
    public static Action<Vector3> OnMoved;

    [SerializeField] [Range(0, 20)] private float speed = 0.3f;
    [SerializeField] private Vector3 offset;

    private void UpdateTargetPosition(Vector3 _targetPosition)
    {
        Vector3 _targetPositionWithOffset = _targetPosition + offset;

        if (transform.position == _targetPositionWithOffset)
            return;

        Vector3 _delta = _targetPositionWithOffset - transform.position;
        Vector3 _direction = _delta.normalized;
        float _distance = Vector3.Distance(_targetPositionWithOffset, transform.position);
        float _deltaSpeed = speed * Time.deltaTime;

        Vector3 _positionIncrement = _direction * (_distance * _deltaSpeed);
        transform.position += _positionIncrement;

        if (OnMoved != null) {
            OnMoved(transform.position);
        }
    }

    private void OnEnable()
    {
        LookPositionOnPlane.LookPositionUpdatedEvent += UpdateTargetPosition;
    }

    private void OnDisable()
    {
        LookPositionOnPlane.LookPositionUpdatedEvent -= UpdateTargetPosition;
    }
}