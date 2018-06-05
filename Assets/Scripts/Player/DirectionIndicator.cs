using UnityEngine;

/// <summary>
/// Indicates the direction the player is moving.
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class DirectionIndicator : MonoBehaviour
{
    [SerializeField] private float maxDistance = 100;
    [SerializeField] private LayerMask layerMask;

    private LineRenderer lineRenderer;
    private bool planeHit;
    private Vector3 lookPositionOnPlane;

    private void Update()
    {
        lookPositionOnPlane = LookPositionOnPlane.Instance.GetLookPosition(out planeHit);
        if (!planeHit) { return; }

        lineRenderer.SetPosition(0, transform.position);

        Vector3 _offsetToTarget = lookPositionOnPlane - transform.position;
        Vector3 _direction = _offsetToTarget.normalized;

        RaycastHit _hit;
        if (Physics.Raycast(transform.position, _direction, out _hit, layerMask))
        {
            lineRenderer.SetPosition(1, _hit.point);
        }
        else
        {
            Vector3 _directionPoint = transform.position + (_direction * maxDistance);
            lineRenderer.SetPosition(1, _directionPoint);
        }
    }

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
}