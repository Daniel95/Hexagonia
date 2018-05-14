using UnityEngine;
using System.Collections;
using System;

public class ResourceValue : MonoBehaviour
{
    public static ResourceValue Instance { get { return GetInstance(); } }

    #region Instance
    private static ResourceValue instance;

    private static ResourceValue GetInstance()
    {
        if(instance == null)
        {
            instance = FindObjectOfType<ResourceValue>();
        }
        return instance;
    }
    #endregion

    public float Value { get { return value; } }

    [SerializeField] private float resouceIncreaseOnPickup = 0.3f;

    private float value;

	public float maxValue = 1f; //Maximum Resource Value 
	private float minValue = 0f; //Minumum Resource Value
	private float waitTime = 0.5f;

	private Coroutine coroutineIncrease, coroutineDecrease;


	private void Awake()
	{
		value = 0;
	}

	private void OnScoreUpdatedEvent(int _score)
    {
		coroutineIncrease = StartCoroutine(IncreaseOverTime());
	}

	private IEnumerator DecreaseOverTime()
	{
		value -= 0.1f;

		ResourceBarUI.Instance.UpdateResourceBar();

		yield return new WaitForSeconds(waitTime);

		StopCoroutine(coroutineDecrease);
	}

	private IEnumerator IncreaseOverTime()
	{
		if (value > maxValue)
		{
			value = 1f;
		}
		else
		{
			value += resouceIncreaseOnPickup; //0 = 0 + 0.3 = 0.3  || 0.3 = 0.3 + 0.3 = 0.6 || 0.9 = 0.9 + 0.3 = 1.2
			ResourceBarUI.Instance.UpdateResourceBar();

		}

		yield return new WaitForSeconds(waitTime);

		coroutineDecrease = StartCoroutine(DecreaseOverTime());
		StopCoroutine(coroutineIncrease);
	}


	private void OnEnable()
    {
        LevelProgess.ScoreUpdatedEvent += OnScoreUpdatedEvent;
    }

    private void OnDisable()
    {
        LevelProgess.ScoreUpdatedEvent -= OnScoreUpdatedEvent;
    }

}
