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

    private static ChunkPool instance;

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
        if (ChunkMover.Instance.CurrentChunks().Count != 0)
        {
            Chunk _newestChunk = ChunkMover.Instance.CurrentChunks()[ChunkMover.Instance.CurrentChunks().Count-1];
            spawnPosition = new Vector3(transform.position.x, transform.position.y, _newestChunk.transform.position.z +_newestChunk.Length);
        }
        else
        {
            spawnPosition = new Vector3(transform.position.x, transform.position.y, chunksZStartPosition);
        }

        GameObject _spawnedChunkGameObject = Instantiate(GenerateRandomChunk().gameObject, spawnPosition, Quaternion.identity, transform);
        Chunk _spawnedChunk = _spawnedChunkGameObject.GetComponent<Chunk>();

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

    private static ChunkPool GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<ChunkPool>();
        }
        return instance;
    }
}