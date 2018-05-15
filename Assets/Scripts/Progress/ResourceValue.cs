using UnityEngine;
using System.Collections;

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

	public float maxValue = 1f; 
	public float minValue = 0f; 

	private float value;
	private float waitTime = 0.5f;

	[SerializeField] private float resouceIncreaseOnPickup = 0.3f;
	[SerializeField] private float adjustBySpeed = 0.1f;

	private Coroutine coroutineIncrease, coroutineDecrease;

	private void Awake()
	{
		value = 0;
		ResourceBarUI.Instance.UpdateResourceBar();
	}

	private void OnScoreUpdatedEvent(int _score)
    {
		coroutineIncrease = StartCoroutine(IncreaseOverTime());
	}

	private IEnumerator DecreaseOverTime()
	{
		while(value > minValue)
		{
			float _decreaseByFrame = adjustBySpeed * Time.deltaTime;
			value -= _decreaseByFrame;
			ResourceBarUI.Instance.UpdateResourceBar();
			yield return null;
		}

		yield return new WaitForSeconds(waitTime);

		StopCoroutine(coroutineDecrease);
	}

	private IEnumerator IncreaseOverTime()
	{		
		float _newValue = Mathf.Clamp(value + resouceIncreaseOnPickup, minValue, maxValue);
		
		while (value < _newValue)
		{ 
			float _increaseByFrame = adjustBySpeed * Time.deltaTime;
			value += _increaseByFrame;
			ResourceBarUI.Instance.UpdateResourceBar();
			
			yield return null;
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