using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls game over UI.
/// </summary>
public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private Text currentScore;
    [SerializeField] private Text totalScore;

    private const string PLAYERPREFS_SCORE = "TotalScore";

    private void OnEnable()
    {
        DyingPlayer.AnimationEnd += Activate;
    }

    private void OnDisable()
    {
        DyingPlayer.AnimationEnd -= Activate;
    }

    private void Activate()
    {
        menu.SetActive(true);
        currentScore.text = Progression.Score.ToString();
        PlayerPrefs.SetInt(PLAYERPREFS_SCORE, PlayerPrefs.GetInt(PLAYERPREFS_SCORE) + Progression.Score);
        totalScore.text = PlayerPrefs.GetInt(PLAYERPREFS_SCORE).ToString();
    }
}
