using UnityEngine;
using UnityEngine.XR;
using System.Collections;

public class VRSwitch : MonoBehaviour
{
    public void Switch()
    {
        XRSettings.enabled = !XRSettings.enabled;
    }

    private void Start()
    {
        XRSettings.enabled = false;
    }
}