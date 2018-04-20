using System;
using UnityEngine;

public class Coin : MonoBehaviour
{
	public static Action<int> CollectedEvent;
	public int amount { get { return amount; } set { amount; } }

	public static Coin Instance { get { return GetInstance(); } }

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


	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == Tags.Player)
		{
			if(CollectedEvent != null)
			{
				CollectedEvent(amount);
			}
		}
	}
}