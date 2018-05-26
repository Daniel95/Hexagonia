using UnityEngine;

[RequireComponent(typeof(ScriptedAnimationController))]
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

    private ScriptedAnimationController scriptedAnimationController;

    private void Awake()
    {
        scriptedAnimationController = GetComponent<ScriptedAnimationController>();

        scriptedAnimationController.StartAnimation(ScriptedAnimationType.In, () => {
            scriptedAnimationController.StartAnimation(ScriptedAnimationType.Out, () => {
                scriptedAnimationController.StartAnimation(ScriptedAnimationType.In, () => {
                    scriptedAnimationController.StartAnimation(ScriptedAnimationType.Out, () => {
                        scriptedAnimationController.StartAnimation(ScriptedAnimationType.In, () => {
                            scriptedAnimationController.StartAnimation(ScriptedAnimationType.Out, () => {
                                scriptedAnimationController.StartAnimation(ScriptedAnimationType.In, () => {
                                    scriptedAnimationController.StartAnimation(ScriptedAnimationType.Out, () => {
                                        Debug.Log("Animation chain completed");
                                    });
                                });
                            });
                        });
                    });
                });
            });
        });
    }

}
