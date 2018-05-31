using UnityEngine;

/// <summary>
///  UpdateTargetPosition gets updated everytime the method gets called with the event: LookPositionUpdateEvent in LookPositionPlane.
/// </summary>
public class InstantPlaneMovement : MonoBehaviour
{
    [SerializeField] private Vector3 offset;

    private void UpdateTargetPosition(Vector3 _targetPosition)
    {
        transform.position = _targetPosition + offset;
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