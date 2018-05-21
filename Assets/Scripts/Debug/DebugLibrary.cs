using System;
using UnityEngine;

public class DebugLibrary : MonoBehaviour
{

    #region Singleton
    public static DebugLibrary Instance { get { return GetInstance(); } }

    private static DebugLibrary instance;

    private const string DEBUG_LIBRARY_PATH = "Debug/DebugLibrary";

    private static DebugLibrary GetInstance()
    {
        if(instance == null)
        {
            instance = Resources.Load<DebugLibrary>(DEBUG_LIBRARY_PATH);
        }
        return instance;
    }
    #endregion

    public bool DebugMode
    {
        get
        {
            return debugMode;
        }
    }

    [SerializeField] private bool debugMode;

}
