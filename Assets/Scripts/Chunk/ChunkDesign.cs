using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

/// <summary>
/// This contains the information of a specific chunk and functions to instantiate and remove itself.
/// </summary>

[RequireComponent(typeof(Renderer))]
public class ChunkDesign : MonoBehaviour
{
    public List<Transform> ObjectsToPool { get { return objectsToPool; } }

    public float Length
    {
        get
        {
            if(length == null)
            {
                length = GetLength();
            }
            return (float)length;
        }
    }

    [SerializeField] private List<GameObject> coinPositions;
    [SerializeField] private List<Transform> objectsToPool;
    [SerializeField] private GameObject ground;
    [SerializeField] private int amountOfCoins;

    private float? length;

    public List<Vector3> GetCoinLocalPositions()
    {
        List<Vector3> _randomCoinPositions = new List<Vector3>();

        for (int i = 0; i < amountOfCoins; i++)
        {
            Vector3 _coinPosition;
            do
            {
                int _randomCoinPositionIndex = Random.Range(0, coinPositions.Count);
                _coinPosition = coinPositions[_randomCoinPositionIndex].transform.position;
            }
            while (_randomCoinPositions.Contains(_coinPosition));

            _randomCoinPositions.Add(_coinPosition);
        }

        return _randomCoinPositions;
    }

    [ContextMenu("UpdatePoolableObjects")]
    private void UpdatePoolableObjects()
    {
        objectsToPool = new List<Transform>();
        foreach (Transform child in transform.AllChildren())
        {
            foreach (ObjectPool.ObjectPoolEntry objectPoolEntry in ObjectPool.Instance.Entries)
            {
                if (objectPoolEntry.Prefab.name == child.name)
                {
                    objectsToPool.Add(child);
                    break;
                }
            }
        }

#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
#endif
    }

    private float GetLength()
    {
        Renderer _renderer = ground.GetComponent<Renderer>();
        float _minZ = _renderer.bounds.min.z;
        float _maxZ = _renderer.bounds.max.z;
        float _length = _maxZ - _minZ;
        return _length;
    }

    private void OnValidate()
    {
        if(amountOfCoins > coinPositions.Count)
        {
            Debug.LogWarning("amountOfCoins (" + amountOfCoins + ") is higher then the amount of coinPositions (" + coinPositions.Count + ") in chunk " + name, this);
        }
    }

}
