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

    [SerializeField] [Range(0, 1)] private float scaledInput = 0;

	private Transform hmdTransform;
	private Vector2 maxBounds;
    private Vector2 minBounds;
    private Vector2 size;
    private Plane plane;

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
        Vector3 _planePoint = GetRaycastPointOnPlane(out _hit);
        if(!_hit) { return; }

        if(LookPositionUpdatedEvent != null)
        {
            LookPositionUpdatedEvent(_planePoint);
        }
    }

    private Vector3 GetRaycastPointOnPlane(out bool _hit)
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
            Vector2 _centerOffset = transform.position - _hitPoint;
            Vector2 _scaleOffset = ((Vector2)_hitPoint - _centerOffset) * scaledInput;
            Vector2 _scaledPointOnPlane = (Vector2)_hitPoint + _scaleOffset;
            Vector2 _clampedPointOnPlane = VectorHelper.Clamp(_scaledPointOnPlane, minBounds, maxBounds);
            _pointOnPlane = new Vector3(_clampedPointOnPlane.x, _clampedPointOnPlane.y, _hitPoint.z);
        }

        return _pointOnPlane;
    }

    private void StopMovementOnPlane()
    {
        enabled = false;
    }

    private void OnEnable()
    {
        Player.DiedEvent += StopMovementOnPlane;
    }

    private void OnDisable()
    {
        Player.DiedEvent -= StopMovementOnPlane;
    }
}