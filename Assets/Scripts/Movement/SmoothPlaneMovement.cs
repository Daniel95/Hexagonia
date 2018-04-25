using UnityEngine;

public class SmoothPlaneMovement : MonoBehaviour
{

    [SerializeField] [Range(0, 1)] private float speed = 0.3f;

    private void UpdateTargetPosition(Vector3 _targetPosition)
    {
        if (transform.position == _targetPosition)
            return;

        Vector3 _delta = _targetPosition - transform.position;
        Vector3 _direction = _delta.normalized;
        float _distance = Vector3.Distance(_targetPosition, transform.position);

        transform.position += _direction * (_distance * speed);
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