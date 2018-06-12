using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// The chunkpool has a list with all the available chunk prefabs.
/// This script is responsible for spawning the chunks when needed. 
/// In the editor is defined which, where and how many chunks are being spawned.
/// </summary>
public class ChunkSpawner : MonoBehaviour
{
    public static Action<GameObject, float> ChunkSpawnedEvent;

    public static ChunkSpawner Instance { get { return GetInstance(); } }

    public float ChunksZStartPosition { get { return chunksZStartPosition; } }

    #region Singleton
    private static ChunkSpawner instance;

    private static ChunkSpawner GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<ChunkSpawner>();
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

    private List<Transform> objectsToPool = new List<Transform>();
    private List<Transform> objectsToInstance = new List<Transform>();

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

    private void SpawnRandomChunk(List<ChunkDesign> _chunkPrefabs)
    {
        int _randomNumber = Random.Range(0, _chunkPrefabs.Count);
        ChunkDesign _chunk = _chunkPrefabs[_randomNumber];
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
            float _spawnZPosition = _latestChunk.transform.position.z + _offset;

            _spawnPosition = new Vector3(transform.position.x, transform.position.y, _spawnZPosition);
        }
        else
        {
            _spawnPosition = new Vector3(transform.position.x, transform.position.y, chunksZStartPosition);
        }

        GameObject _chunkParent = ObjectPool.Instance.GetObjectForType(CHUNK_NAME, false);
        _chunkParent.transform.position = _spawnPosition;
        _chunkParent.transform.parent = transform;
        _chunkParent.name = _chunkDesign.name;

        if (SystemInfo.supportsInstancing)
        {
            objectsToInstance = _chunkDesign.ObjectsToInstance;
            objectsToPool = _chunkDesign.ObjectsToPool.Except(objectsToInstance).ToList();
        }
        else
        {
            objectsToPool = _chunkDesign.ObjectsToPool;
        }

        foreach (Transform _transform in objectsToInstance)
        {
            GPUInstancing.Instance.AddTransform(_transform, _chunkParent.transform);
        }

        foreach (Transform _transform in objectsToPool)
        {
            GameObject _object = ObjectPool.Instance.GetObjectForType(_transform.name, false);
            _object.transform.parent = _chunkParent.transform;
            _object.transform.position = new Vector3(_transform.position.x, _transform.position.y, _transform.position.z + _chunkParent.transform.position.z);
            _object.transform.localScale = _transform.localScale;
            _object.transform.rotation = _transform.rotation;
        }

        SpawnCoins(_chunkDesign, _chunkParent.transform);

        if (ChunkSpawnedEvent != null)
        {
            ChunkSpawnedEvent(_chunkParent, _chunkDesign.Length);
        }
    }

    private void PoolChunkObjects(GameObject _chunk)
    {
        List<Transform> _children = _chunk.transform.FirstLayerChildren();
        for (int i = _children.Count - 1; i >= 0; i--) {
            ObjectPool.Instance.PoolObject(_children[i].gameObject);
        }

        if (SystemInfo.supportsInstancing)
        {
            GPUInstancing.Instance.RemoveTransformsByParent(_chunk.transform);
        }

        Destroy(_chunk);
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

    private void SpawnCoins(ChunkDesign _chunkDesign, Transform _chunkParent)
    {
        List<Vector3> _coinLocalPositions = _chunkDesign.GetCoinLocalPositions();
        List<CoinType> _coinTypes = CoinSpawnChancesByTimeLibrary.Instance.GetCoinTypesToSpawn(_coinLocalPositions.Count, Progression.Timer);

        for (int i = 0; i < _coinTypes.Count; i++)
        {
            CoinType _coinType = _coinTypes[i];
            Vector3 _localPosition = _coinLocalPositions[i];

            GameObject _coinPrefab = CoinPrefabByCoinTypeLibrary.Instance.GetCoinPrefab(_coinType);
            GameObject _coinGameObject = ObjectPool.Instance.GetObjectForType(_coinPrefab.name, false);
            _coinGameObject.transform.parent = _chunkParent.transform;
            _coinGameObject.transform.position = new Vector3(_localPosition.x, _localPosition.y, _localPosition.z + _chunkParent.transform.position.z);
        }
    }

    private void OnMoverRemovedChunk(GameObject _chunk)
    {
        PoolChunkObjects(_chunk);
        SpawnRandomCoreChunk();
    }

    private void OnPoolInitialisationCompleted() 
    {
        GetChunkListsByChunkType();
        SpawnRandomStartChunks();
    }

    private void OnEnable()
    {
        ChunkMover.ChunkRemovedEvent += OnMoverRemovedChunk;
        ObjectPool.PoolingInitialisationCompletedEvent += OnPoolInitialisationCompleted;
    }

    private void OnDisable()
    {
        ChunkMover.ChunkRemovedEvent -= OnMoverRemovedChunk;
        ObjectPool.PoolingInitialisationCompletedEvent -= OnPoolInitialisationCompleted;
    }
}