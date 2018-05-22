using System;
using UnityEngine;

public class LevelProgess : MonoBehaviour {

	public static LevelProgess Instance { get { return GetInstance(); } }

	public static Action<int> ScoreUpdatedEvent;

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

    public int Score { get { return score; } }
	public float Timer { get { return Time.time; }  }
    
	private int score;

    public void AddScore(int _scoreIncrement)
	{
        score += _scoreIncrement;

        if (ScoreUpdatedEvent != null)
		{
			ScoreUpdatedEvent(score);
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