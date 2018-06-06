using UnityEngine;

/// <summary>
///  UpdateTargetPosition gets updated everytime the method gets called with the event: LookPositionUpdateEvent in LookPositionPlane.
/// </summary>
public class InstantPlaneMovement : MonoBehaviour
{
    [SerializeField] private Vector3 offset;

    private bool planeHit;
    private Vector3 lookPositionOnPlane;

    private void Update()
    {
        lookPositionOnPlane = LookPositionOnPlane.Instance.GetLookPosition(out planeHit);
        if(!planeHit) { return; }

        transform.position = lookPositionOnPlane + offset;
    }
}