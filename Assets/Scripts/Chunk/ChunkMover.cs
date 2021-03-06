﻿using System;
using System.Collections;
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

	public int ChunkCount { get { return currentChunks.Count; } }
	public bool MoveChunks { get { return moveChunks; } set { moveChunks = value; } }
	public float Speed { get { return speed; } }
	public float SpeedProgress { get { return Mathf.Clamp01(Progression.Timer / timeForMaximumSpeed); } }

    [SerializeField] private float stopTime;
	[SerializeField] private float minimumSpeed = 15;
    [SerializeField] private float maximumSpeed = 35;
    [SerializeField] public float timeForMaximumSpeed = 60;
    [SerializeField] private bool moveChunks = true;

    private bool stopping = false;
    private float speed;
    private List<ChunkByChunkLengthPair> currentChunks = new List<ChunkByChunkLengthPair>();

	/// <summary>
	/// Gets the latest chunk in the currentChunks list.
	/// </summary>
	/// <returns>_chunkMoverEntry.Chunk</returns>
    public GameObject GetLastestChunk()
    {
        ChunkByChunkLengthPair _chunkMoverEntry = currentChunks[currentChunks.Count - 1];
        return _chunkMoverEntry.Chunk;
    }

	/// <summary>
	/// Gets the latest chunk in the currentChunks list.
	/// </summary>
	/// <param name="_length"></param>
	/// <returns>_chunkMoverEntry.Chunk</returns>
	public GameObject GetLastestChunk(out float _length)
    {
        ChunkByChunkLengthPair _chunkMoverEntry = currentChunks[currentChunks.Count - 1];
        _length = _chunkMoverEntry.Length;
        return _chunkMoverEntry.Chunk;
    }

	private void Update ()
	{
	    if (!moveChunks) { return; }

	    if (speed < maximumSpeed && !stopping)
	    {
	        float _speedOffset = maximumSpeed - minimumSpeed;
	        float _speedIncrement = _speedOffset * SpeedProgress;
            speed = minimumSpeed + _speedIncrement;
        }

        for (int i = currentChunks.Count - 1; i >= 0; i--) 
        {
            if (currentChunks[i].Chunk.transform.position.z <= ChunkSpawner.Instance.ChunksZStartPosition)
            {
                RemoveFirstChunk();
            }
            else 
            {
                currentChunks[i].Chunk.transform.Translate(Vector3.back * (speed * Time.deltaTime));
            }
        }
    }

    private void AddChunk(GameObject _chunk, float _length)
    {
        ChunkByChunkLengthPair _chunkMoverEntry = new ChunkByChunkLengthPair
        {
            Chunk = _chunk,
            Length = _length,
        };
        currentChunks.Add(_chunkMoverEntry);
    }

    private void RemoveFirstChunk()
    {
        ChunkByChunkLengthPair _chunkMoverEntry = currentChunks[0];
        currentChunks.Remove(_chunkMoverEntry);

        if (ChunkRemovedEvent != null) 
        {
            ChunkRemovedEvent(_chunkMoverEntry.Chunk);
        }
    }

    private void StopChunkOverTime()
    {
        StartCoroutine(StopChunkMovementUpdate());
    }

    private IEnumerator StopChunkMovementUpdate()
    {
        stopping = true;
        float _maxSpeed = speed;
        float _timeSpend = 0;

        while (speed > 0.1)
        {
            speed = _maxSpeed - (_maxSpeed * (_timeSpend / stopTime));
            _timeSpend += Time.deltaTime;
            yield return new WaitForEndOfFrame(); ;
        }

        speed = 0;
        moveChunks = false;
    }

    private void OnEnable()
    {
        ChunkSpawner.ChunkSpawnedEvent += AddChunk;
        PlayerCollisions.DiedEvent += StopChunkOverTime;
    }

    private void OnDisable()
    {
        ChunkSpawner.ChunkSpawnedEvent -= AddChunk;
        PlayerCollisions.DiedEvent -= StopChunkOverTime;
    }
}