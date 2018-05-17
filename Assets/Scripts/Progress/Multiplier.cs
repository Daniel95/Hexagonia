using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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

	public int MultiplierGet { get { return multiplier; } }

	[SerializeField] private int multiplier = 1;
	[SerializeField] private float resetTime = 2f;
	[SerializeField] private Text multiplierText;
	private int counter;

	private Coroutine coroutine;

	public void Mutliplier()
	{
		if(coroutine != null)
		{
			StopCoroutine(coroutine);
		}

		if (ResourceBarUI.Instance.resourceBar.fillAmount == 1 && counter == 0)
		{
			multiplier = 2;
			counter = 1;

			UpdateMultiplierUI();

			ResourceBarUI.Instance.ChangeColor(Color.green);

			ResourceValue.Instance.Value = 0f;

		}
		else if(ResourceBarUI.Instance.resourceBar.fillAmount == 1 && counter == 1)
		{
			multiplier = 3;
			counter = 2;

			UpdateMultiplierUI();

			ResourceBarUI.Instance.ChangeColor(Color.yellow);

			ResourceValue.Instance.Value = 0f;
		}
		else if (ResourceBarUI.Instance.resourceBar.fillAmount == 1 && counter == 2)
		{
			multiplier = 4;
			counter = 3;

			UpdateMultiplierUI();

			ResourceBarUI.Instance.ChangeColor(Color.blue);

			ResourceValue.Instance.Value = 0f;
		}
		ResourceBarUI.Instance.UpdateResourceBar();
		coroutine = StartCoroutine(ResetMultiplier());

	}

	private IEnumerator ResetMultiplier()
	{
		yield return new WaitForSeconds(resetTime);
		multiplier = 1; //Reset
		counter = 0; //Reset
		multiplierText.text = "";
		ResourceBarUI.Instance.ChangeColor(Color.white);

		coroutine = null;
	}

	private void UpdateMultiplierUI()
	{
		multiplierText.text = "X" + multiplier;
	}
}