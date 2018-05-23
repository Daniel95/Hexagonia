using UnityEngine;
using UnityEngine.XR;
using System;
using System.Collections;

public class VRSwitch : MonoBehaviour
{
    public static Action VRModeSwitchedEvent;

    public static VRSwitch Instance
    {
        get
        {
            return GetInstance();
        }
    }

    public GameObject GvrReticlePointerGameObject
    {
        get
        {
            return gvrReticlePointerGameObject;
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

    public bool VrState { get; set; }

    [SerializeField] private GameObject gvrGameObject;
    private GameObject gvrReticlePointerGameObject;
    private bool clicked;

    private void Awake()
    {
        gvrReticlePointerGameObject = FindObjectOfType<GvrReticlePointer>().gameObject;
        /*if (!XRSettings.enabled)
        {
            XRSettings.enabled = true;
        }
        */
        //gvrGameObject.SetActive(true);
        //VrState = true;
        //Debug.Log("AWOKEN");
    }

    private void SetVRModeAfterInitialization()
    {
        VrState = true;
        XRSettings.enabled = true;
        gvrGameObject.SetActive(true);

        if (PlayerPrefs.GetInt("VRMode") == 0)
        {
            if (VRModeSwitchedEvent != null)
            {
                VRModeSwitchedEvent();
            }
        }
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

        PlayerPrefs.SetInt("VRMode", Convert.ToInt32(VrState));
        PlayerPrefs.Save();

        gvrGameObject.SetActive(VrState);
        gvrReticlePointerGameObject.SetActive(VrState);

        if (VrState)
        {
            GvrCardboardHelpers.Recenter();
        }

        StartCoroutine(ClickedToFalseAfterFrame());
        return VrState;
    }

    private IEnumerator ClickedToFalseAfterFrame()
    {
        yield return new WaitForSeconds(0);
        clicked = false;
    }

    private void SetReticlePointer()
    {
        if (VrState)
        {
            gvrReticlePointerGameObject.SetActive(true);
        }
    }

    private void OnEnable()
    {
        VRModeButton.InitializedEvent += SetVRModeAfterInitialization;
        Player.PlayerDiedEvent += SetReticlePointer;
    }

    private void OnDisable()
    {
        VRModeButton.InitializedEvent -= SetVRModeAfterInitialization;
        Player.PlayerDiedEvent -= SetReticlePointer;
    }

}