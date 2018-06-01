using UnityEngine;
using System;
using System.ComponentModel;
using System.Diagnostics;
using SRDebugger;
using SRDebugger.Services;
using SRF;
using SRF.Service;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public partial class SROptions
{
    DebugLibrary debugLibrary;

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

    private void OnValueChanged(string n, object newValue)
    {
        Debug.Log("[SRDebug] {0} value changed to {1}".Fmt(n, newValue));
        OnPropertyChanged(n);
    }
}
