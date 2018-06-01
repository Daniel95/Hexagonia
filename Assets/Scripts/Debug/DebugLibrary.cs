using System;
using UnityEngine;

public class DebugLibrary : MonoBehaviour
{
    public static bool FPSCounterEnabled
    {
        get
        {
            return Convert.ToBoolean(PlayerPrefs.GetInt("FPSCounterEnabled", 0));
        }
        set
        {
            PlayerPrefs.SetInt("FPSCounterEnabled", Convert.ToInt16(value));
            if(FPSCounterEnabledChangedEvent != null)
            {
                FPSCounterEnabledChangedEvent(value);
            }
        }
    }

    public static bool LookControlsEnabled
    {
        get
        {
            return Convert.ToBoolean(PlayerPrefs.GetInt("LookControlsEnabled", 0));
        }
        set
        {
            PlayerPrefs.SetInt("LookControlsEnabled", Convert.ToInt16(value));
            if(LookControlsEnabledChanged != null)
            {
                LookControlsEnabledChanged(value);
            }
        }
    }

    public static Action<bool> FPSCounterEnabledChangedEvent;
    public static Action<bool> LookControlsEnabledChanged;
}
