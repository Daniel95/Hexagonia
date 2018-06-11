using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Displays the score multiplier UI, and calculates the score multiplier value.
/// </summary>
public class ScoreMultiplierUI : FlickerableText
{
	[SerializeField] private string animTriggerName = "MultiplierChanged";

	private Animator multiplierTextAnimation;

	private void UpdateMultiplierUI(int _multiplier)
	{
        multiplierTextAnimation.SetTrigger(animTriggerName);
        Text.text = "X" + _multiplier;

        StartFlicker();
	}

    protected override void Awake()
    {
        base.Awake();
		multiplierTextAnimation = Text.GetComponent<Animator>();
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