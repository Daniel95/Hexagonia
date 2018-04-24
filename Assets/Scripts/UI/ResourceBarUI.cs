using UnityEngine;

public class ResourceBarUI : MonoBehaviour
{

	private float scaleX = 0f;
	private float scaleBarByScore;
	private float offset = 1f;
	[SerializeField] private GameObject resourceBar;

	private void Awake()
	{
		resourceBar.transform.localScale = new Vector3(0, resourceBar.transform.localScale.y, resourceBar.transform.localScale.z);
	}

	private void Update()
    {
		UpdateResourceBar();
    }

	private void UpdateResourceBar()
	{
		if (scaleBarByScore < 1)
		{
			scaleBarByScore = scaleX + ResourceValue.Instance.Value;
			resourceBar.transform.position = new Vector3((scaleBarByScore - offset) / 0.5f, resourceBar.transform.position.y, resourceBar.transform.position.z);

			resourceBar.transform.localScale = new Vector3(scaleBarByScore, resourceBar.transform.localScale.y, resourceBar.transform.localScale.z);
		}
	}
}