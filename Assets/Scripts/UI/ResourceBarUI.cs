using UnityEngine;
using System.Collections;
using System;

public class ResourceBarUI : MonoBehaviour
{
	private float maxValue = 1f; //Maximum Resource Value 
	private float minValue = 0f; //Minumum Resource Value
	private float currentValue; //Current Resource Value

	private Coroutine coroutineIncrease, coroutineDecrease;


	private float waitTime = 0.5f;
	[SerializeField] private GameObject resourceBar;

	private void Awake()
	{
		resourceBar.transform.localScale = new Vector3(minValue, resourceBar.transform.localScale.y, resourceBar.transform.localScale.z);
	}

	private void UpdateResourceBar()
	{
		//-----------------------------
		coroutineIncrease = StartCoroutine(IncreaseOverTime());
		//-----------------------------

		/*
		currentValue = _score;

		if (currentValue > minValue)
		{
			currentValue = Mathf.Clamp(currentValue - (0.1f * Time.deltaTime), minValue, maxValue);
			//StartCoroutine("DecreaseOverTime");
		}
		if (currentValue <= minValue)
		{
			currentValue = 0;
			Debug.Log("resourcebar is " + currentValue + " empty?");
		}
		resourceBar.transform.localScale = new Vector3(currentValue, resourceBar.transform.localScale.y, resourceBar.transform.localScale.z);
		Debug.Log("Currentvalue is: " + currentValue);
		*/
	}

	private IEnumerator DecreaseOverTime()
	{
		currentValue = currentValue - 0.1f;
		Debug.Log("DecreaseOverTime CurrentValue: " + currentValue);
		resourceBar.transform.localScale = new Vector3(Mathf.Clamp(currentValue - (0.01f * Time.deltaTime), minValue, maxValue), resourceBar.transform.localScale.y , resourceBar.transform.localScale.z);
		//resourceBar.transform.localScale = new Vector3(currentValue, resourceBar.transform.localScale.y, resourceBar.transform.localScale.z);

		yield return new WaitForSeconds(waitTime);

		StopCoroutine(coroutineDecrease);
	}

	private IEnumerator IncreaseOverTime()
	{
		if(currentValue >= maxValue)
		{
			Debug.Log("BAR IS FULL");
			currentValue = 1f;
		}
		else
		{
			currentValue = ResourceValue.Instance.Value;
		}
		Debug.Log("increaseOverTime CurrentValue: " + currentValue);

		resourceBar.transform.localScale = new Vector3(Mathf.Clamp(currentValue, minValue, maxValue), resourceBar.transform.localScale.y, resourceBar.transform.localScale.z);

		//resourceBar.transform.localScale = new Vector3(currentValue, resourceBar.transform.localScale.y, resourceBar.transform.localScale.z);

		yield return new WaitForSeconds(waitTime);

		coroutineDecrease = StartCoroutine(DecreaseOverTime());
		StopCoroutine(coroutineIncrease);
	}

	private void OnEnable()
	{
		Coin.CoinCollectedEvent += UpdateResourceBar;
	}

	private void OnDisable()
	{
		Coin.CoinCollectedEvent -= UpdateResourceBar;
	}
}