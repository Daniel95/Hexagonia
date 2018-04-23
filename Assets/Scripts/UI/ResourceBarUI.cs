using UnityEngine;
using UnityEngine.UI;

public class ResourceBarUI : MonoBehaviour
{

	private float startValue = 0f;
	private Image image;
	private float yVelocity = 0.0F;

	private void Awake()
	{
		image = GetComponent<Image>();
	}

	private void Update()
    {
		UpdateResourceBar();
    }

	private void UpdateResourceBar()
	{
		float _speed = 0.5f;
		float newValue = startValue + ResourceValue.Instance.Value;

		image.fillAmount = startValue + ResourceValue.Instance.Value;
	}
}