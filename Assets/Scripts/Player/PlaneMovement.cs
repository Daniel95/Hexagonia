using System;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(BoxCollider))]
public class PlaneMovement : MonoBehaviour {

    public static Action<Vector3> RaycastPointOnPlaneEvent;

    [SerializeField] private Transform hmdTransform;
    [SerializeField] private int maxDistance;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] [Range(0, 1)] private float scaledInput = 0;

    private Vector2 maxBounds;
    private Vector2 minBounds;
    private Plane plane;

    private void Awake()
    {
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        minBounds = boxCollider.bounds.min;
        maxBounds = boxCollider.bounds.max;

        plane = new Plane(Vector3.forward, transform.position);
    }

    private void Update()
    {
        bool hit;
        Vector3 planePoint = GetRaycastPointOnPlane(out hit);
        if(!hit) { return; }

        if(RaycastPointOnPlaneEvent != null)
        {
            RaycastPointOnPlaneEvent(planePoint);
        }
    }

    private Vector3 GetRaycastPointOnPlane(out bool _hit)
    {
        Vector3 lookOriginPosition = hmdTransform.position;
        Vector3 lookDirection = hmdTransform.forward;

        float enter = 0.0f;
        Ray ray = new Ray(lookOriginPosition, lookDirection);

        Vector3 pointOnPlane = new Vector3();
        _hit = plane.Raycast(ray, out enter);

        if (_hit)
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            Vector2 centerOffset = transform.position - hitPoint;
            Vector2 scaleOffset = ((Vector2)hitPoint - centerOffset) * scaledInput;
            Vector2 scaledPointOnPlane = (Vector2)hitPoint + scaleOffset;
            Vector2 clampedPointOnPlane = VectorHelper.Clamp(scaledPointOnPlane, minBounds, maxBounds);
            pointOnPlane = new Vector3(clampedPointOnPlane.x, clampedPointOnPlane.y, hitPoint.z);
        }

        return pointOnPlane;
    }

}
