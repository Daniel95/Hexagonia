using UnityEngine;
using UnityEngine.UI;

public class ResourceBarUI : MonoBehaviour
{

	public static ResourceBarUI Instance { get { return GetInstance(); } }

	#region Instance
	private static ResourceBarUI instance;

	private static ResourceBarUI GetInstance()
	{
		if (instance == null)
		{
			instance = FindObjectOfType<ResourceBarUI>();
		}
		return instance;
	}
	#endregion

	public Image resourceBar;

	public void UpdateResourceBar()
	{
		float _barValue = ResourceValue.Instance.Value - ScoreMultiplier.Instance.Multiplier;
		resourceBar.fillAmount = _barValue;
	} 

	public void ChangeColor(Color _color)
	{
		resourceBar.color = _color;
	}
}	