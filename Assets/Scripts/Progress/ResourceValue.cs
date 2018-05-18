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

    public float Value { get { return resourceValue; } set { resourceValue = value; } }

	private const float MAXVALUE = 1f; 
	private const float MINVALUE = 0f; 

	private float resourceValue;

	[Tooltip("Wait for seconds(timeBetweenCoroutines), A higher number increases the wait time.")]
	[SerializeField] private float timeBetweenCoroutines = 1f;
	[SerializeField] private float resouceIncreaseOnPickup = 0.3f;
	[SerializeField] private float decreaseSpeed, increaseSpeed = 0.5f;

	private float targetValue;
	private Coroutine coroutineIncrease, coroutineDecrease;

	private void Awake()
	{
		resourceValue = 0;
		ResourceBarUI.Instance.UpdateResourceBar();
	}

	private void OnScoreUpdated(int _score)
	{
		float _offsetToPreviousTargetValue = targetValue - resourceValue;
		float _currentTargetValue = resourceValue + _offsetToPreviousTargetValue + resouceIncreaseOnPickup;

		StartIncreaseCoroutine(_currentTargetValue);
	}

	private void StartIncreaseCoroutine(float _increase)
    {
		if(coroutineIncrease != null)
		{
			StopCoroutine(coroutineIncrease);
		}
		if (coroutineDecrease != null)
		{
			StopCoroutine(coroutineDecrease);
		}

		coroutineIncrease = StartCoroutine(IncreaseOverTime(_increase, () => {
			StartDecreaseCoroutine();
		}));
	}

	private void StartDecreaseCoroutine()
	{
		if (coroutineDecrease != null)
		{
			StopCoroutine(coroutineDecrease);
		}

		coroutineDecrease = StartCoroutine(DecreaseToZeroOverTime());
	}

	private IEnumerator IncreaseOverTime(float _increase, Action onCompleted = null)
	{
		targetValue = resourceValue + _increase;

		while (resourceValue < targetValue)
		{
			resourceValue += increaseSpeed * Time.deltaTime;
			ResourceBarUI.Instance.UpdateResourceBar();
			yield return null;
		}

		resourceValue = targetValue;

		//yield return new WaitForSeconds(timeBetweenCoroutines);

		coroutineIncrease = null; 
		if(onCompleted != null)
		{
			onCompleted();
		}
	}

	private IEnumerator DecreaseToZeroOverTime(Action onCompleted = null)
	{
		while (resourceValue > MINVALUE)
		{
			resourceValue -= decreaseSpeed * Time.deltaTime;
			ResourceBarUI.Instance.UpdateResourceBar();
			yield return null;
		}

		coroutineDecrease = null;
		if (onCompleted != null)
		{
			onCompleted();
		}
	}

	private void StopResources()
	{
		if(coroutineDecrease != null)
		{
			StopCoroutine(coroutineDecrease);
		}
		if(coroutineIncrease != null)
		{
			StopCoroutine(coroutineIncrease);
		}
	}

	private void OnEnable()
    {
        LevelProgess.ScoreUpdatedEvent += OnScoreUpdated;
		Player.PlayerDiedEvent += StopResources;
    }

    private void OnDisable()
    {
        LevelProgess.ScoreUpdatedEvent -= OnScoreUpdated;
		Player.PlayerDiedEvent -= StopResources;
	}
}