using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Updates the resourcebar fill amount and color also rotates the canvas to the ratio amount of the player.
/// </summary>
public class ResourceBarUI //: MonoBehaviour
{
	[SerializeField] private float rotationAmount = 0.5f;
	[SerializeField] private RectTransform rotateCanvas;
	[SerializeField] private Image resourceBarImage;

	private void RotateBar()
	{
		float _x = PlayerAnimations.Ratio.x * Mathf.Rad2Deg;
		float _y = PlayerAnimations.Ratio.y * Mathf.Rad2Deg;

		rotateCanvas.rotation = Quaternion.Euler(-_y * rotationAmount, 0, -_x * rotationAmount);
	}

	private void UpdateBarValue(float _value)
	{
        float _barValue = 1;

        if(_value < ResourceValue.Instance.MaxValue)
        {
            _barValue = _value % 1;
        }

        resourceBarImage.fillAmount = _barValue;
    }

    private void OnScoreMultiplierUpdated(int _multiplier)
	{
        resourceBarImage.color = ScoreMultiplier.Instance.MultiplierColor;
	}

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private void Awake()
    {
        rotateCanvas = GetComponent<RectTransform>();
        resourceBarImage.fillAmount = 0;
    }

    private void Update()
    {
        RotateBar();
    }

    private void OnEnable()
    {
        PlayerCollisions.DiedEvent += Deactivate;
        ResourceValue.UpdatedEvent += UpdateBarValue;
        ScoreMultiplier.UpdatedEvent += OnScoreMultiplierUpdated;
    }

    private void OnDisable()
    {
        PlayerCollisions.DiedEvent -= Deactivate;
        ResourceValue.UpdatedEvent -= UpdateBarValue;
        ScoreMultiplier.UpdatedEvent -= OnScoreMultiplierUpdated;
    }
}	