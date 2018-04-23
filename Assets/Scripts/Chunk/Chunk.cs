using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script contains the information of a specific chunk and a function to remove itself.
/// More specific how long the chunk is and where the available coin positions are.
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

    private float? length;

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

		for (int i = 0; i < coinPositions.Count; i++)
		{
			Instantiate(_coinPrefab, coinPositions[i].transform.position, transform.rotation, transform);
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
