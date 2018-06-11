using System;
using UnityEngine;

/// <summary>
/// Contains the information about progression of level.
/// </summary>
public class Progression : MonoBehaviour
{
    public static int VRHighScore
    {
        get { return PlayerPrefs.GetInt(VR_LOCAL_HIGHSCORE, 0); }
        set
        {
            PlayerPrefs.SetInt(VR_LOCAL_HIGHSCORE, value);
            PlayerPrefs.Save();
        }
    }

    public static int NonVRHighScore
    {
        get { return PlayerPrefs.GetInt(NON_VR_LOCAL_HIGHSCORE, 0); }
        set
        {
            PlayerPrefs.SetInt(NON_VR_LOCAL_HIGHSCORE, value);
            PlayerPrefs.Save();
        }
    }

    public static int TotalScore
    {
        get { return PlayerPrefs.GetInt(TOTAL_SCORE, 0); }
        set
        {
            PlayerPrefs.SetInt(TOTAL_SCORE, value);
            PlayerPrefs.Save();
        }
    }

    public static float Timer { get { return Time.time - startUpTime; }  }
	public float Score { get { return score; }  }
    public static Progression Instance { get { return GetInstance(); } }

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

    public static Action HighscoresUpdatedEvent;
    public static Action<int> ScoreUpdatedEvent;

    private static float startUpTime;

    private const string VR_LOCAL_HIGHSCORE = "VR_Local_HighScore";
    private const string NON_VR_LOCAL_HIGHSCORE = "Non_VR_Local_HighScore";
    private const string TOTAL_SCORE = "Total_Score";

    private int score;
    private int lastHighscoreScore;
    private bool reachedHighscore;

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

        if (!reachedHighscore && lastHighscoreScore > 0 && score > lastHighscoreScore)
        {
            reachedHighscore = true;
            AudioEffectManager.Instance.PlayEffect(AudioEffectType.Highscore);
        }
	}

    private void SaveHighscores()
    {
        TotalScore += score;

        if (VRSwitch.VRState && score > VRHighScore)
        {
            VRHighScore = score;
        } 
        else if(!VRSwitch.VRState && score > NonVRHighScore)
        {
            NonVRHighScore = score;
        }

        if(HighscoresUpdatedEvent != null)
        {
            HighscoresUpdatedEvent();
        }
    }

	private void Awake()
	{
		startUpTime = Time.time;

        if (VRSwitch.VRState)
        {
            lastHighscoreScore = VRHighScore;
        }
        else if (!VRSwitch.VRState)
        {
            lastHighscoreScore = NonVRHighScore;
        }
    }

	private void OnEnable()  
	{
		Coin.CollectedEvent += IncreaseScore;
        PlayerCollisions.DiedEvent += SaveHighscores;
	}

	private void OnDisable()
	{
		Coin.CollectedEvent -= IncreaseScore;
        PlayerCollisions.DiedEvent -= SaveHighscores;
	}
}