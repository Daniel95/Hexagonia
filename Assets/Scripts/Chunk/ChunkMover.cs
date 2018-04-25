using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ChunkMover is a script responsible for moving all the spawned chunks in the level.
/// It contains a speed variable which is influenced by the progress of the game.
/// </summary>

public class ChunkMover : MonoBehaviour
{
    public static Action<Chunk> ChunkRemovedAction;

    public static ChunkMover Instance { get { return GetInstance(); } }

    public List<Chunk> CurrentChunks()
    {
        return currentChunks;
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
    private List<Chunk> currentChunks = new List<Chunk>();

	private void Update ()
	{
        Debug.Log(LevelProgess.Instance.Timer);
        if (speed < maximumSpeed)
	    {
	        float _speedOffset = maximumSpeed - minimumSpeed;
	        float _speedTimeMultiplier = LevelProgess.Instance.Timer / timeForMaximumSpeed;
	        float _speedIncrement = _speedOffset * _speedTimeMultiplier;
            speed = minimumSpeed + _speedIncrement;
        }

        for (int i = 0; i < currentChunks.Count; i++)
	    {
            if (currentChunks[i].transform.position.z <= ChunkPool.Instance.ChunksZStartPosition)
	        {
                RemoveChunk();
            }
	    }

	    for (int i = 0; i < currentChunks.Count; i++)
	    {
	        currentChunks[i].transform.Translate(Vector3.back * (speed * Time.deltaTime));
        }
    }

    private void AddChunk(Chunk _chunk)
    {
        currentChunks.Add(_chunk);
    }

    private void RemoveChunk()
    {
        Chunk _removedChunk = currentChunks[0];
        ChunkRemovedAction(_removedChunk);
        currentChunks.Remove(_removedChunk);
    }

    private void OnEnable()
    {
        ChunkPool.ChunkSpawnedAction += AddChunk;
    }

    private void OnDisable()
    {
        ChunkPool.ChunkSpawnedAction -= AddChunk;
    }

}