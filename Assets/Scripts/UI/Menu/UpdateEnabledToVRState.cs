using UnityEngine;

public class UpdateEnabledToVRState : MonoBehaviour
{
    [SerializeField] private bool enabledIsInvertedVRState;

    private void UpdateEnabled()
    {
        bool _enabled = enabledIsInvertedVRState ? !VRSwitch.VRState : VRSwitch.VRState;
        gameObject.SetActive(_enabled);
    }

    private void Awake()
    {
        UpdateEnabled();
    }

    private void OnVRSwitched()
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
