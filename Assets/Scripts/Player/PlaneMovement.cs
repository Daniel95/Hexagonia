using System;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(BoxCollider))]
public class PlaneMovement : MonoBehaviour
{

    public static Action<Vector3> MovePointOnPlaneEvent;

    [SerializeField] private Transform hmdTransform;
    [SerializeField] [Range(0, 1)] private float scaledInput = 0;

    private Vector2 maxBounds;
    private Vector2 minBounds;
    private Plane plane;

    private void Awake()
    {
        BoxCollider _boxCollider = GetComponent<BoxCollider>();
        minBounds = _boxCollider.bounds.min;
        maxBounds = _boxCollider.bounds.max;

        plane = new Plane(Vector3.forward, transform.position);
    }

    private void Update()
    {
        bool _hit;
        Vector3 _planePoint = GetRaycastPointOnPlane(out _hit);
        if(!_hit) { return; }

        if(MovePointOnPlaneEvent != null)
        {
            MovePointOnPlaneEvent(_planePoint);
        }
    }

    private Vector3 GetRaycastPointOnPlane(out bool _hit)
    {
        Vector3 _lookOriginPosition = hmdTransform.position;
        Vector3 _lookDirection = hmdTransform.forward;

        float _enter = 0.0f;
        Ray _ray = new Ray(_lookOriginPosition, _lookDirection);

        Vector3 _pointOnPlane = new Vector3();
        _hit = plane.Raycast(_ray, out _enter);

        if (_hit)
        {
            Vector3 _hitPoint = _ray.GetPoint(_enter);
            Vector2 _centerOffset = transform.position - _hitPoint;
            Vector2 _scaleOffset = ((Vector2)_hitPoint - _centerOffset) * scaledInput;
            Vector2 _scaledPointOnPlane = (Vector2)_hitPoint + _scaleOffset;
            Vector2 _clampedPointOnPlane = VectorHelper.Clamp(_scaledPointOnPlane, minBounds, maxBounds);
            _pointOnPlane = new Vector3(_clampedPointOnPlane.x, _clampedPointOnPlane.y, _hitPoint.z);
        }

        return _pointOnPlane;
    }
}