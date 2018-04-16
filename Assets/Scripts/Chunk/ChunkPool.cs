using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// The chunkpool has a list with all the available chunk prefabs
/// This script is responsible for Initialising the next chunk when asked
/// </summary>

public class ChunkPool : MonoBehaviour
{
    public static Action<Chunk> ChunkSpawnedAction;

    [SerializeField] private int amountOfChunks;
    [SerializeField] private float chunkStartPosition;

    [SerializeField] private List<Chunk> availableChunkPrefabs;

    private float totalChunkLengths;

    void Start()
    {
        for (int i = 0; i < amountOfChunks; i++)
        {
            SpawnChunk();
        }
    }

    private void OnRemovedChunk(Chunk _chunk)
    {
        totalChunkLengths -= _chunk.Length;
        SpawnChunk();
    }

    private void SpawnChunk()
    {
        Vector3 _spawnPosition = new Vector3(0, 0, (chunkStartPosition + totalChunkLengths));

        GameObject _spawnedChunkGameObject = Instantiate(GenerateRandomChunk().gameObject, _spawnPosition, Quaternion.identity);
        Chunk _spawnedChunk = _spawnedChunkGameObject.GetComponent<Chunk>();

        totalChunkLengths += _spawnedChunk.Length;

        ChunkSpawnedAction(_spawnedChunk);

    }

    private Chunk GenerateRandomChunk()
    {
        int _randomNumber = Random.Range(0, availableChunkPrefabs.Count);
        return availableChunkPrefabs[_randomNumber];
    }

    private void OnEnable()
    {
        ChunkMover.ChunkRemovedAction += OnRemovedChunk;
    }

    private void OnDisable()
    {
        ChunkMover.ChunkRemovedAction -= OnRemovedChunk;
    }
}