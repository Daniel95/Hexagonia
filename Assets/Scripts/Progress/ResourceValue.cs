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

	public float maxValue = 1f; //Maximum Resource Value 
	public float minValue = 0f; //Minumum Resource Value

	private float value;
	private float waitTime = 0.5f;

	[Space(10)]
	[SerializeField] private float resouceIncreaseOnPickup = 0.3f;

	[Space(10)]
	[SerializeField] private float decreaseAmount = 0.1f;
	[SerializeField] private float decreaseTime = 4f;

	[Space(10)]
	[SerializeField] private float increaseAmount = 0.1f;
	[SerializeField] private float increaseTime = 0.5f;

	private Coroutine coroutineIncrease, coroutineDecrease;


	private void Awake()
	{
		value = 0;
		ResourceBarUI.Instance.UpdateResourceBar();
	}

	private void OnScoreUpdatedEvent(int _score)
    {
		coroutineIncrease = StartCoroutine(IncreaseOverTime(increaseAmount, increaseTime));
	}

	private IEnumerator DecreaseOverTime(float __decrementAmount, float decreaseTime)
	{
		float _decreaseByTime = __decrementAmount / decreaseTime;


		while(value > minValue)
		{
			float _decreaseByFrame = _decreaseByTime * Time.deltaTime;
			value -= _decreaseByFrame;
			ResourceBarUI.Instance.UpdateResourceBar();
			yield return null;
		}

		yield return new WaitForSeconds(waitTime);

		StopCoroutine(coroutineDecrease);
	}

	private IEnumerator IncreaseOverTime(float _increaseAmount, float _increaseTime)
	{
		float _increaseByTime = _increaseAmount / _increaseTime;
		
		float _newValue = value + resouceIncreaseOnPickup;
		
		while (value < _newValue)
		{ 
			if (value > maxValue)
			{
				value = maxValue;
				ResourceBarUI.Instance.UpdateResourceBar();
				Debug.Log("MaxValue has been reached: " + value);
			}
			else if(value < maxValue)
			{
				float _increaseByFrame = _increaseByTime * Time.deltaTime;
				value += _increaseByFrame;
				ResourceBarUI.Instance.UpdateResourceBar();
			}

			yield return null;
		}

		yield return new WaitForSeconds(waitTime);

		coroutineDecrease = StartCoroutine(DecreaseOverTime(decreaseAmount, decreaseTime));
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