using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// The chunkpool has a list with all the available chunk prefabs.
/// This script is responsible for spawning the chunks when needed. 
/// In the editor is defined which, where and how many chunks are being spawned.
/// </summary>

public class ChunkPool : MonoBehaviour
{
    public static Action<Chunk> ChunkSpawnedAction;

    public static ChunkPool Instance { get { return GetInstance(); } }

    public float ChunksZStartPosition
    {
        get
        {
            return chunksZStartPosition;
        }
    }

    #region Singleton
    private static ChunkPool instance;

    private static ChunkPool GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<ChunkPool>();
        }
        return instance;
    }
    #endregion

    [SerializeField] private int amountOfChunks;
    [SerializeField] private float chunksZStartPosition;
    [SerializeField] private List<Chunk> availableChunkPrefabs;

    private Vector3 spawnPosition;

    private void Start()
    {
        for (int i = 0; i < amountOfChunks; i++)
        {
            SpawnChunk();
        }
    }

    private void OnRemovedChunk(Chunk _chunk)
    {
        SpawnChunk();
    }

    private void SpawnChunk()
    {
        float _lengthOfRandomChunk;
        GameObject _randomChunkPrefab = GetRandomChunkPrefab(out _lengthOfRandomChunk);

        if (ChunkMover.Instance.CurrentChunks().Count != 0)
        {
            List<Chunk> currentChunks = ChunkMover.Instance.CurrentChunks();
            Chunk _newestChunk = currentChunks[ChunkMover.Instance.CurrentChunks().Count - 1];
            float spawnZPosition = _newestChunk.transform.position.z + _newestChunk.Length / 2 + _lengthOfRandomChunk / 2;
            spawnPosition = new Vector3(transform.position.x, transform.position.y, spawnZPosition);
        }
        else
        {
            spawnPosition = new Vector3(transform.position.x, transform.position.y, chunksZStartPosition);
        }

        GameObject _spawnedChunkGameObject = Instantiate(_randomChunkPrefab, spawnPosition, Quaternion.identity, transform);
        Chunk _spawnedChunk = _spawnedChunkGameObject.GetComponent<Chunk>();

        ChunkSpawnedAction(_spawnedChunk);
    }

    private GameObject GetRandomChunkPrefab(out float length)
    {
        int _randomNumber = Random.Range(0, availableChunkPrefabs.Count);
        Chunk chunk = availableChunkPrefabs[_randomNumber];
        length = chunk.Length;
        return availableChunkPrefabs[_randomNumber].gameObject;
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