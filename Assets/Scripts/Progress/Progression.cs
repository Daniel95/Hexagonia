using System;
using UnityEngine;

/// <summary>
/// Contains the progression of the game.
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

    private void UpdateHighscores()
    {
        TotalScore += score;

        Debug.Log("TotalScore " + TotalScore);

        if (VRSwitch.VRState && score > VRHighScore)
        {
            VRHighScore = score;
            Debug.Log("VRHighScore " + VRHighScore);
        } 
        else if(!VRSwitch.VRState && score > NonVRHighScore)
        {
            NonVRHighScore = score;
            Debug.Log("NonVRHighScore " + NonVRHighScore);
        }

        if(HighscoresUpdatedEvent != null)
        {
            HighscoresUpdatedEvent();
        }
    }

	private void Awake()
	{
		startUpTime = Time.time;
	}

	private void OnEnable()  
	{
		Coin.CollectedEvent += IncreaseScore;
		Player.DiedEvent += UpdateHighscores;
	}

	private void OnDisable()
	{
		Coin.CollectedEvent -= IncreaseScore;
		Player.DiedEvent -= UpdateHighscores;
	}
}