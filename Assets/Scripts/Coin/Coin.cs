using System;
using UnityEngine;

public class Coin : MonoBehaviour
{
	public static Coin Instance { get { return GetInstance(); }  }

	#region Singleton
	private static Coin instance;

	private static Coin GetInstance()
	{
		if (instance == null)
		{
			instance = FindObjectOfType<Coin>();
		}
		return instance;
	}
	#endregion

	public static Action<int> CollectedEvent;
	public static Action CoinCollectedEvent;
	public int value;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == Tags.Player)
		{
			Destroy(this.gameObject);
			if (CollectedEvent != null)
			{
				CollectedEvent(value);
			}
			if (CoinCollectedEvent != null)
			{
				CoinCollectedEvent();
			}
		}
	}
}