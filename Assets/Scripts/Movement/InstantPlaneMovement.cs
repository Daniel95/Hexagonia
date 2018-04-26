using UnityEngine;

public class InstantPlaneMovement : MonoBehaviour
{

    private void UpdateTargetPosition(Vector3 _targetPosition)
    {
        transform.position = _targetPosition;
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