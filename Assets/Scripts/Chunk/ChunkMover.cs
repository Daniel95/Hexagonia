using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ChunkMover is a script responsible for moving all the spawned chunks in the level.
/// It contains a speed variable which is influenced by the progress of the game.
/// </summary>

public class ChunkMover : MonoBehaviour
{
    public static Action<Chunk> ChunkRemovedEvent;

    public static ChunkMover Instance { get { return GetInstance(); } }

    public int ChunkCount
    {
        get 
        {
            return currentChunks.Count;
        }
    }

    public Chunk LastestChunk 
    {
        get 
        {
            return currentChunks[currentChunks.Count - 1];
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
    private List<Chunk> currentChunks = new List<Chunk>();

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
            if (currentChunks[i].transform.position.z <= ChunkPool.Instance.ChunksZStartPosition)
            {
                RemoveChunk();
            }
            else 
            {
                currentChunks[i].transform.Translate(Vector3.back * (speed * Time.deltaTime));
            }
        }
    }

    private void AddChunk(Chunk _chunk)
    {
        currentChunks.Add(_chunk);
    }

    private void RemoveChunk()
    {
        Chunk _removedChunk = currentChunks[0];
        currentChunks.Remove(_removedChunk);

        if (ChunkRemovedEvent != null) 
        {
            ChunkRemovedEvent(_removedChunk);
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