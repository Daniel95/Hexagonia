using System;
using UnityEngine;

public class DebugLibrary : MonoBehaviour
{
    public static bool FPSCounterEnabled
    {
        get
        {
            return Convert.ToBoolean(PlayerPrefs.GetInt(FPS_COUNTER_ENABLED, 0));
        }
        set
        {
            PlayerPrefs.SetInt(FPS_COUNTER_ENABLED, Convert.ToInt16(value));
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
            return Convert.ToBoolean(PlayerPrefs.GetInt(LOOK_CONTROLS_ENABLED, 0));
        }
        set
        {
            PlayerPrefs.SetInt(LOOK_CONTROLS_ENABLED, Convert.ToInt16(value));
            if(LookControlsEnabledChanged != null)
            {
                LookControlsEnabledChanged(value);
            }
        }
    }

    public static Action<bool> FPSCounterEnabledChangedEvent;
    public static Action<bool> LookControlsEnabledChanged;

	private const string LOOK_CONTROLS_ENABLED = "LookControlsEnabled";
	private const string FPS_COUNTER_ENABLED = "FPSCounterEnabled";
}