using System;
using UnityEngine;

/// <summary>
/// Sends a raycast to a plane, on hit it sends a event to the InstantPlaneMovement.cs what moves the player.
/// </summary>
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(BoxCollider))]
public class LookPositionOnPlane : MonoBehaviour
{
    public static LookPositionOnPlane Instance { get { return GetInstance(); } }
    /// <summary>
    /// Parameters: Position, Delta
    /// </summary>
    public static Action<Vector3> LookPositionUpdatedEvent;

    #region Singeton
    private static LookPositionOnPlane instance;

    private static LookPositionOnPlane GetInstance()
    {
        if(instance == null)
        {
            instance = FindObjectOfType<LookPositionOnPlane>();
        }
        return instance;
    }
    #endregion

    public Vector3 MinBounds { get { return maxBounds; } }
    public Vector3 MaxBounds { get { return minBounds; } }
    public Vector3 Size { get { return size; } }

	private Transform hmdTransform;
	private Vector3 maxBounds;
    private Vector3 minBounds;
    private Vector2 size;
    private Plane plane;

    public Vector3 ClampToPlane(Vector3 _point)
    {
        Vector3 _clampedPoint = VectorHelper.Clamp(_point, minBounds, maxBounds);
        //Vector3 _clampedPoint = new Vector3(Mathf.Clamp(_point.x, minBounds.x, maxBounds.x), Mathf.Clamp(_point.y, minBounds.y, maxBounds.y), transform.position.z);
        return _clampedPoint;
    }

    public Vector3 GetRaycastPointOnPlane(out bool _hit)
    {
        Vector3 _lookOriginPosition = hmdTransform.position;
        Vector3 _lookDirection = hmdTransform.forward;

        float _enter = 0.0f;
        Ray _ray = new Ray(_lookOriginPosition, _lookDirection);
        _hit = plane.Raycast(_ray, out _enter);

        Vector3 _pointOnPlane = new Vector3();
        if (_hit)
        {
            Vector3 _hitPoint = _ray.GetPoint(_enter);
            Vector3 _clampedPointOnPlane = VectorHelper.Clamp(_hitPoint, minBounds, maxBounds);
            _pointOnPlane = new Vector3(_clampedPointOnPlane.x, _clampedPointOnPlane.y, _hitPoint.z);
        }

        return _pointOnPlane;
    }

    private void Awake()
    {
        BoxCollider _boxCollider = GetComponent<BoxCollider>();
        minBounds = _boxCollider.bounds.min;
        maxBounds = _boxCollider.bounds.max;

        size = new Vector2(maxBounds.x - minBounds.x, maxBounds.y - minBounds.y);
        plane = new Plane(Vector3.forward, transform.position);

        hmdTransform = Camera.main.transform;
    }

    private void Update()
    {
        bool _hit;
        Vector3 _lookPosition = GetRaycastPointOnPlane(out _hit);
        if(!_hit) { return; }

        if (LookPositionUpdatedEvent != null)
        {
            LookPositionUpdatedEvent(_lookPosition);
        }
    }
}