using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Displays the score multiplier UI, and calculates the score multiplier value.
/// </summary>
public class ScoreMultiplierUI : MonoBehaviour
{
    [SerializeField] private Text multiplierText;
	[SerializeField] private string animTriggerName = "MultiplierChanged";

	private Animator multiplierTextAnimation;

	private void UpdateMultiplierUI(int _multiplier)
	{
        multiplierTextAnimation.SetTrigger(animTriggerName);
        multiplierText.text = "X" + _multiplier;
	}

	private void Awake()
	{
		multiplierTextAnimation = multiplierText.GetComponent<Animator>();
	}

    private void OnEnable()
    {
        ScoreMultiplier.UpdatedEvent += UpdateMultiplierUI;    
    }

    private void OnDisable()
    {
        ScoreMultiplier.UpdatedEvent -= UpdateMultiplierUI;
    }
}