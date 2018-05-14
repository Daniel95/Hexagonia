using UnityEngine;
using System.Collections;
using System;

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

	[SerializeField] private GameObject resourceBar;
	
	public void UpdateResourceBar()
	{
		resourceBar.transform.localScale = new Vector3(Mathf.Lerp(ResourceValue.Instance.Value, ResourceValue.Instance.maxValue, 2f * Time.deltaTime), resourceBar.transform.localScale.y, resourceBar.transform.localScale.z);
	}
}