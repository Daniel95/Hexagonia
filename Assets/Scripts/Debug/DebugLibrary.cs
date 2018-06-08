using System;
using UnityEngine;

public class DebugLibrary : MonoBehaviour
{
    public static bool FPSCounterEnabled
    {
        get { return Convert.ToBoolean(PlayerPrefs.GetInt(FPS_COUNTER_ENABLED, 0)); }
        set { PlayerPrefs.SetInt(FPS_COUNTER_ENABLED, Convert.ToInt16(value)); }
    }

    private const string FPS_COUNTER_ENABLED = "FPSCounterEnabled";
}