using System;
using UnityEngine;

/// <summary>
/// Sends a raycast to a plane, on hit it sends a event to the InstantPlaneMovement.cs what moves the player.
/// </summary>
//[RequireComponent(typeof(MeshRenderer))]
//[RequireComponent(typeof(BoxCollider))]
public class LookPositionOnPlane //: MonoBehaviour
{
    public static LookPositionOnPlane Instance { get { return GetInstance(); } }
    /// <summary>
    /// Parameters: Position, Delta
    /// </summary>
    public static Action<Vector3> LookPositionUpdatedEvent;
    public static Action InitiatedEvent;

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

    public Vector3 MinBounds { get { return minBounds; } }
    public Vector3 MaxBounds { get { return maxBounds; } }
    public Vector3 Size { get { return size; } }

	private Transform hmdTransform;
	private Vector3 maxBounds;
    private Vector3 minBounds;
    private Vector2 size;
    private Plane plane;
    private int latestCalculatedFrame;
    private Vector3 lookPosition;

    /// <summary>
    /// Returns a value that is within the look plane.
    /// </summary>
    /// <param name="_point"></param>
    /// <returns></returns>
    public Vector3 ClampToPlane(Vector3 _point)
    {
        Vector3 _clampedPoint = VectorHelper.Clamp(_point, minBounds, maxBounds);
        return _clampedPoint;
    }

    /// <summary>
    /// Returns the center of the camera view that intersects with the look plane.
    /// </summary>
    /// <param name="_hit"></param>
    /// <returns></returns>
    public Vector3 GetLookPosition(out bool _hit)
    {
        if (latestCalculatedFrame == Time.frameCount)
        {
            _hit = true;
            return lookPosition;
        }
        latestCalculatedFrame = Time.frameCount;

        Vector3 _lookOriginPosition = hmdTransform.position;
        Vector3 _lookDirection = hmdTransform.forward;

        float _enter = 0.0f;
        Ray _ray = new Ray(_lookOriginPosition, _lookDirection);
        _hit = plane.Raycast(_ray, out _enter);

        if (_hit)
        {
            Vector3 _hitPoint = _ray.GetPoint(_enter);
            Vector3 _clampedPointOnPlane = VectorHelper.Clamp(_hitPoint, minBounds, maxBounds);
            lookPosition = new Vector3(_clampedPointOnPlane.x, _clampedPointOnPlane.y, _hitPoint.z);
        }

        return lookPosition;
    }

    private void Start()
    {
        BoxCollider _boxCollider = GetComponent<BoxCollider>();
        minBounds = _boxCollider.bounds.min;
        maxBounds = _boxCollider.bounds.max;

        size = new Vector2(maxBounds.x - minBounds.x, maxBounds.y - minBounds.y);
        plane = new Plane(Vector3.forward, transform.position);

        hmdTransform = Camera.main.transform;

        if(InitiatedEvent != null)
        {
            InitiatedEvent();
        }
    }
}