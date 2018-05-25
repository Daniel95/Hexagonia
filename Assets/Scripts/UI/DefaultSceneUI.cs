using UnityEngine;

public class DefaultSceneUI : MonoBehaviour
{

    public static DefaultSceneUI Instance
    {
        get
        {
            return GetInstance();
        }
    }

    #region Singleton
    private static DefaultSceneUI instance;

    private static DefaultSceneUI GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<DefaultSceneUI>();
        }
        return instance;
    }
    #endregion

    public ScriptedAnimationController ScriptedAnimationController { get { return scriptedAnimationController; } }

    [SerializeField] private ScriptedAnimationController scriptedAnimationController;

}
