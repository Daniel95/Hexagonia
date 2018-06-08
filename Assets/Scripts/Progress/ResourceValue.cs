using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Contains and updates the resource value used in several elements in the game.
/// </summary>
public class ResourceValue : MonoBehaviour
{
	public static ResourceValue Instance { get { return GetInstance(); } }
	public static float Value { get { return resourceValue; } set { resourceValue = value; } }

    public static Action<float> UpdatedEvent;

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

	private static float resourceValue;

	public float Ratio { get { return resourceValue / maxValue; } }
    public int MaxValue { get { return maxValue; } set { maxValue = value; } }

    [Tooltip("Wait for seconds(timeBetweenCoroutines), A higher number increases the wait time.")]
	[SerializeField] private float timeBetweenCoroutines = 0.1f;
	[SerializeField] private float resouceIncreaseOnPickup = 0.4f;
	[SerializeField] private float increaseSpeed = 0.25f;
	[SerializeField] private float minDecreaseSpeed = 0.5f;
	[SerializeField] private float maxDecreaseSpeed = 1f;
	[SerializeField] private int maxValue = 5;

	private float targetValue;
	private Coroutine coroutineIncrease, coroutineDecrease;

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

            if (UpdatedEvent != null)
            {
                UpdatedEvent(resourceValue);
            }

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
			float _decreaseSpeed = minDecreaseSpeed + (_decreaseRange * Ratio);
			resourceValue -= _decreaseSpeed * Time.deltaTime;
			targetValue = resourceValue;

            if(UpdatedEvent != null)
            {
                UpdatedEvent(resourceValue);
            } 

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

    private void Awake()
    {
        resourceValue = 0;
    }

    private void OnEnable()
    {
        Progression.ScoreUpdatedEvent += OnScoreUpdated;
        PlayerCollisions.DiedEvent += StopResources;
    }

    private void OnDisable()
    {
        Progression.ScoreUpdatedEvent -= OnScoreUpdated;
        PlayerCollisions.DiedEvent -= StopResources;
	}
}