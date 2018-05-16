using UnityEngine;
using UnityEngine.XR;
using System;

public class VRSwitch : MonoBehaviour
{
    [SerializeField] Gyro gyro;

    private void Start()
    {
        XRSettings.enabled = false;
        gyro.enabled = !XRSettings.enabled;
    }

    public void Switch()
    {
        XRSettings.enabled = !XRSettings.enabled;
        gyro.enabled = !XRSettings.enabled;
    }
}