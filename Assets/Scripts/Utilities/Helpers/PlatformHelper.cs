using UnityEngine;

public static class PlatformHelper {

    public static bool PlatformIsMobile { get { return Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer; } }

}
