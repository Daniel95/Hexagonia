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

	private float waitTime = 1f;

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

		Debug.Log("OnStartCoroutine");
	}

	private IEnumerator DecreaseOverTime()
	{
		Debug.Log("DecreaseOverTime");
		float _decreaseByFrame = decreaseSpeed * Time.deltaTime;

		while (resourceValue > MINVALUE)
		{
			resourceValue -= _decreaseByFrame;
			ResourceBarUI.Instance.UpdateResourceBar();
			Debug.Log("DecreaseOverTime::WHILELOOP");
			yield return null;
		}

		yield return new WaitForSeconds(0);

		StopCoroutine(coroutineDecrease);
	}

	private IEnumerator IncreaseOverTime()
	{
		Debug.Log("IncreaseOverTime");
		float _newValue = Mathf.Clamp(resourceValue + resouceIncreaseOnPickup, MINVALUE, MAXVALUE);
		float _increaseByFrame = increaseSpeed * Time.deltaTime;

		while (resourceValue < _newValue)
		{ 
			resourceValue += _increaseByFrame;
			ResourceBarUI.Instance.UpdateResourceBar();
			Debug.Log("IncreaseOverTime::WHILELOOP");
			yield return null;
		}
		Multiplier.Instance.Mutliplier();
		Debug.Log("IncreaseOverTime::MultiplierActivated");

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