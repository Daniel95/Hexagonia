using System;
using UnityEngine;

/// <summary>
/// The coin checks collision and sends event and a value if it has been hit.
/// </summary>
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

	private const string COIN_PARTICLE = "CoinParticle";

	[SerializeField] private int value;
	private GameObject particle;

	private void OnPlayerTriggerCollision(GameObject _gameObject) 
    {
        if(_gameObject != gameObject) { return; }

        if (CollectedEvent != null) {
            CollectedEvent(value);
        }
		particle = ObjectPool.Instance.GetObjectForType(COIN_PARTICLE, false);
		particle.transform.position = transform.position;
        
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