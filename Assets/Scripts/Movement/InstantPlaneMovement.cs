using UnityEngine;

public class InstantPlaneMovement : MonoBehaviour
{

    private void UpdateTargetPosition(Vector3 _targetPosition)
    {
        transform.position = _targetPosition;
    }

    private void OnEnable()
    {
        LookPositionOnPlane.LookPositionUpdate += UpdateTargetPosition;
    }

    private void OnDisable()
    {
        LookPositionOnPlane.LookPositionUpdate -= UpdateTargetPosition;
    }
}