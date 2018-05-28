using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Displays the score multiplier UI, and calculates the score multiplier value.
/// </summary>
public class ScoreMultiplierUI : MonoBehaviour
{
	public static Action MultiplierIncreasedEvent;
	public static Action MultiplierDecreasedEvent;

	public static ScoreMultiplierUI Instance { get { return GetInstance(); } }

	#region Instance
	private static ScoreMultiplierUI instance;

	private static ScoreMultiplierUI GetInstance()
	{
		if (instance == null)
		{
			instance = FindObjectOfType<ScoreMultiplierUI>();
		}
		return instance;
	}
	#endregion

	public int Multiplier { get { return multiplier; } }

	[SerializeField] private Text multiplierText;
	[SerializeField] private List<Color> multiplierColors = new List<Color>(); 
	[SerializeField] private string animTriggerName = "MultiplierChanged";

	private Animator anim;
	private int multiplier = 1;
	private int previousMultiplier = 1;

	private void Awake()
	{
		anim = GetComponent<Animator>();
        if(multiplierColors.Count != ResourceValue.Instance.MaxValue) {
            Debug.LogError("multiplierColors should have " + ResourceValue.Instance.MaxValue + " colors!", gameObject);
        }
	}

	private void Update()
	{
		multiplier = Mathf.Clamp(Mathf.FloorToInt(ResourceValue.Instance.Value + 1), 1, ResourceValue.Instance.MaxValue);
        int colorIndex = multiplier - 1;
        ResourceBarUI.Instance.UpdateColor(multiplierColors[colorIndex]);
		UpdateMultiplierUI();

        if(multiplier > previousMultiplier) {
            anim.SetTrigger(animTriggerName);
        }
        previousMultiplier = multiplier;
    }

	private void UpdateMultiplierUI()
	{
		multiplierText.text = "X" + Multiplier;
	}
}