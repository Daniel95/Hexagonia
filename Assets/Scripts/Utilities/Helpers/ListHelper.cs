using System.Collections.Generic;
using UnityEngine;

public static class ListHelper
{

    public static bool AddDistinct<T>(this List<T> list, T element)
    {
        if (list.Contains(element)) { return false; }
        list.Add(element);
        return true;
    }

    public static T GetNext<T>(this List<T> list, T current, int offset, bool loop = true)
    {
        int index = list.IndexOf(current);

        index += offset;

        if (!loop && (index >= list.Count || index < 0)) { return default(T); }

        while (index < 0)
        {
            index += list.Count;
        }
        index = index % list.Count;

        return list[index];
    }

    public static T GetRandom<T>(this List<T> list)
    {
        if (list.Count == 0) { return default(T); }
        int randomIndex = Random.Range(0, list.Count);
        return list[randomIndex];
    }

    public static T GetLast<T>(this List<T> list)
    {
        if (list.Count == 0) { return default(T); }
        return list[list.Count - 1];
    }

    public static List<T> GetAtIndices<T>(this List<T> list, List<int> indices)
    {
        List<T> result = new List<T>();
        foreach (int index in indices)
        {
            if (index < 0 || index > list.Count - 1) { continue; }
        }
        indices.ForEach(x => result.Add(list[x]));
        return result;
    }

    /// <summary>Clears the list and sets its content to the content of the given list.</summary>
    public static void SetRange<T>(this List<T> list, IEnumerable<T> range)
    {
        list.Clear();
        list.AddRange(range);
    }

    public static void DebugLogEntries<T>(this List<T> list)
    {
        Debug.Log("~~ Log Entries ~~");
        list.ForEach(x => Debug.Log(x));
        Debug.Log("~~~~~~~~");
    }

    public static Vector2 CombineVectors(this List<Vector2> list)
    {
        Vector2 combinedVector = new Vector2();
        list.ForEach(x => combinedVector += x);
        return combinedVector;
    }

    public static Vector2Int CombineVectors(this List<Vector2Int> list)
    {
        Vector2Int combinedVector = new Vector2Int();
        list.ForEach(x => combinedVector += x);
        return combinedVector;
    }

    public static Vector3 CombineVectors(this List<Vector3> list)
    {
        Vector3 combinedVector = new Vector3();
        list.ForEach(x => combinedVector += x);
        return combinedVector;
    }

    public static Vector2 Average(this List<Vector2> vectors)
    {
        Vector2 combinedVector = vectors.CombineVectors();
        Vector2 center = combinedVector / vectors.Count;
        return center;
    }

    public static Vector3 Average(this List<Vector3> vectors)
    {
        Vector3 combinedVector = vectors.CombineVectors();
        Vector3 center = combinedVector / vectors.Count;
        return center;
    }

    public static Vector2 Highest(this List<Vector2> vectors)
    {
        float highestValue = float.MinValue;
        Vector2 highestVector = new Vector2();

        foreach (Vector2 vector in vectors)
        {
            float value = vector.x + vector.y;
            if (value > highestValue)
            {
                highestValue = value;
                highestVector = vector;
            }
        }

        return highestVector;
    }

    public static Vector2 Lowest(this List<Vector2> vectors)
    {
        float lowestValue = float.MaxValue;
        Vector2 lowestVector = new Vector2();

        foreach (Vector2 vector in vectors)
        {
            float value = vector.x + vector.y;
            if (value < lowestValue)
            {
                lowestValue = value;
                lowestVector = vector;
            }
        }

        return lowestVector;
    }

}
