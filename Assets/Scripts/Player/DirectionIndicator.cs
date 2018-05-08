using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DirectionIndicator : MonoBehaviour {

    [SerializeField] private float maxDistance = 100;
    [SerializeField] private LayerMask layerMask;

    private LineRenderer lineRenderer;

    private void UpdateTargetPosition(Vector3 _targetPosition)
    {
        lineRenderer.SetPosition(0, transform.position);

        Vector3 _offsetToTarget = _targetPosition - transform.position;
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

    private void OnEnable()
    {
        LookPositionOnPlane.LookPositionUpdatedEvent += UpdateTargetPosition;
    }

    private void OnDisable()
    {
        LookPositionOnPlane.LookPositionUpdatedEvent -= UpdateTargetPosition;
    }

}
