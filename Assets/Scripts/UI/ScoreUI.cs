﻿using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour {

	[SerializeField] private Text scoreText;

	private void UpdateScore(int _score)
	{
		Debug.Log(scoreText, gameObject);
		scoreText.text = "" + _score;
	}

	private void OnEnable()
	{
		LevelProgess.ScoreUpdatedEvent += UpdateScore;
	}

	private void OnDisable()
	{
		LevelProgess.ScoreUpdatedEvent -= UpdateScore;
	}
}