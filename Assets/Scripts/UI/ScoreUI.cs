using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour {

	[SerializeField] private Text scoreText;
	private int updateScore;

	private void UpdateScore(int _score)
	{
		updateScore += _score;
		scoreText.text = "Score: " + updateScore;
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