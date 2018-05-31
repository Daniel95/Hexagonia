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
        currentScore.text = ResourceValue.Value.ToString();
        PlayerPrefs.SetFloat("score", PlayerPrefs.GetFloat("score") + ResourceValue.Value);
        totalScore.text = PlayerPrefs.GetFloat("score").ToString();
    }
}
