using System.ComponentModel;
using SRF;
using Debug = UnityEngine.Debug;

public partial class SROptions
{
    [Category("Debug")] [Sort(-1)] public bool FPSCounterEnabled { get { return DebugLibrary.FPSCounterEnabled; } set { DebugLibrary.FPSCounterEnabled = value; } }

    [Category("VR Controls")]
    [Sort(-1)]
    public void UseDefaultVRControls()
    {
        DebugLibrary.UseDefaultVRPlayerInputType = true;
    }

    [Category("VR Controls")]
    [Sort(-1)]
    public void UseLookControls()
    {
        DebugLibrary.UseDefaultVRPlayerInputType = false;
        DebugLibrary.VRPlayerInputType = PlayerInputType.Look;
    }

    [Category("VR Controls")]
    [Sort(-1)]
    public void UseTiltControls()
    {
        DebugLibrary.UseDefaultVRPlayerInputType = false;
        DebugLibrary.VRPlayerInputType = PlayerInputType.Tilt;
    }

    [Category("Non VR Controls")]
    [Sort(-1)]
    public void UseDefaultNonVRControls()
    {
        DebugLibrary.UseDefaultNonVRPlayerInputType = true;
    }

    [Category("Non VR Controls")]
    [Sort(-1)]
    public void UseDragControls()
    {
        DebugLibrary.UseDefaultNonVRPlayerInputType = false;
        DebugLibrary.NonVRPlayerInputType = PlayerInputType.Drag;
    }

    [Category("Non VR Controls")]
    [Sort(-1)]
    public void UseTouchControls()
    {
        DebugLibrary.UseDefaultNonVRPlayerInputType = false;
        DebugLibrary.NonVRPlayerInputType = PlayerInputType.Touch;
    }

    private void OnValueChanged(string _n, object _newValue)
    {
        Debug.Log("[SRDebug] {0} value changed to {1}".Fmt(_n, _newValue));
        OnPropertyChanged(_n);
    }
}