using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
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

	public void DestroyChunk(GameObject _chunk)
    {
        if (_chunk == this)
        {
            Destroy(gameObject);
        }
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

    private void Awake()
	{
        CoroutineHelper.Delay(2, UpdatePoolableObjects);


        InstantiateCoins();
	}

    private float GetLength()
    {
        Renderer _renderer = ground.GetComponent<Renderer>();
        float _minZ = _renderer.bounds.min.z;
        float _maxZ = _renderer.bounds.max.z;
        float _length = _maxZ - _minZ;
        return _length;
    }

    private void InstantiateCoins()
    {
		CoinType _coinType = CoinTypeByTimeLibrary.Instance.GetCoinType(LevelProgess.Instance.Timer);
		GameObject _coinPrefab = CoinPrefabByCoinTypeLibrary.Instance.GetCoinPrefab(_coinType);

        List<int> _randomCoinPositionIndexes = new List<int>();

        for (int i = 0; i < amountOfCoins; i++)
        {
            int _randomCoinPositionIndex;
            do
            {
                _randomCoinPositionIndex = Random.Range(0, coinPositions.Count);
            }
            while (_randomCoinPositionIndexes.Contains(_randomCoinPositionIndex));

            _randomCoinPositionIndexes.Add(_randomCoinPositionIndex);
            Instantiate(_coinPrefab, coinPositions[_randomCoinPositionIndex].transform.position, transform.rotation, transform);
        }
    }

    private void OnEnable()
    {
        ChunkMover.ChunkRemovedEvent += DestroyChunk;
    }

    private void OnDisable()
    {
        ChunkMover.ChunkRemovedEvent -= DestroyChunk;
    }

    private void OnValidate()
    {
        if(amountOfCoins >= coinPositions.Count)
        {
            Debug.LogWarning("amountOfCoins (" + amountOfCoins + ") is higher then the amount of coinPositions (" + coinPositions.Count + ") in chunk " + name, this);
        }
    }

}
