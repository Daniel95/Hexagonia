using UnityEngine;

/// <summary>
///  UpdateTargetPosition gets updated everytime the method gets called with the event: LookPositionUpdateEvent in LookPositionPlane.
///  It smoothly moves the player when moving around in the game but in 2D.
/// </summary>
public class SmoothPlaneMovement2D : MonoBehaviour
{
    [SerializeField] [Range(0, 20)] private float speed = 0.3f;
    [SerializeField] private Vector3 offset;

    private void UpdateTargetPosition(Vector3 _targetPosition, Vector3 _delta)
    {
        Vector3 _targetPositionWithOffset = _targetPosition + offset;
        Vector2 _direction = _delta.normalized;
        float _distance = Vector2.Distance(_targetPositionWithOffset, transform.position);
        float deltaSpeed = speed * Time.deltaTime;

        Vector3 positionIncrement = _direction * (_distance * deltaSpeed);
        transform.position += positionIncrement;
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