using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script contains the information of a specific chunk.
/// More specific how long the chunk is and where the available coin positions are
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

    [SerializeField] private int length;

    private void InstantiateCoins()
    {

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
