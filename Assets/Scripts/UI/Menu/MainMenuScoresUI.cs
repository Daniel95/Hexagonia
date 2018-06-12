using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Displays the scores in the menu.
/// </summary>
public class MainMenuScoresUI : MonoBehaviour
{
    [SerializeField] private Text vrHighscoreText;
    [SerializeField] private Text nonVRHighscoreText;
    [SerializeField] private Text totalScoreText;

    private void Awake()
    {
        vrHighscoreText.text = Progression.VRHighScore.ToString();
        nonVRHighscoreText.text = Progression.NonVRHighScore.ToString();        
        totalScoreText.text = Progression.TotalScore.ToString();        
    }
}
