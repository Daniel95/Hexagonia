using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls game over UI.
/// </summary>
public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private Text localHighscoreText;
    [SerializeField] private Text obtainedScoreText;

    private void UpdateScoreText()
    {
        int _modeHighscore = 0;

        if(VRSwitch.VRState)
        {
            _modeHighscore = Progression.VRHighScore;
        }
        else
        {
            _modeHighscore = Progression.NonVRHighScore;
        }

        localHighscoreText.text = "" + _modeHighscore;
        obtainedScoreText.text = "" + Progression.Instance.Score;
    }

    private void OnEnable()
    {
        PlayerDiedAnimation.CompletedEvent += Activate;
        Player.DiedEvent += RecenterUI;
        Progression.HighscoresUpdatedEvent += UpdateScoreText;
    }

    private void OnDisable()
    {
        PlayerDiedAnimation.CompletedEvent -= Activate;
        Player.DiedEvent -= RecenterUI;
        Progression.HighscoresUpdatedEvent -= UpdateScoreText;
    }

    private void Activate()
    {
        menu.SetActive(true);
    }

    private void RecenterUI()
    {
        menu.transform.position = new Vector3(Player.Instance.transform.position.x, Player.Instance.transform.position.y, menu.transform.position.z);
    }
}
