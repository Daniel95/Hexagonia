using UnityEngine;

public class UpdateEnabledToVRState : MonoBehaviour
{

    [SerializeField] private bool enabledIsInvertedVRState;

    private void UpdateEnabled()
    {
        enabled = enabledIsInvertedVRState ? !VRSwitch.VRState : !VRSwitch.VRState;
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
