using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This contains the information of a specific chunk and functions to instantiate and remove itself.
/// </summary>

[RequireComponent(typeof(Renderer))]
public class Chunk : MonoBehaviour
{

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
    [SerializeField] private GameObject ground;
    [SerializeField] private int amountOfCoins;

    private float? length;

	public void DestroyChunk()
    {
        Destroy(gameObject);
    }

    private void Awake()
	{
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
        ChunkMover.ChunkRemovedEvent += Destroy;
    }

    private void OnDisable()
    {
        ChunkMover.ChunkRemovedEvent -= Destroy;
    }

    private void OnValidate()
    {
        if(amountOfCoins >= coinPositions.Count)
        {
            Debug.LogWarning("amountOfCoins (" + amountOfCoins + ") is higher then the amount of coinPositions (" + coinPositions.Count + ") in chunk " + name, this);
        }
    }

}
