using System;
using UnityEngine;

public class Coin : MonoBehaviour
{
	public static Action<int> CollectedEvent;
	[SerializeField] private int amount;

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