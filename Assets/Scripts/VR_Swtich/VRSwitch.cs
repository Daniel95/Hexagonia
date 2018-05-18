using UnityEngine;
using UnityEngine.XR;
using System;
using System.Collections;

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

    public bool VrState { get; private set; }

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
    private bool clicked;


    private void Start()
    {
        //TODO Set starting VRState according to playerprefs

        XRSettings.enabled = false;
        gvrGameObject.SetActive(false);
        VrState = false;

        //gyro.enabled = !XRSettings.enabled;
    }

    public bool Switch()
    {
        
        if (clicked)
        {
            return VrState;
        }
        clicked = true;

        XRSettings.enabled = !XRSettings.enabled;
        VrState = !VrState;

        gvrGameObject.SetActive(VrState);

        if (VrState)
        {
            GvrCardboardHelpers.Recenter();
        }



        /*
        if (VRModeSwitchedEvent != null)
        {
            VRModeSwitchedEvent(vrState);
        }
        */
        //gyro.enabled = !XRSettings.enabled;

        StartCoroutine(ClickedToFalseAfterFrame());
        return VrState;
    }

    private IEnumerator ClickedToFalseAfterFrame()
    {
        yield return new WaitForSeconds(0);
        clicked = false;
    }
}