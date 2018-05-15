using UnityEngine;

public class Multiplier : MonoBehaviour {

	public static Multiplier Instance { get { return GetInstance(); } }

	#region Instance
	private static Multiplier instance;

	private static Multiplier GetInstance()
	{
		if (instance == null)
		{
			instance = FindObjectOfType<Multiplier>();
		}
		return instance;
	}
	#endregion

	private float timeProgress = 2f;
	private float timeSinceLastCoin;
	[SerializeField] private float multiplier = 1;

	public void Mutliplier()
	{
		if(ResourceBarUI.Instance.resourceBar.fillAmount == 1)
		{
			multiplier++;
			Debug.Log("MUTLIPLIER");
			Debug.Log("Multiplier: " + multiplier);
			ResourceBarUI.Instance.resourceBar.fillAmount = 0;
			Debug.Log("FillAmount: " + ResourceBarUI.Instance.resourceBar.fillAmount);

		}

		if (ResourceBarUI.Instance.resourceBar.fillAmount == 0)
		{
			multiplier = 1f;
		}
		ResourceValue.Instance.Value *= multiplier;
	}
}
