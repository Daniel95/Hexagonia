using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

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
	private int multiplier;
	private Coroutine resetAfterDelayCoroutine;

	private void Start()
	{
		anim = GetComponent<Animator>();
	}

	private void Update()
	{
		multiplier = Mathf.FloorToInt(ResourceValue.Instance.Value + 1);
		ResourceBarUI.Instance.ChangeColor(multiplierColors[0]);
		UpdateMultiplierUI();
	}

	private void UpdateMultiplierUI()
	{
		multiplierText.text = "X" + Multiplier;
	}

}