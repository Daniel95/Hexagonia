using System.Collections.Generic;
using UnityEngine;

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
