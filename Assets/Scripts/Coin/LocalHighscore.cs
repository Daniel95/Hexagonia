using UnityEngine;

public class LocalHighscore : MonoBehaviour {

	private const string LOCAL_HIGHSCORE = "Local_HighScore";

	public static int HighScore
	{
		get
		{
			return PlayerPrefs.GetInt(LOCAL_HIGHSCORE, 0);
		}
		set
		{
			PlayerPrefs.SetInt(LOCAL_HIGHSCORE, value);
			PlayerPrefs.Save();
		}
	}

	private void UpdateHighScore(int _score)
	{
		if (_score > HighScore)
		{
			HighScore = _score;
		}	
	}

	private void OnEnable()
	{
		Progression.ScoreUpdatedEvent += UpdateHighScore;
	}

	private void OnDisable()
	{
		Progression.ScoreUpdatedEvent -= UpdateHighScore;
	}
}
