using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Updates the Score UI.
/// </summary>
public class ScoreUI : MonoBehaviour
{
	[SerializeField] private Text scoreText;

	private void UpdateScore(int _score)
	{
		scoreText.text = "" + _score;
	}

	private void OnEnable()
	{
		Progression.ScoreUpdatedEvent += UpdateScore;
	}

	private void OnDisable()
	{
		Progression.ScoreUpdatedEvent -= UpdateScore;
	}
}