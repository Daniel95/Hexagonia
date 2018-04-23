using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This contains the information of a specific chunk and functions to instantiate and remove itself.
/// </summary>

public class Chunk : MonoBehaviour
{
    public int Length
    {
        get
        {
            return length;
        }
    }

    [SerializeField] private List<GameObject> coinPositions;

    [SerializeField] private int amountOfCoins;

    [SerializeField] private int length;

	private void Awake()
	{
		InstantiateCoins();
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
            } while (_randomCoinPositionIndexes.Contains(_randomCoinPositionIndex));

            _randomCoinPositionIndexes.Add(_randomCoinPositionIndex);
            Instantiate(_coinPrefab, coinPositions[_randomCoinPositionIndex].transform.position, transform.rotation, transform);
        }
    }

	private void DestroyMe(Chunk _chunk)
    {
        if (_chunk == this)
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        ChunkMover.ChunkRemovedAction += DestroyMe;
    }

    private void OnDisable()
    {
        ChunkMover.ChunkRemovedAction -= DestroyMe;
    }
}
