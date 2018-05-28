using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Updates the resourcebar fill amount and color.
/// </summary>
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

	[SerializeField] private Image resourceBar;

    /// <summary>
    /// Updates the value of resource bar.
    /// </summary>
	public void UpdateBarValue()
	{
        float _barValue = 1;

        if(ResourceValue.Instance.Value < ResourceValue.Instance.MaxValue)
        {
            _barValue = ResourceValue.Instance.Value % 1;
        }

        resourceBar.fillAmount = _barValue;
    }

    /// <summary>
    /// Update the color of the resource bar.
    /// </summary>
    public void UpdateColor(Color _color)
	{
		resourceBar.color = _color;
	}

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Player.DiedEvent += Deactivate;
    }

    private void OnDisable()
    {
        Player.DiedEvent -= Deactivate;
    }
}	