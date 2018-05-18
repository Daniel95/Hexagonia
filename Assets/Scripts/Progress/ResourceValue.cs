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

	[Tooltip("Wait for seconds(timeBetweenCoroutines), A higher number increases the wait time.")]
	[SerializeField] private float timeBetweenCoroutines = 1f;

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
		Debug.Log("OnStartCoroutine");
		coroutineIncrease = StartCoroutine(IncreaseOverTime());
	}

	private IEnumerator DecreaseOverTime()
	{
		Debug.Log("DecreaseOverTime");
		float _decreaseByFrame = decreaseSpeed * Time.deltaTime;

		while (resourceValue > MINVALUE)
		{
			Debug.Log("DecreaseOverTime::WHILELOOP");
			resourceValue -= _decreaseByFrame;
			Debug.Log("DecreaseByFrame: " + _decreaseByFrame);
			ResourceBarUI.Instance.UpdateResourceBar();
			yield return null;
		}

		coroutineDecrease = null;
	}

	private IEnumerator IncreaseOverTime()
	{
		if (coroutineDecrease != null)
		{
			StopCoroutine(coroutineDecrease);
		}

		Debug.Log("IncreaseOverTime");
		float _newValue = Mathf.Clamp(resourceValue + resouceIncreaseOnPickup, MINVALUE, MAXVALUE);
		float _increaseByFrame = increaseSpeed * Time.deltaTime;

		while (resourceValue < _newValue)
		{
			Debug.Log("IncreaseOverTime::WHILELOOP");
			resourceValue += _increaseByFrame;
			ResourceBarUI.Instance.UpdateResourceBar();
			yield return null;
		}
		Debug.Log("IncreaseOverTime::MultiplierActivated");
		Multiplier.Instance.Mutliplier();

		yield return new WaitForSeconds(timeBetweenCoroutines);

		coroutineDecrease = StartCoroutine(DecreaseOverTime());
		coroutineIncrease = null; 
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