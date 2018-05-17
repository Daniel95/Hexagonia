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

    public float Value { get { return resourceValue; } set { resourceValue = value; } }

	private const float MAXVALUE = 1f; 
	private const float MINVALUE = 0f; 

	private float resourceValue;

	private float waitTime = 10f;

	[SerializeField] private float resouceIncreaseOnPickup = 0.3f;
	[SerializeField] private float decreaseSpeed, increaseSpeed = 0.5f;

	private Coroutine coroutineIncrease, coroutineDecrease;

	private void Awake()
	{
		resourceValue = 0;
		ResourceBarUI.Instance.UpdateResourceBar();
	}

	private void OnStartCoroutine(int _score)
    {
		coroutineIncrease = StartCoroutine(IncreaseOverTime());
	}

	private IEnumerator DecreaseOverTime()
	{
		while(resourceValue > MINVALUE)
		{
			float _decreaseByFrame = decreaseSpeed * Time.deltaTime;
			resourceValue -= _decreaseByFrame;
			ResourceBarUI.Instance.UpdateResourceBar();
			yield return null;
		}

		yield return new WaitForSeconds(waitTime);

		StopCoroutine(coroutineDecrease);
	}

	private IEnumerator IncreaseOverTime()
	{		
		float _newValue = Mathf.Clamp(resourceValue + resouceIncreaseOnPickup, MINVALUE, MAXVALUE);
		
		while (resourceValue < _newValue)
		{ 
			float _increaseByFrame = increaseSpeed * Time.deltaTime;
			resourceValue += _increaseByFrame;
			ResourceBarUI.Instance.UpdateResourceBar();
			yield return null;
		}
		Multiplier.Instance.Mutliplier();

		yield return new WaitForSeconds(waitTime);

		coroutineDecrease = StartCoroutine(DecreaseOverTime());
		StopCoroutine(coroutineIncrease);
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
        LevelProgess.ScoreUpdatedEvent += OnStartCoroutine;
		Player.PlayerDiedEvent += StopResources;
    }

    private void OnDisable()
    {
        LevelProgess.ScoreUpdatedEvent -= OnStartCoroutine;
		Player.PlayerDiedEvent -= StopResources;
	}
}