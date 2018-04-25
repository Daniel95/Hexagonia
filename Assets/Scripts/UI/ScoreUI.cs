using UnityEngine;

public class ScoreUI : MonoBehaviour {

	[SerializeField] private TextMesh scoreText;

	private void UpdateScore(int _score)
	{
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