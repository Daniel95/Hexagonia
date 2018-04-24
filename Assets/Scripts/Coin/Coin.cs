using System;
using UnityEngine;

public class Coin : MonoBehaviour
{
	public static Action<int> CollectedEvent;
	[SerializeField] private int value;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == Tags.Player)
		{
			Destroy(this.gameObject);
			if (CollectedEvent != null)
			{
				CollectedEvent(value);
			}
		}
	}
}