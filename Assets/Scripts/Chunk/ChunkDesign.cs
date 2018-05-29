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

    private const string POOLED_NAME = "Pooled";
	private const string GROUND = "Ground";

	[SerializeField] private List<GameObject> coinPositions;
    [SerializeField] private List<Transform> objectsToPool;
    [SerializeField] private GameObject ground;
    [SerializeField] private int amountOfCoins;

    private float? length;

	/// <summary>
	/// Gets the local positions of the coins
	/// </summary>
	/// <returns>_randomCoinPositions</returns>
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
        if (!gameObject.activeInHierarchy)
        {
            Debug.LogError("CAN'T DO THIS IN FOLDER!");
            return;
        }

        Transform pooled = transform.Find(POOLED_NAME);
        if (!pooled)
        {
            Debug.LogError("THERE IS NO 'POOLED' GAMEOBJECT IN THE CHUNK");
            return;
        }

        objectsToPool = new List<Transform>();
        foreach (Transform _child in pooled.FirstLayerChildren())
        {
            foreach (ObjectPool.ObjectPoolEntry _objectPoolEntry in ObjectPool.Instance.Entries)
            {
                if (_objectPoolEntry.Prefab.name == _child.name)
                {
                    objectsToPool.Add(_child);
                    if (_child.name == GROUND)
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
        if (!gameObject.activeInHierarchy)
        {
            Debug.LogError("CAN'T DO THIS IN FOLDER!");
            return;
        }

        Transform _pooled = transform.Find(POOLED_NAME);

        if (!_pooled)
        {
            Debug.LogError("THERE IS NO 'POOLED' GAMEOBJECT IN THE CHUNK");
            return;
        }

        foreach (Transform _objectToPool in _pooled.FirstLayerChildren())
        {
            ObjectPool.ObjectPoolEntry _objectPoolEntry =
                ObjectPool.Instance.Entries.Find(x => x.Prefab.name == _objectToPool.name);

            if (_objectPoolEntry == null)
            {
                Debug.LogError("GAMEOBJECT " + _objectToPool.name + " DOES NOT EXIST IN THE POOL!", _objectToPool);
                continue;
            }

            GameObject _instantiatedGameObject = Instantiate(_objectPoolEntry.Prefab, _objectToPool);
            _instantiatedGameObject.transform.parent = _pooled;
            _instantiatedGameObject.transform.name = _objectPoolEntry.Prefab.name;

            _instantiatedGameObject.transform.position = _objectToPool.position;
            _instantiatedGameObject.transform.rotation = _objectToPool.rotation;
            _instantiatedGameObject.transform.localScale = _objectToPool.localScale;

            Undo.DestroyObjectImmediate(_objectToPool.gameObject);
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