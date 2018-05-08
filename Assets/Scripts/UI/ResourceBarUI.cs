using UnityEngine;
using System.Collections;

public class ResourceBarUI : MonoBehaviour
{
	private float scaleX = 0f;
	private float scaleBarByScore = 0f;
	private float offset = -2.3f;
	private float waitTime = 4f;
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
		}
		if(scaleBarByScore != 0)
		{
			StartCoroutine("DecreaseOverTime");
		}
		if(scaleBarByScore > 1) //BUG, Bar not decreasing when over 1 / when full
		{
			scaleX = 1f;
		}
		resourceBar.transform.position = new Vector3(offset, resourceBar.transform.position.y, resourceBar.transform.position.z);
		resourceBar.transform.localScale = new Vector3(scaleBarByScore, resourceBar.transform.localScale.y, resourceBar.transform.localScale.z);
	}

	private IEnumerator DecreaseOverTime()
	{
		scaleX -= 0.05f * Time.deltaTime;
		yield return new WaitForSeconds(waitTime);
	}
}