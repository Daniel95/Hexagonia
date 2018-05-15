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
		resourceBar.fillAmount = Mathf.Lerp(ResourceValue.Instance.Value, ResourceValue.Instance.maxValue, 2f * Time.deltaTime);
	} 
}