using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ChunkMover is a script responsible for moving all the spawned chunks in the level.
/// It contains a speed variable which is influenced by the progress of the game.
/// </summary>

public class ChunkMover : MonoBehaviour
{
    public static Action<GameObject> ChunkRemovedEvent;

    public static ChunkMover Instance { get { return GetInstance(); } }

    public int ChunkCount
    {
        get 
        {
            return currentChunks.Count;
        }
    }

    #region Singleton
    private static ChunkMover instance;

    private static ChunkMover GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<ChunkMover>();
        }
        return instance;
    }
    #endregion

    [SerializeField] private float minimumSpeed = 15;
    [SerializeField] private float maximumSpeed = 35;
    [SerializeField] private float timeForMaximumSpeed = 60;

    private float speed;
    private List<ChunkMoverEntry> currentChunks = new List<ChunkMoverEntry>();

    public GameObject GetLastestChunk()
    {
        ChunkMoverEntry _chunkMoverEntry = currentChunks[currentChunks.Count - 1];
        return _chunkMoverEntry.Chunk;
    }

    public GameObject GetLastestChunk(out int _length)
    {
        ChunkMoverEntry _chunkMoverEntry = currentChunks[currentChunks.Count - 1];
        _length = _chunkMoverEntry.Length;
        return _chunkMoverEntry.Chunk;
    }

    private void Update ()
	{
        if (speed < maximumSpeed)
	    {
	        float _speedOffset = maximumSpeed - minimumSpeed;
	        float _speedTimeMultiplier = LevelProgess.Instance.Timer / timeForMaximumSpeed;
	        float _speedIncrement = _speedOffset * _speedTimeMultiplier;
            speed = minimumSpeed + _speedIncrement;
        }

        for (int i = currentChunks.Count - 1; i >= 0; i--) 
        {
            if (currentChunks[i].Chunk.transform.position.z <= ChunkPool.Instance.ChunksZStartPosition)
            {
                RemoveFirstChunk();
            }
            else 
            {
                currentChunks[i].Chunk.transform.Translate(Vector3.back * (speed * Time.deltaTime));
            }
        }
    }

    private void AddChunk(GameObject _chunk, int _length)
    {
        ChunkMoverEntry _chunkMoverEntry = new ChunkMoverEntry
        {
            Chunk = _chunk,
            Length = _length,
        };
        currentChunks.Add(_chunkMoverEntry);
    }

    private void RemoveFirstChunk()
    {
        ChunkMoverEntry _chunkMoverEntry = currentChunks[0];
        currentChunks.Remove(_chunkMoverEntry);

        if (ChunkRemovedEvent != null) 
        {
            ChunkRemovedEvent(_chunkMoverEntry.Chunk);
        }
    }

    private void OnEnable()
    {
        ChunkPool.ChunkSpawnedEvent += AddChunk;
    }

    private void OnDisable()
    {
        ChunkPool.ChunkSpawnedEvent -= AddChunk;
    }

}

public class ChunkMoverEntry
{
    public int Length;
    public GameObject Chunk;
}