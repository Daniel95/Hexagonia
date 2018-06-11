using UnityEngine.UI;

/// <summary>
/// Updates the Score UI.
/// </summary>
public class ScoreUI : FlickerableText
{
    private void UpdateScore(int _score)
	{
        Text.text = "" + _score;
        StartFlicker();
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