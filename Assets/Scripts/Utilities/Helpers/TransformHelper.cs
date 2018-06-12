using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class TransformHelper
{

    public static List<Transform> FirstLayerChildren(this Transform transform)
    {
        List<Transform> firstLayerChildren = new List<Transform>();
        foreach (Transform child in transform)
        {
            firstLayerChildren.Add(child);
        }
        return firstLayerChildren;
    }

    public static List<Transform> LastLayerChildren(this Transform transform)
    {
        List<Transform> lastLayerChildren = new List<Transform>();
        foreach (Transform child in transform.GetComponentsInChildren<Transform>())
        {
            if (child.childCount == 0)
            {
                lastLayerChildren.Add(child);
            }
        }
        return lastLayerChildren;
    }

    public static List<Transform> ChildrenWithMultipleChildren(this Transform transform)
    {
        List<Transform> intersectionChildren = new List<Transform>();
        foreach (Transform child in transform.GetComponentsInChildren<Transform>())
        {
            if (child.childCount > 1)
            {
                intersectionChildren.Add(child);
            }
        }
        return intersectionChildren;
    }

    public static List<Transform> AllChildren(this Transform transform)
    {
        return transform.GetComponentsInChildren<Transform>().ToList();
    }

    public static List<List<Transform>> HierachyLines(this Transform transform)
    {
        List<Transform> childrenWithMultipleChildren = transform.ChildrenWithMultipleChildren();
        List<Transform> lastLayerChildren = transform.LastLayerChildren();
        List<Transform> lineEndPoints = lastLayerChildren.Concat(childrenWithMultipleChildren).ToList();

        List<List<Transform>> hierachyLines = new List<List<Transform>>();

        foreach (Transform lineEndPoint in lineEndPoints)
        {
            List<Transform> hierachyLine = new List<Transform>() {
                lineEndPoint,
            };

            Transform linetransformParent = lineEndPoint.parent;
            while (linetransformParent != null && linetransformParent.childCount <= 1 && linetransformParent != transform)
            {
                hierachyLine.Add(linetransformParent);
                linetransformParent = linetransformParent.parent;
            }

            hierachyLine.Add(linetransformParent);
            hierachyLine.Reverse();

            hierachyLines.Add(hierachyLine);
        }

        return hierachyLines;
    }

    public static void ResetLocal(this Transform transform)
    {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
    }
}
