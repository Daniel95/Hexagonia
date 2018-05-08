using UnityEngine;

public class ResourceBarUI : MonoBehaviour
{
	private float scaleX = 0f;
	private float scaleBarByScore = 0f;
	private float offset = -2.3f;
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
			scaleBarByScore = scaleX + ResourceValue.Instance.Value; //Chained with the coin pick-up
			resourceBar.transform.position = new Vector3(offset, resourceBar.transform.position.y, resourceBar.transform.position.z);
			resourceBar.transform.localScale = new Vector3(scaleBarByScore, resourceBar.transform.localScale.y, resourceBar.transform.localScale.z);

			scaleBarByScore -= 0.2f * Time.deltaTime;
		}
		if(scaleBarByScore == 1)
		{
			
		}
	}
}