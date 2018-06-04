using System.Collections;
using UnityEngine;
using UnityEngine.XR;

public class RuntimeCardboardLoader : MonoBehaviour
{
	void Start () {
		StartCoroutine(LoadDevice("cardboard"));		
	}

	IEnumerator LoadDevice(string newDevice)
	{
		XRSettings.LoadDeviceByName(newDevice);
		yield return null;
		XRSettings.enabled = true;
	}
}