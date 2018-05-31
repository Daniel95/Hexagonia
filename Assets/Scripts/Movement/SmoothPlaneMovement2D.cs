using UnityEngine;

/// <summary>
///  UpdateTargetPosition gets updated everytime the method gets called with the event: LookPositionUpdateEvent in LookPositionPlane.
///  It smoothly moves the player when moving around in the game but in 2D.
/// </summary>
public class SmoothPlaneMovement2D : MonoBehaviour
{
    [SerializeField] [Range(0, 20)] private float speed = 0.3f;
    [SerializeField] private Vector3 offset;

    private void UpdateTargetPosition(Vector3 _targetPosition)
    {
        Vector3 _targetPositionWithOffset = _targetPosition + offset;
        Vector2 _delta = _targetPositionWithOffset - transform.position;
        Vector2 _direction = _delta.normalized;
        float _distance = Vector2.Distance(_targetPositionWithOffset, transform.position);
        float _deltaSpeed = speed * Time.deltaTime;

        Vector3 _positionIncrement = _direction * (_distance * _deltaSpeed);
        transform.position += _positionIncrement;
    }

    private void OnEnable()
    {
        PlayerInput.InputEvent += UpdateTargetPosition;
    }

    private void OnDisable()
    {
        PlayerInput.InputEvent -= UpdateTargetPosition;
    }
}