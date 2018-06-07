using UnityEngine;

public class ParamicCubes : MonoBehaviour {

	[SerializeField] private int band;
	[SerializeField] private float startScale, maxScale;
	[SerializeField] private bool useBuffer;
	
	void Update () {
		if(useBuffer)
		{
			transform.localScale = new Vector3(transform.localScale.x, (AudioPeer_test.audioBandBuffer[band] * maxScale) + startScale, transform.localScale.z);
		}
		if(!useBuffer)
		{
			transform.localScale = new Vector3(transform.localScale.x, (AudioPeer_test.audioBand[band] * maxScale) + startScale, transform.localScale.z);
		}
	}
}