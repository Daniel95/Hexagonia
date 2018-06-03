using System;
using UnityEngine;

public class DebugLibrary : MonoBehaviour
{
    public static bool FPSCounterEnabled
    {
        get { return Convert.ToBoolean(PlayerPrefs.GetInt(FPS_COUNTER_ENABLED, 0)); }
        set { PlayerPrefs.SetInt(FPS_COUNTER_ENABLED, Convert.ToInt16(value)); }
    }

    public static bool UseDefaultVRPlayerInputType
    {
        get { return Convert.ToBoolean(PlayerPrefs.GetInt(USE_DEFAULT_VR_PLAYER_INPUT_TYPE, 0)); }
        set { PlayerPrefs.SetInt(USE_DEFAULT_VR_PLAYER_INPUT_TYPE, Convert.ToInt16(value)); }
    }

    public static bool UseDefaultNonVRPlayerInputType
    {
        get { return Convert.ToBoolean(PlayerPrefs.GetInt(USE_DEFAULT_NON_VR_PLAYER_INPUT_TYPE, 0)); }
        set { PlayerPrefs.SetInt(USE_DEFAULT_NON_VR_PLAYER_INPUT_TYPE, Convert.ToInt16(value)); }
    }

    public static PlayerInputType VRPlayerInputType
    {
        get
        {
            int _playerInputTypeIndex = PlayerPrefs.GetInt(VR_PLAYER_INPUT_TYPE, 0);
            return (PlayerInputType)_playerInputTypeIndex;
        }
        set { PlayerPrefs.SetInt(VR_PLAYER_INPUT_TYPE, (int)value); }
    }

    public static PlayerInputType NonVRPlayerInputType
    {
        get
        {
            int _playerInputTypeIndex = PlayerPrefs.GetInt(NON_VR_PLAYER_INPUT_TYPE, 0);
            return (PlayerInputType)_playerInputTypeIndex;
        }
        set { PlayerPrefs.SetInt(NON_VR_PLAYER_INPUT_TYPE, (int)value); }
    }

    private const string FPS_COUNTER_ENABLED = "FPSCounterEnabled";
	private const string VR_PLAYER_INPUT_TYPE = "VRPlayerInputType";
	private const string NON_VR_PLAYER_INPUT_TYPE = "NonVRPlayerInputType";
	private const string USE_DEFAULT_VR_PLAYER_INPUT_TYPE = "UseDefaultVRPlayerInputType";
	private const string USE_DEFAULT_NON_VR_PLAYER_INPUT_TYPE = "UseDefaultNonVRPlayerInputType";
}