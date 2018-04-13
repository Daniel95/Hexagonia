using System;
using UnityEngine;

public class MovementPlane : MonoBehaviour {

    public static Action<Vector3> RaycastPointOnPlaneEvent;

    [SerializeField] private Transform hmdTransform;
    [SerializeField] private int maxDistance;
    [SerializeField] private LayerMask layerMask;

    private void Update()
    {
        Vector3 planePoint = GetRaycastPointOnPlane();

        if(RaycastPointOnPlaneEvent != null)
        {
            RaycastPointOnPlaneEvent(planePoint);
        }
    }

    private Vector3 GetRaycastPointOnPlane()
    {
        Vector3 direction = hmdTransform.TransformDirection(Vector3.forward);
        RaycastHit hitInfo;
        Vector3 planePoint = Vector3.zero;

        if (Physics.Raycast(hmdTransform.position, direction, out hitInfo, maxDistance, layerMask))
        {
            planePoint = hitInfo.point;
        }

        return planePoint;
    }

}
