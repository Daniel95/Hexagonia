using UnityEngine;
using System;

public class PlayerCollision : MonoBehaviour
{
    public static Action PlayerDiedEvent;

    [SerializeField] private GameObject gameOverMenu;


    public static Action<GameObject> ChunkRemovedEvent;

    public static PlayerCollision Instance
    {
        get
        {
            return GetInstance();
        }
    }
    
    private void OnTriggerEnter(Collider _otherCollider)
    {
        if (_otherCollider.tag == Tags.Obstacle)
        {
            LookPositionOnPlane.Instance.enabled = false;
            if (PlayerDiedEvent != null)
                PlayerDiedEvent();

            Destroy(gameObject);
        }
    }

    #region Singleton
    private static PlayerCollision instance;

    private static PlayerCollision GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<PlayerCollision>();
        }
        return instance;
    }
    #endregion
}