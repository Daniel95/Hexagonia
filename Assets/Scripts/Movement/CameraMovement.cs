using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    [SerializeField] [Range(0, 1)] private float speed = 0.3f;

    private void UpdateTargetPosition(Vector3 _targetPosition)
    {
        Vector2 _delta = _targetPosition - transform.position;
        Vector2 _direction = _delta.normalized;
        float _distance = Vector2.Distance(_targetPosition, transform.position);

        Vector3 newPosition = _direction * (_distance * speed);
        transform.position += newPosition;
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