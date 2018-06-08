using System.Collections.Generic;
using UnityEngine;

public class DebugHelper
{

    private const string DEBUG_VISUAL_PREFAB_PATH = "Debug/DebugPositionVisual";

    private static GameObject prefab;

    private static Dictionary<string, GameObject> positionVisualsById = new Dictionary<string, GameObject>();

    public static GameObject SetDebugPosition(Vector3 position, string id, bool printGameObject = false, Color? color = null)
    {
        if (color == null)
        {
            color = Color.magenta;
        }

        GameObject positionVisual;
        if (positionVisualsById.ContainsKey(id))
        {
            positionVisual = positionVisualsById[id].gameObject;
            positionVisual.transform.position = position;
        }
        else
        {
            GameObject prefab = GetDebugVisualPrefab();
            positionVisual = Object.Instantiate(prefab, position, new Quaternion());
            positionVisual.name = "Debug " + id;
            positionVisual.GetComponent<Renderer>().material.color = (Color)color;
            positionVisualsById.Add(id, positionVisual);
        }

        if (printGameObject)
        {
            Debug.Log("Placed " + positionVisualsById[id].name + " at " + position, positionVisualsById[id]);
        }

        return positionVisual;
    }

    public static void RemoveDebugPosition(string id)
    {
        Object.Destroy(positionVisualsById[id].gameObject);
        positionVisualsById.Remove(id);
    }

    public static void RemoveAllDebugPositions()
    {
        foreach (GameObject debugVisualGameObject in positionVisualsById.Values)
        {
            Object.Destroy(debugVisualGameObject);
        }
        positionVisualsById.Clear();
    }

    public static void LogPreciseVector(Vector2 vector, string message = null)
    {
        string log = "(" + vector.x + ", " + vector.y + ")";
        Debug.Log(message + " " + log);
    }

    public static void LogPreciseVector(Vector3 vector, string message = null)
    {
        string log = "(" + vector.x + ", " + vector.y + ", " + vector.z + ")";
        Debug.Log(message + " " + log);
    }

    private static GameObject GetDebugVisualPrefab()
    {
        if (prefab == null)
        {
            prefab = Resources.Load<GameObject>(DEBUG_VISUAL_PREFAB_PATH);
        }
        return prefab;
    }

}
