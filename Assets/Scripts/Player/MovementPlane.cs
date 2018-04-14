using System;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(BoxCollider))]
public class MovementPlane : MonoBehaviour {

    public static Action<Vector3> RaycastPointOnPlaneEvent;

    [SerializeField] private Transform hmdTransform;
    [SerializeField] private int maxDistance;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] [Range(0, 1)] private float scaledInput = 0;

    private Vector2 maxBounds;
    private Vector2 minBounds;

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
        Vector3 lookDirection = hmdTransform.TransformDirection(Vector3.forward);
        RaycastHit hitInfo;
        Vector3 pointOnPlane = Vector3.zero;

        _hit = Physics.Raycast(hmdTransform.position, lookDirection, out hitInfo, maxDistance, layerMask);

        if (_hit)
        {
            Vector3 hitPoint = hitInfo.point;
            Vector2 centerOffset = transform.position - hitPoint;
            Vector2 scaleOffset = ((Vector2)hitPoint - centerOffset) * scaledInput;
            Vector2 scaledPointOnPlane = (Vector2)hitPoint + scaleOffset;
            Vector2 clampedPointOnPlane = VectorHelper.Clamp(scaledPointOnPlane, minBounds, maxBounds);
            pointOnPlane = new Vector3(clampedPointOnPlane.x, clampedPointOnPlane.y, hitPoint.z);
        } 

        return pointOnPlane;
    }

    private void Awake() {
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        minBounds = boxCollider.bounds.min;
        maxBounds = boxCollider.bounds.max;
    }

}
