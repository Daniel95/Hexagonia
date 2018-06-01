using System.ComponentModel;
using SRF;
using Debug = UnityEngine.Debug;

public partial class SROptions
{
    [Category("FPS counter")]
    [Sort(-1)]
    public bool FPSCounterEnabled
    {
        get
        {
            return DebugLibrary.FPSCounterEnabled;
        }
        set
        {
            DebugLibrary.FPSCounterEnabled = value;
        }
    }

    [Category("Look controls")]
    [Sort(-1)]
    public bool LookControlsEnabled
    {
        get
        {
            return DebugLibrary.LookControlsEnabled;
        }
        set
        {
            DebugLibrary.LookControlsEnabled = value;
        }
    }

    private void OnValueChanged(string _n, object _newValue)
    {
        Debug.Log("[SRDebug] {0} value changed to {1}".Fmt(_n, _newValue));
        OnPropertyChanged(_n);
    }
}