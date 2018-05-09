﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// The chunkpool has a list with all the available chunk prefabs.
/// This script is responsible for spawning the chunks when needed. 
/// In the editor is defined which, where and how many chunks are being spawned.
/// </summary>

public class ChunkPool : MonoBehaviour
{
    public static Action<GameObject, float> ChunkSpawnedEvent;

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

    private const string CHUNK_NAME = "Chunk";
    private const string CHUNK_PATH = "Chunks/";

    [SerializeField] private int startChunksCount;
    [SerializeField] private int maxActiveChunkCount;
    [SerializeField] private float chunksZStartPosition;

    private Dictionary<ChunkType, List<ChunkDesign>> chunkListsByChunkType = new Dictionary<ChunkType, List<ChunkDesign>>();

    private void SpawnRandomStartChunks()
    {
        for (int i = 0; i < startChunksCount; i++)
        {
            SpawnRandomChunk(chunkListsByChunkType[ChunkType.Start]);
        }

        int _coreChunksLeftToSpawn = maxActiveChunkCount - startChunksCount;
        for (int i = 0; i < _coreChunksLeftToSpawn; i++) 
        {
            SpawnRandomChunk(chunkListsByChunkType[ChunkType.Core]);
        }
    }

    private void SpawnRandomCoreChunk()
    {
        SpawnRandomChunk(chunkListsByChunkType[ChunkType.Core]);
    }

    private void SpawnRandomChunk(List<ChunkDesign> chunkPrefabs)
    {
        int _randomNumber = Random.Range(0, chunkPrefabs.Count);
        ChunkDesign _chunk = chunkPrefabs[_randomNumber];
        SpawnChunk(_chunk);
    }

    private void SpawnChunk(ChunkDesign _chunkDesign)
    {
        Vector3 _spawnPosition;
        if (ChunkMover.Instance.ChunkCount != 0)
        {
            float _latestChunkLength;
            GameObject _latestChunk = ChunkMover.Instance.GetLastestChunk(out _latestChunkLength);
            float _offset = _latestChunkLength / 2 + _chunkDesign.Length / 2;
            float spawnZPosition = _latestChunk.transform.position.z + _offset;

            _spawnPosition = new Vector3(transform.position.x, transform.position.y, spawnZPosition);
        }
        else
        {
            _spawnPosition = new Vector3(transform.position.x, transform.position.y, chunksZStartPosition);
        }
        GameObject _chunkParent = ObjectPool.Instance.GetObjectForType(CHUNK_NAME, false);
        _chunkParent.transform.position = _spawnPosition;
        _chunkParent.transform.parent = transform;

        foreach (Transform _transform in _chunkDesign.ObjectsToPool)
        {
            GameObject _object = ObjectPool.Instance.GetObjectForType(_transform.name, false);
            _object.transform.parent = _chunkParent.transform;
            _object.transform.position = new Vector3(_transform.position.x, _transform.position.y, _transform.position.z + _chunkParent.transform.position.z);
            _object.transform.localScale = _transform.localScale;
            _object.transform.rotation = _transform.rotation;
        }

        if(ChunkSpawnedEvent != null)
        {
            ChunkSpawnedEvent(_chunkParent, _chunkDesign.Length);
        }
    }

    private void PoolChunkObjects(GameObject _chunk)
    {
        foreach (Transform _transform in _chunk.transform)
        {
            ObjectPool.Instance.PoolObject(_transform.gameObject);
        }
        ObjectPool.Instance.PoolObject(_chunk);
    }

    private void GetChunkListsByChunkType()
    {
        List<ChunkType> _chunkTypes = EnumHelper.GetValues<ChunkType>();

        foreach (ChunkType _chunkType in _chunkTypes)
        {
            string _path = CHUNK_PATH + _chunkType + "/";
            List<ChunkDesign> _chunks = Resources.LoadAll<ChunkDesign>(_path).ToList();
            chunkListsByChunkType.Add(_chunkType, _chunks);
        }
    }

    private void OnMoverRemovedChunk(GameObject _chunk)
    {
        PoolChunkObjects(_chunk);
        SpawnRandomCoreChunk();
    }

    private void OnPoolingCompleted() 
    {
        GetChunkListsByChunkType();
        SpawnRandomStartChunks();
    }

    private void OnEnable()
    {
        ChunkMover.ChunkRemovedEvent += OnMoverRemovedChunk;
        ObjectPool.PoolingCompletedEvent += OnPoolingCompleted;
    }

    private void OnDisable()
    {
        ChunkMover.ChunkRemovedEvent -= OnMoverRemovedChunk;
        ObjectPool.PoolingCompletedEvent -= OnPoolingCompleted;
    }

}