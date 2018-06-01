using UnityEngine.UI;
using UnityEngine;

public class LocalHighscore : MonoBehaviour {

	public static LocalHighscore Instance { get { return GetInstance(); } }

#region Singleton
	private static LocalHighscore instance;

	private static LocalHighscore GetInstance()
	{
		if(instance != null)
		{
			instance = FindObjectOfType<LocalHighscore>();
		}

		return instance;
	}
	#endregion

	private const string LOCAL_HIGHSCORE = "Local_HighScore";

	[SerializeField] private Text localHighscoreText;
	[SerializeField] private Text obtainedScoreText;
	private int score;

	public static int UpdateLocalHighscore
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

	private void LoadPlayerPrefs()
	{
		localHighscoreText.text = "" + UpdateLocalHighscore;
		obtainedScoreText.text = "" + score;
	}

	private void UpdatePlayerPrefs(int _score)
	{
		score = _score;
		if (_score > UpdateLocalHighscore)
		{
			UpdateLocalHighscore = _score;
		}	
	}

	private void OnEnable()
	{
		Progression.ScoreUpdatedEvent += UpdatePlayerPrefs;
		Player.DiedEvent += LoadPlayerPrefs;
	}

	private void OnDisable()
	{
		Progression.ScoreUpdatedEvent -= UpdatePlayerPrefs;
		Player.DiedEvent -= LoadPlayerPrefs;
	}
}
