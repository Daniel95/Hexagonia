using System;
using UnityEngine;

/// <summary>
/// Contains the progression of the game.
/// </summary>
public class Progression : MonoBehaviour {

	public static Action<int> ScoreUpdatedEvent;

    public static int Score { get { return score; } }
	public static float Timer { get { return Time.time - startUpTime; }  }

	private static float startUpTime;
	private static int score;

    /// <summary>
    /// Increases the score by _scoreIncrement parameter.
    /// </summary>
    /// <param name="_scoreIncrement"></param>
    public void IncreaseScore(int _scoreIncrement)
	{
		score += _scoreIncrement * ScoreMultiplierUI.Multiplier;

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