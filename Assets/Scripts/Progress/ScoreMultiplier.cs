using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreMultiplier : MonoBehaviour
{
	public static Action MultiplierIncreasedEvent;
	public static Action MultiplierDecreasedEvent;

	public static ScoreMultiplier Instance { get { return GetInstance(); } }

	#region Instance
	private static ScoreMultiplier instance;

	private static ScoreMultiplier GetInstance()
	{
		if (instance == null)
		{
			instance = FindObjectOfType<ScoreMultiplier>();
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
        ResourceBarUI.Instance.ChangeColor(multiplierColors[colorIndex]);
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