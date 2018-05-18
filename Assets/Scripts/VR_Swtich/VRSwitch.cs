using UnityEngine;
using UnityEngine.XR;
using System;

public class VRSwitch : MonoBehaviour
{
    public static Action<bool> VRModeSwitchedEvent;

    public static VRSwitch Instance
    {
        get
        {
            return GetInstance();
        }
    }

    public bool VrState
    {
        get
        {
            return vrState;
        }
    }

    #region Singleton
    private static VRSwitch instance;

    private static VRSwitch GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<VRSwitch>();
        }
        return instance;
    }
    #endregion

    [SerializeField] private GameObject gvrGameObject;

    //[SerializeField] Gyro gyro;
    private bool vrState;


    private void Start()
    {
        //TODO Set starting VRState according to playerprefs
        //XRSettings.enabled = false;
        //gvrGameObject.SetActive(false);
        //vrState = false;
        //gyro.enabled = !XRSettings.enabled;
    }

    public bool Switch()
    {
        XRSettings.enabled = !XRSettings.enabled;
        vrState = !vrState;
        Debug.Log("switched");
        //gvrGameObject.SetActive(vrState);

        if (VRModeSwitchedEvent != null)
        {
            VRModeSwitchedEvent(vrState);
        }
        //gyro.enabled = !XRSettings.enabled;
        return vrState;
    }
}