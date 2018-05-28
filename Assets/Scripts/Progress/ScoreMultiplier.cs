using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Displays the score multiplier UI, and calculates the score multiplier value.
/// </summary>
public class ScoreMultiplierUI : MonoBehaviour
{
    public static Action<int> ScoreMultiplierUpdatedEvent;

	public static Action MultiplierIncreasedEvent;
	public static Action MultiplierDecreasedEvent;

	public static int Multiplier { get { return multiplier; } }

    private static int multiplier = 1;

    [SerializeField] private List<Color> multiplierColors = new List<Color>();
    [SerializeField] private Text multiplierText;
	[SerializeField] private string animTriggerName = "MultiplierChanged";

	private Animator multiplierTextAnimation;
	private int previousMultiplier = 1;

	private void Awake()
	{
		multiplierTextAnimation = GetComponent<Animator>();
	}

    private void UpdateScoreMultiplier(float _value)
    {
        multiplier = Mathf.Clamp(Mathf.FloorToInt(ResourceValue.Value + 1), 1, ResourceValue.Instance.MaxValue);
        if(multiplier == previousMultiplier) { return; }

        int colorIndex = multiplier - 1;
        UpdateMultiplierUI();

        if (multiplier > previousMultiplier)
        {
            multiplierTextAnimation.SetTrigger(animTriggerName);
        }
        previousMultiplier = multiplier;

        if(ScoreMultiplierUpdatedEvent != null)
        {
            ScoreMultiplierUpdatedEvent(multiplier);
        }
    }

	private void UpdateMultiplierUI()
	{
		multiplierText.text = "X" + Multiplier;
	}

    private void OnEnable()
    {
        ResourceValue.ResourceValueUpdatedEvent += UpdateScoreMultiplier;    
    }

    private void OnDisable()
    {
        ResourceValue.ResourceValueUpdatedEvent -= UpdateScoreMultiplier;
    }
}