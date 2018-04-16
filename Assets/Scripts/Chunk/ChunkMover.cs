using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ChunkMover is a script responsible for moving all the spawned chunks in the level
/// It contains a speed variable which is influenced by the progress of the game
/// </summary>

public class ChunkMover : MonoBehaviour
{
    public static Action<Chunk> ChunkRemovedAction;

    [SerializeField] private float speed;

    [SerializeField] private float despawnZPosition;

    private List<Chunk> currentChunks = new List<Chunk>();
	
	void Update () {
	    for (int i = 0; i < currentChunks.Count; i++)
	    {
	        if (currentChunks[i].transform.position.z < despawnZPosition)
	        {
	            RemoveChunk();
	        }
            currentChunks[i].transform.Translate(Vector3.back * speed * Time.deltaTime);

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
        ChunkRemovedAction(_removedChunk);
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
