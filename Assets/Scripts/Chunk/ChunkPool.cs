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
    public static Action<Chunk> ChunkSpawnedEvent;

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

    [SerializeField] private int startChunksCount;
    [SerializeField] private int maxActiveChunkCount;
    [SerializeField] private float chunksZStartPosition;
    [SerializeField] private List<Chunk> startChunkPrefabs;
    [SerializeField] private List<Chunk> coreChunkPrefabs;

    private void SpawnRandomStartChunks()
    {
        for (int i = 0; i < startChunksCount; i++)
        {
            SpawnRandomChunk(startChunkPrefabs);
        }

        int _coreChunksLeftToSpawn = maxActiveChunkCount - startChunksCount;
        for (int i = 0; i < _coreChunksLeftToSpawn; i++) 
        {
            SpawnRandomChunk(coreChunkPrefabs);
        }
    }

    private void SpawnRandomCoreChunk()
    {
        SpawnRandomChunk(coreChunkPrefabs);
    }

    private void SpawnRandomChunk(List<Chunk> chunkPrefabs)
    {
        int _randomNumber = Random.Range(0, chunkPrefabs.Count);
        Chunk _chunk = chunkPrefabs[_randomNumber];
        SpawnChunk(_chunk.gameObject, _chunk.Length);
    }

    private void SpawnChunk(GameObject _chunkPrefab, float _chunkLength)
    {
        Vector3 _spawnPosition;

        if (ChunkMover.Instance.ChunkCount != 0)
        {
            Chunk _latestChunk = ChunkMover.Instance.LastestChunk;
            float _offset = _latestChunk.Length / 2 + _chunkLength / 2;
            float spawnZPosition = _latestChunk.transform.position.z + _offset;
            _spawnPosition = new Vector3(transform.position.x, transform.position.y, spawnZPosition);
        }
        else
        {
            _spawnPosition = new Vector3(transform.position.x, transform.position.y, chunksZStartPosition);
        }

        GameObject _spawnedChunkGameObject = Instantiate(_chunkPrefab, _spawnPosition, Quaternion.identity, transform);
        Chunk _spawnedChunk = _spawnedChunkGameObject.GetComponent<Chunk>();

        if(ChunkSpawnedEvent != null)
        {
            ChunkSpawnedEvent(_spawnedChunk);
        }
    }

    private void OnRemovedChunk(Chunk _chunk)
    {
        SpawnRandomCoreChunk();
    }

    private void Start() 
    {
        SpawnRandomStartChunks();
    }

    private void OnEnable()
    {
        ChunkMover.ChunkRemovedEvent += OnRemovedChunk;
    }

    private void OnDisable()
    {
        ChunkMover.ChunkRemovedEvent -= OnRemovedChunk;
    }

}