using System;
using UnityEngine;

public class Coin : MonoBehaviour
{
	public static Action<int> CollectedEvent;

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

	[SerializeField] private int value;

    private void OnPlayerTriggerCollision(GameObject _gameObject) 
    {
        if(_gameObject != gameObject) { return; }

        if (CollectedEvent != null) {
            CollectedEvent(value);
        }
        Destroy(gameObject);
    }

    private void OnEnable() 
    {
        Player.TriggerCollisionEvent += OnPlayerTriggerCollision;
    }

    private void OnDisable() {
        Player.TriggerCollisionEvent -= OnPlayerTriggerCollision;
    }

}