using System;
using UnityEngine;

/// <summary>
/// Contains the progression of the game.
/// </summary>
public class Progression : MonoBehaviour
{
	public static float Timer { get { return Time.time - startUpTime; }  }
    public static Action<int> ScoreUpdatedEvent;

	private static float startUpTime;
	private static int score;

	private int lastScore;

	/// <summary>
	/// Increases the score by _scoreIncrement parameter.
	/// </summary>
	/// <param name="_scoreIncrement"></param>
	public void IncreaseScore(int _scoreIncrement)
	{
		score += _scoreIncrement * ScoreMultiplier.Multiplier;

        if (ScoreUpdatedEvent != null)
		{
			ScoreUpdatedEvent(score);
		}
	}

	private void ResetScore()
	{
		lastScore = score;
		score = 0;
	}

	private void Awake()
	{
		startUpTime = Time.time;
	}

	private void OnEnable()  
	{
		Coin.CollectedEvent += IncreaseScore;
		Player.DiedEvent += ResetScore;
	}

	private void OnDisable()
	{
		Coin.CollectedEvent -= IncreaseScore;
		Player.DiedEvent -= ResetScore;
	}
}