using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PhysicsHelper
{

    public static List<Collider> GetCollidersAligningOnAxis(Vector3 position, Vector3 alignAxises, float length = float.MaxValue)
    {
        float halfLength = length / 2;
        Vector3 size = new Vector3(halfLength * Mathf.Abs(alignAxises.x), halfLength * Mathf.Abs(alignAxises.y), halfLength * Mathf.Abs(alignAxises.z));

        Collider[] colliders = Physics.OverlapBox(position, size);
        return colliders.ToList();
    }

    public static List<Collider> GetCollidersAligningOnAxis(Vector3 position, Vector3 alignAxises, float length, LayerMask layerMask)
    {
        float halfLength = length / 2;
        Vector3 size = new Vector3(halfLength * Mathf.Abs(alignAxises.x), halfLength * Mathf.Abs(alignAxises.y), halfLength * Mathf.Abs(alignAxises.z));

        Collider[] colliders = Physics.OverlapBox(position, size, new Quaternion(), layerMask);
        return colliders.ToList();
    }

    public static List<Collider> GetCollidersAligningOnAxis(Vector3 position, Vector3 alignAxises, float length, LayerMask layerMask, QueryTriggerInteraction queryTriggerInteraction)
    {
        float halfLength = length / 2;
        Vector3 size = new Vector3(halfLength * Mathf.Abs(alignAxises.x), halfLength * Mathf.Abs(alignAxises.y), halfLength * Mathf.Abs(alignAxises.z));

        Collider[] colliders = Physics.OverlapBox(position, size, new Quaternion(), layerMask, queryTriggerInteraction);
        return colliders.ToList();
    }

}
