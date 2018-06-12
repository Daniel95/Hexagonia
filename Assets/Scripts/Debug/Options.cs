using System.ComponentModel;
using SRF;
using UnityEngine;
using Debug = UnityEngine.Debug;

public partial class SROptions
{
    //[Category("Debug")] [Sort(-1)] public bool FPSCounterEnabled { get { return DebugLibrary.FPSCounterEnabled; } set { DebugLibrary.FPSCounterEnabled = value; } }

    [Category("Debug")]
    [Sort(-1)]
    public void GoToMenuScene()
    {
        SceneLoader.Instance.SwitchScene(Scenes.Menu);
    }

    [Category("Debug")]
    [Sort(-1)]
    public void DeletePlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    [Category("VR Controls")]
    [Sort(-1)]
    public void UseLookControls()
    {
        PlayerInputController.VRPlayerInputType = PlayerInputType.Look;
    }

    [Category("VR Controls")]
    [Sort(-1)]
    public void UseTiltControls()
    {
        PlayerInputController.VRPlayerInputType = PlayerInputType.Tilt;
    }
    
    private void OnValueChanged(string _n, object _newValue)
    {
        Debug.Log("[SRDebug] {0} value changed to {1}".Fmt(_n, _newValue));
        OnPropertyChanged(_n);
    }
}