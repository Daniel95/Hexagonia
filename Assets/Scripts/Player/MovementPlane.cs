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

    /*
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
    */

    private void Update() {
        Vector3 planePoint = GetPointOnPlane();

        Debug.Log(planePoint);

        if (RaycastPointOnPlaneEvent != null) {
            RaycastPointOnPlaneEvent(planePoint);
        }
    }

    private Vector3 GetPointOnPlane() {
        Vector3 lookDirection = hmdTransform.TransformDirection(Vector3.forward);
        Vector3 planePosition = transform.position;
        Vector3 lookOriginPosition = hmdTransform.position;

        Vector2 lookOriginPositionFromTop = new Vector2(lookOriginPosition.x, lookOriginPosition.z);
        Vector2 lookDirectionFromTop = new Vector2(lookDirection.x, lookDirection.z);
        Vector2 planePositionFromTop = new Vector2(planePosition.x, planePosition.z);
        Vector2 planeDirectionFromTop = Vector2.right;

        Vector2 topIntersection = MathHelper.GetPointOfIntersection(lookOriginPositionFromTop, planePositionFromTop, lookDirectionFromTop, planeDirectionFromTop);

        Vector2 lookOriginPositionFromRight = new Vector2(lookOriginPosition.z, lookOriginPosition.y);
        Vector2 lookDirectionFromRight = new Vector2(lookDirection.z, lookDirection.y);
        Vector2 planePositionFromRight = new Vector2(planePosition.z, planePosition.y);
        Vector2 planeDirectionFromRight = Vector2.up;

        Vector2 rightIntersection = MathHelper.GetPointOfIntersection(lookOriginPositionFromRight, planePositionFromRight, lookDirectionFromRight, planeDirectionFromRight);

        Vector3 pointOnPlane = new Vector3(topIntersection.x, rightIntersection.y, planePosition.z);

        return pointOnPlane;
    }

    private void Awake() {
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        minBounds = boxCollider.bounds.min;
        maxBounds = boxCollider.bounds.max;
    }

}
