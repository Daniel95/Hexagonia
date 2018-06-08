using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// This script prevents the user from pressing any buttons while loading between scenes
/// </summary>
public class EventSystemHandler : MonoBehaviour
{
    [SerializeField] private GameObject gvrEventSystemGameObject;
    [SerializeField] private GameObject genericEventSystemGameObject;

    private void OnEnable()
    {
        SceneLoader.FadeSceneOutStartedEvent += ToggleEventSystems;
        SceneLoader.FadeSceneInCompletedEvent += ToggleEventSystems;

        SceneLoader.FadeSceneInStartedEvent += ToggleEventSystemsAfterFrame;
    }

    private void OnDisable()
    {
        SceneLoader.FadeSceneOutStartedEvent -= ToggleEventSystems;
        SceneLoader.FadeSceneInCompletedEvent -= ToggleEventSystems;

        SceneLoader.FadeSceneInStartedEvent -= ToggleEventSystemsAfterFrame;
    }

    private void ToggleEventSystemsAfterFrame()
    {
        CoroutineHelper.DelayFrames(1, ToggleEventSystems);
    }

    private void ToggleEventSystems()
    {
        if (gvrEventSystemGameObject.activeSelf || genericEventSystemGameObject.activeSelf)
        {
            gvrEventSystemGameObject.SetActive(false);
            genericEventSystemGameObject.SetActive(false);
        }
        else
        {
            if (VRSwitch.VRState)
            {
                gvrEventSystemGameObject.SetActive(true);
            }
            else
            {
                gvrEventSystemGameObject.SetActive(true);
                genericEventSystemGameObject.SetActive(true);
            }
        }
    }
}
