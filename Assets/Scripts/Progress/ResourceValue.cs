using UnityEngine;

public class ResourceValue : MonoBehaviour
{

    public static ResourceValue Instance { get { return GetInstance(); } }

    #region Instance
    private static ResourceValue instance;

    private static ResourceValue GetInstance()
    {
        if(instance == null)
        {
            instance = FindObjectOfType<ResourceValue>();
        }
        return instance;
    }
    #endregion

    public float Value { get { return value; } }

    [SerializeField] private float resouceIncreaseOnPickup = 0.3f;

    private float value;

    private void OnScoreUpdatedEvent(int _score)
    {
        value += resouceIncreaseOnPickup;
    }

    private void OnEnable()
    {
        LevelProgess.ScoreUpdatedEvent += OnScoreUpdatedEvent;
    }

    private void OnDisable()
    {
        LevelProgess.ScoreUpdatedEvent -= OnScoreUpdatedEvent;
    }

}
