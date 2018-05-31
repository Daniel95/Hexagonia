
using System;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// Contains current state of VR mode, and has public methods to switch VR mode.
/// </summary>
public class VRSwitch : MonoBehaviour
{
    public static VRSwitch Instance { get { return GetInstance(); } }
    public static bool VRState { get { return vrState; } }

    public static Action SwitchedEvent;

    private static bool vrState;

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

    private const string VR_MODE = "VRMode";

    public GameObject GVRReticlePointerGameObject { get { return gvrReticlePointerGameObject; } }

    [SerializeField] private GameObject gvrGameObject;

    private GameObject gvrReticlePointerGameObject;

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

        if(SwitchedEvent != null)
        {
            SwitchedEvent();
        }

        return vrState;
    }

    private void Start()
    {
        gvrReticlePointerGameObject = FindObjectOfType<GvrReticlePointer>().gameObject;

        vrState = Convert.ToBoolean(PlayerPrefs.GetInt(VR_MODE));
        XRSettings.enabled = vrState;
        gvrGameObject.SetActive(vrState);

        if (SwitchedEvent != null)
        {
            SwitchedEvent();
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
        Player.DiedEvent += SetReticlePointer;
    }

    private void OnDisable()
    {
        Player.DiedEvent -= SetReticlePointer;
    }
}