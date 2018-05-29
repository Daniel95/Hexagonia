using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Displays the score multiplier UI, and calculates the score multiplier value.
/// </summary>
public class ScoreMultiplier : MonoBehaviour
{
    public static ScoreMultiplier Instance { get { return GetInstance(); } }
    public static int Multiplier { get { return multiplier; } }
    public static Action<int> UpdatedEvent;
    public static Action MultiplierIncreasedEvent;
    public static Action MultiplierDecreasedEvent;

    #region Singleton
    private static ScoreMultiplier instance;

    private static ScoreMultiplier GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<ScoreMultiplier>();
        }
        return instance;
    }
    #endregion

    private static int multiplier = 1;

    public Color MultiplierColor { get { return multiplierColors[multiplier - 1]; } }

    [SerializeField] private List<Color> multiplierColors = new List<Color>();

    private int previousMultiplier = 1;

    private void UpdateScoreMultiplier(float _value)
    {
        multiplier = Mathf.Clamp(Mathf.FloorToInt(ResourceValue.Value + 1), 1, ResourceValue.Instance.MaxValue);
        if (multiplier == previousMultiplier) { return; }
        previousMultiplier = multiplier;

        if (UpdatedEvent != null)
        {
            UpdatedEvent(multiplier);
        }
    }

    private void OnEnable()
    {
        ResourceValue.UpdatedEvent += UpdateScoreMultiplier;
    }

    private void OnDisable()
    {
        ResourceValue.UpdatedEvent -= UpdateScoreMultiplier;
    }
}