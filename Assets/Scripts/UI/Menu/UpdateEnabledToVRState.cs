using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Disables/enables the attached gameobject depending on the VR state.
/// </summary>
public class UpdateEnabledToVRState : MonoBehaviour
{
    [SerializeField] private bool enabledIsInvertedVRState;

    private void UpdateEnabled()
    {
        bool _enabled = enabledIsInvertedVRState ? !VRSwitch.VRState : VRSwitch.VRState;

        List<Transform> _children = transform.FirstLayerChildren();
        foreach (Transform _child in _children)
        {
            _child.gameObject.SetActive(_enabled);
        }
    }

    private void OnVRSwitched()
    {
        UpdateEnabled();
    }

    private void Awake()
    {
        UpdateEnabled();
    }

    private void OnEnable()
    {
        VRSwitch.SwitchedEvent += OnVRSwitched;
    }

    private void OnDisable()
    {
        VRSwitch.SwitchedEvent -= OnVRSwitched;
    }
}
