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
    public int MaxValue { get { return maxValue; } set { maxValue = value; } }
    public float ResourceRatio { get { return resourceValue / maxValue; } }

    [Tooltip("Wait for seconds(timeBetweenCoroutines), A higher number increases the wait time.")]
	[SerializeField] private float timeBetweenCoroutines = 1f;
	[SerializeField] private float resouceIncreaseOnPickup = 0.3f;
	[SerializeField] private float increaseSpeed = 0.5f;
	[SerializeField] private float minDecreaseSpeed = 0.5f;
	[SerializeField] private float maxDecreaseSpeed = 1f;
	[SerializeField] private int maxValue = 5;

    private float resourceValue;
	private float targetValue;
	private Coroutine coroutineIncrease, coroutineDecrease;

	private void Awake()
	{
		resourceValue = 0;
        ResourceBarUI.Instance.UpdateBarValue();
	}

	private void OnScoreUpdated(int _score)
	{
		float _newValue = targetValue + resouceIncreaseOnPickup;
		targetValue = Mathf.Clamp(_newValue, 0, maxValue);

        StartIncreaseCoroutine(targetValue);
	}

	private void StartIncreaseCoroutine(float _targetValue)
    {
		if(coroutineIncrease != null)
		{
			StopCoroutine(coroutineIncrease);
		}
		if (coroutineDecrease != null)
		{
			StopCoroutine(coroutineDecrease);
		}

		coroutineIncrease = StartCoroutine(IncreaseToTargetValueOverTime(_targetValue, () => {
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

	private IEnumerator IncreaseToTargetValueOverTime(float _targetValue, Action onCompleted = null)
	{
		while (resourceValue < _targetValue)
		{
			resourceValue += increaseSpeed * Time.deltaTime;
			ResourceBarUI.Instance.UpdateBarValue();
			yield return null;
		}

		resourceValue = _targetValue;
		yield return new WaitForSeconds(timeBetweenCoroutines);

		coroutineIncrease = null; 
		if(onCompleted != null)
		{
			onCompleted();
		}
	}

	private IEnumerator DecreaseToZeroOverTime(Action onCompleted = null)
	{
		float _decreaseRange = maxDecreaseSpeed - minDecreaseSpeed;

		while (resourceValue > 0)
		{
			float _decreaseSpeed = minDecreaseSpeed + (_decreaseRange * ResourceRatio);
			resourceValue -= _decreaseSpeed * Time.deltaTime;
			targetValue = resourceValue;

			ResourceBarUI.Instance.UpdateBarValue();
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
		Player.DiedEvent += StopResources;
    }

    private void OnDisable()
    {
        LevelProgess.ScoreUpdatedEvent -= OnScoreUpdated;
		Player.DiedEvent -= StopResources;
	}
}