using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// Contains current state of VR mode, and has public methods to switch VR mode.
/// </summary>
public class VRSwitch : MonoBehaviour
{
    public static VRSwitch Instance { get { return GetInstance(); } }
    public static bool VRState { get { return vrState; } }
    public GameObject GVRReticlePointerGameObject { get { return gvrReticlePointerGameObject; } }

    public static Action SwitchedEvent;

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

    private static bool vrState;

    private const string VR_MODE = "VRMode";
    private const string VR_CARDBOARD = "cardboard";
    private const string VR_NONE = "None";

    [SerializeField] private GameObject gvrGameObject;

    private GameObject gvrReticlePointerGameObject;

    /// <summary>
    /// Switches the VR Mode, returns the VR State.
    /// </summary>
    /// <returns></returns>
    public bool Switch()
    {
        vrState = !vrState;

        StartCoroutine(LoadDevice());

        PlayerPrefs.SetInt(VR_MODE, Convert.ToInt32(vrState));
        PlayerPrefs.Save();

        gvrGameObject.SetActive(vrState);
        gvrReticlePointerGameObject.SetActive(vrState);

        if (SwitchedEvent != null)
        {
            SwitchedEvent();
        }

        return vrState;
    }

    private void Start()
    {
        gvrReticlePointerGameObject = FindObjectOfType<GvrReticlePointer>().gameObject;

        vrState = Convert.ToBoolean(PlayerPrefs.GetInt(VR_MODE));
        gvrGameObject.SetActive(vrState);
        gvrReticlePointerGameObject.SetActive(vrState);

        if (SwitchedEvent != null)
        {
            SwitchedEvent();
        }
    }

    private void Initialize()
    {
        StartCoroutine(LoadDevice());
        SceneLoader.FadeSceneInStartedEvent -= Initialize;
    }

    IEnumerator LoadDevice()
    {
        if (VRState)
        {
            XRSettings.LoadDeviceByName(VR_CARDBOARD);
        }
        else
        {
            XRSettings.LoadDeviceByName(VR_NONE);
        }
        yield return null;
        XRSettings.enabled = vrState;
#if !UNITY_EDITOR
        if (vrState)
        {
            GvrCardboardHelpers.Recenter();
        }
#endif
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
        PlayerCollisions.DiedEvent += SetReticlePointer;
        VRModeButton.InitializedEvent += SetReticlePointer;
        SceneLoader.FadeSceneInStartedEvent += Initialize;
    }

    private void OnDisable()
    {
        PlayerCollisions.DiedEvent -= SetReticlePointer;
        VRModeButton.InitializedEvent -= SetReticlePointer;
        SceneLoader.FadeSceneInStartedEvent -= Initialize;
    }
}