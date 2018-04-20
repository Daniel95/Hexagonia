using System;
using UnityEngine;

public class LevelProgess : MonoBehaviour {

	public static LevelProgess Instance { get { return GetInstance(); } }

	#region Singleton
	private static LevelProgess instance;

	private static LevelProgess GetInstance()
	{
		if (instance == null)
		{
			instance = FindObjectOfType<LevelProgess>();
		}
		return instance;
	}
#endregion 

	public static Action<int> ScoreUpdatedEvent;

	public float Timer { get { return Time.realtimeSinceStartup - startUpTime; }  }
	private float startUpTime;

	public void AddScore(int _score)
	{
		if(ScoreUpdatedEvent != null)
		{
			ScoreUpdatedEvent(_score);
		}
	}

	private void Awake()
	{
		startUpTime = Time.realtimeSinceStartup;
	}

	private void OnEnable()  
	{
		Coin.CollectedEvent += AddScore;
	}

	private void OnDisable()
	{
		Coin.CollectedEvent -= AddScore;
	}
}