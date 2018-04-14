using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] [Range(0, 1)] private float speed = 0.3f;

    private void UpdateTargetPosition(Vector3 _targetPosition)
    {
        if (transform.position == _targetPosition)
            return;

        Vector3 offset = _targetPosition - transform.position;
        Vector3 direction = offset.normalized;
        float distance = Vector3.Distance(_targetPosition, transform.position);

        transform.position += direction * (distance * speed);
    }

    private void OnEnable()
    {
        MovementPlane.RaycastPointOnPlaneEvent += UpdateTargetPosition;
    }

    private void OnDisable()
    {
        MovementPlane.RaycastPointOnPlaneEvent -= UpdateTargetPosition;
    }

}
