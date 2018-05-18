﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Multiplier : MonoBehaviour {

	public static Multiplier Instance { get { return GetInstance(); } }

	#region Instance
	private static Multiplier instance;

	private static Multiplier GetInstance()
	{
		if (instance == null)
		{
			instance = FindObjectOfType<Multiplier>();
		}
		return instance;
	}
	#endregion

	public int MultiplierGet { get { return multiplier; } }

	private int multiplier = 1;
	[SerializeField] private float resetTime = 2f;
	[SerializeField] private Text multiplierText;
	private int counter;
	private Animator anim;
	[SerializeField] private List<Color> multiplierColors = new List<Color>(); 
	[SerializeField] private string animTriggerName = "MultiplierChanged";

	private Coroutine resetCoroutine;

	private void Start()
	{
		anim = GetComponent<Animator>();
	}

	public void Mutliplier()
	{
		ResetCoroutines();
		if(ResourceBarUI.Instance.resourceBar.fillAmount == 1)
		{
			anim.SetTrigger(animTriggerName);
			counter++;
			multiplier = counter + 1;

			UpdateMultiplierUI();

			ResourceBarUI.Instance.ChangeColor(multiplierColors[counter]);

			ResourceValue.Instance.Value = 0f;
		}
		
		ResourceBarUI.Instance.UpdateResourceBar();
		resetCoroutine = StartCoroutine(ResetMultiplier());

	}

	private IEnumerator ResetMultiplier()
	{
		yield return new WaitForSeconds(resetTime);
		multiplier = 1; 
		counter = 0; 
		multiplierText.text = "";
		ResourceBarUI.Instance.ChangeColor(multiplierColors[0]);

		resetCoroutine = null;
	}

	private void UpdateMultiplierUI()
	{
		multiplierText.text = "X" + multiplier;
	}

	private void ResetCoroutines()
	{
		if(resetCoroutine != null)
		{
			StopCoroutine(resetCoroutine);
		}
	}

	private void OnEnable()
	{
		Player.PlayerDiedEvent += ResetCoroutines;
	}

	private void OnDisable()
	{
		Player.PlayerDiedEvent -= ResetCoroutines;
	}
}