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

	public float Timer; //Unused Variable

	public void AddScore(int _score)
	{
		Debug.Log("LevelProgress::AddScore " + _score);
		if(ScoreUpdatedEvent != null)
		{
			ScoreUpdatedEvent(_score);
		}
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