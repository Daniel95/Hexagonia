using System;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// Contains current state of VR mode, and has public methods to switch VR mode.
/// </summary>
public class VRSwitch : MonoBehaviour
{
    public static Action VRModeSwitchedEvent;
    public static VRSwitch Instance { get { return GetInstance(); } }

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

    public GameObject GVRReticlePointerGameObject { get { return gvrReticlePointerGameObject; } }
    public bool VRState { get { return vrState; } }

    private const string VR_MODE = "VRMode";

    [SerializeField] private GameObject gvrGameObject;

    private GameObject gvrReticlePointerGameObject;
    private bool vrState;

    /// <summary>
    /// Switches the VR Mode, returns the VR State.
    /// </summary>
    /// <returns></returns>
    public bool Switch()
    {
        vrState = !vrState;
        XRSettings.enabled = vrState;

        PlayerPrefs.SetInt(VR_MODE, Convert.ToInt32(vrState));
        PlayerPrefs.Save();

        gvrGameObject.SetActive(vrState);
        gvrReticlePointerGameObject.SetActive(vrState);
#if !UNITY_EDITOR
        if (vrState)
        {
            GvrCardboardHelpers.Recenter();
        }
#endif

        return vrState;
    }

    private void Awake()
    {
        gvrReticlePointerGameObject = FindObjectOfType<GvrReticlePointer>().gameObject;
    }

    private void SetVRModeAfterInitialization()
    {
        vrState = true;
        XRSettings.enabled = true;
        gvrGameObject.SetActive(true);

        if (PlayerPrefs.GetInt(VR_MODE) == 0)
        {
            if (VRModeSwitchedEvent != null)
            {
                VRModeSwitchedEvent();
            }
        }
    }

    private void SetReticlePointer()
    {
        if (vrState)
        {
            gvrReticlePointerGameObject.SetActive(true);
        }
    }

    private void OnEnable()
    {
        VRModeButton.InitializedEvent += SetVRModeAfterInitialization;
        Player.DiedEvent += SetReticlePointer;
    }

    private void OnDisable()
    {
        VRModeButton.InitializedEvent -= SetVRModeAfterInitialization;
        Player.DiedEvent -= SetReticlePointer;
    }
}