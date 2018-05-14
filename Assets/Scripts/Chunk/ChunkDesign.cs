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

    private string POOLED_NAME = "Pooled";

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

#if UNITY_EDITOR
    [ContextMenu("UpdatePoolableObjects")]
    private void UpdatePoolableObjects()
    {
        Transform pooled = transform.Find(POOLED_NAME);

        objectsToPool = new List<Transform>();
        foreach (Transform _child in pooled.FirstLayerChildren())
        {
            foreach (ObjectPool.ObjectPoolEntry _objectPoolEntry in ObjectPool.Instance.Entries)
            {
                if (_objectPoolEntry.Prefab.name == _child.name)
                {
                    objectsToPool.Add(_child);
                    if (_child.name == "Ground")
                    {
                        ground = _child.gameObject;
                    }

                    break;
                }
            }
        }
        EditorUtility.SetDirty(this);
    }

    [ContextMenu("UpdateChunkFromPool")]
    private void UpdateChunkFromPool()
    {
        Transform pooled = transform.Find(POOLED_NAME);

        foreach (Transform _objectToPool in pooled.FirstLayerChildren())
        {
            ObjectPool.ObjectPoolEntry _objectPoolEntry = ObjectPool.Instance.Entries.Find(x => x.Prefab.name == _objectToPool.name);

            if (_objectPoolEntry == null)
            {
                Debug.LogError("GameObject " + _objectToPool.name + " does not exists in pool!", _objectToPool);
                continue; 
            }

            GameObject _instantiatedGameObject = Instantiate(_objectPoolEntry.Prefab, _objectToPool);
            _instantiatedGameObject.transform.parent = pooled;
            _instantiatedGameObject.transform.name = _objectPoolEntry.Prefab.name;

            _instantiatedGameObject.transform.position = _objectToPool.position;
            _instantiatedGameObject.transform.rotation = _objectToPool.rotation;
            _instantiatedGameObject.transform.localScale = _objectToPool.localScale;

            DestroyImmediate(_objectToPool.gameObject);
        }
        EditorUtility.SetDirty(this);
    }
#endif

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
