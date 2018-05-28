using System;
using UnityEngine;

/// <summary>
/// Contains the progression of the game.
/// </summary>
public class Progression : MonoBehaviour {

	public static Progression Instance { get { return GetInstance(); } }

	public static Action<int> ScoreUpdatedEvent;

	#region Singleton
	private static Progression instance;

	private static Progression GetInstance()
	{
		if (instance == null)
		{
			instance = FindObjectOfType<Progression>();
		}
		return instance;
	}
#endregion 

    public int Score { get { return score; } }
	public float Timer { get { return Time.time - startUpTime; }  }

	private float startUpTime;
	private int score;

    /// <summary>
    /// Increases the score by _scoreIncrement parameter.
    /// </summary>
    /// <param name="_scoreIncrement"></param>
    public void IncreaseScore(int _scoreIncrement)
	{
		score += _scoreIncrement * ScoreMultiplierUI.Instance.Multiplier;

        if (ScoreUpdatedEvent != null)
		{
			ScoreUpdatedEvent(score);
		}
	}

	private void Awake()
	{
		startUpTime = Time.time;
	}

	private void OnEnable()  
	{
		Coin.CollectedEvent += IncreaseScore;
	}

	private void OnDisable()
	{
		Coin.CollectedEvent -= IncreaseScore;
	}
}