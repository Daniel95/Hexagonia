using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Updates the resourcebar fill amount and color also rotates the canvas to the ratio amount of the player.
/// </summary>
public class ResourceBarUI : MonoBehaviour
{
	[SerializeField] private float rotationAmount = 0.5f;
	[SerializeField] private RectTransform rotateCanvas;
	[SerializeField] private Image resourceBarImage;

	private void Awake()
    {
		rotateCanvas = GetComponent<RectTransform>();
		resourceBarImage.fillAmount = 0;
	}

	private void Update()
	{
		RotateBar();
	}

	private void RotateBar()
	{
		float resX = rotateCanvas.rotation.x;
		float resY = rotateCanvas.rotation.y;
		float _X = Player.Instance.Ratio.x;
		float _Y = Player.Instance.Ratio.y;

		resY = _Y * (Mathf.Rad2Deg);
		resX = _X * (Mathf.Rad2Deg);

		rotateCanvas.rotation = Quaternion.Euler((-resY * rotationAmount), 0, (-resX * rotationAmount));
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