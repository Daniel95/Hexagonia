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

	[SerializeField] private Image resourceBar;

	public void UpdateResourceBar()
	{
        float _barValue = 1;

        if(ResourceValue.Instance.Value < ResourceValue.Instance.MaxValue)
        {
            _barValue = ResourceValue.Instance.Value % 1;
        }

        resourceBar.fillAmount = _barValue;
    } 

	public void ChangeColor(Color _color)
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