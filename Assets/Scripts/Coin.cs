using System;
using UnityEngine;

public class Coin : MonoBehaviour
{
	public Action<int> CollectedEvent;
	[SerializeField] private int amount;

	private void OnCollisionEnter(Collision other)
	{
		if(other.gameObject.tag == "Player")
		{
			Debug.Log("Collision with" + other);
			Destroy(other.gameObject);
		}
	}
}
