using UnityEngine;
using UnityEngine.XR;

public class VRSwitch : MonoBehaviour
{
    public void Switch()
    {
        XRSettings.enabled = !XRSettings.enabled;
    }

    private void Start()
    {
        Debug.Log("Switching");
        XRSettings.enabled = false;
    }
}