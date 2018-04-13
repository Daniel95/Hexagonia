using System;
using UnityEngine;

public class Coin : MonoBehaviour
{
	public static Action<int> CollectedEvent;
	[SerializeField] private int amount;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			Debug.Log("Collision with" + other.gameObject.name);
			if(CollectedEvent != null)
			{
				CollectedEvent(amount);
			}
		}
	}
}