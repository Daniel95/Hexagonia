using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Updates the resourcebar fill amount and color.
/// </summary>
public class ResourceBarUI : MonoBehaviour
{
	[SerializeField] private Image resourceBar;
    [SerializeField] private List<Color> multiplierColors = new List<Color>();

    /// <summary>
    /// Updates the value of resource bar.
    /// </summary>
    private void UpdateBarValue(float _value)
	{
        float _barValue = 1;

        if(_value < ResourceValue.Instance.MaxValue)
        {
            _barValue = _value % 1;
        }

        resourceBar.fillAmount = _barValue;
    }

    /// <summary>
    /// Update the color of the resource bar.
    /// </summary>
    public void OnScoreMultiplierUpdated(int _multiplier)
	{
		resourceBar.color = multiplierColors[_multiplier];
	}

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private void Awake()
    {
        if (multiplierColors.Count != ResourceValue.Instance.MaxValue)
        {
            Debug.LogError("multiplierColors should have " + ResourceValue.Instance.MaxValue + " colors!", gameObject);
        }
    }

    private void OnEnable()
    {
        Player.DiedEvent += Deactivate;
        ResourceValue.ResourceValueUpdatedEvent += UpdateBarValue;
        ScoreMultiplierUI.ScoreMultiplierUpdatedEvent += OnScoreMultiplierUpdated;
    }

    private void OnDisable()
    {
        Player.DiedEvent -= Deactivate;
        ResourceValue.ResourceValueUpdatedEvent -= UpdateBarValue;
        ScoreMultiplierUI.ScoreMultiplierUpdatedEvent += OnScoreMultiplierUpdated;
    }
}	