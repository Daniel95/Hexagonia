using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Updates the resourcebar fill amount and color.
/// </summary>
public class ResourceBarUI : MonoBehaviour
{
	[SerializeField] private Image resourceBar;

    private void Awake()
    {
        resourceBar.fillAmount = 0;
    }

    private void UpdateBarValue(float _value)
	{
        float _barValue = 1;

        if(_value < ResourceValue.Instance.MaxValue)
        {
            _barValue = _value % 1;
        }

        resourceBar.fillAmount = _barValue;
    }

    private void OnScoreMultiplierUpdated(int _multiplier)
	{
        resourceBar.color = ScoreMultiplier.Instance.MultiplierColor;
	}

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Player.DiedEvent += Deactivate;
        ResourceValue.UpdatedEvent += UpdateBarValue;
        ScoreMultiplier.UpdatedEvent += OnScoreMultiplierUpdated;
    }

    private void OnDisable()
    {
        Player.DiedEvent -= Deactivate;
        ResourceValue.UpdatedEvent -= UpdateBarValue;
        ScoreMultiplier.UpdatedEvent -= OnScoreMultiplierUpdated;
    }
}	