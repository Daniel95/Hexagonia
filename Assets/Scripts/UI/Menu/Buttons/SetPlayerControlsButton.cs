using UnityEngine;

public class SetPlayerControlsButton : GazeButton
{
    [SerializeField] private PlayerInputType targetPlayerInputType;
    [SerializeField] private bool vrState;
    [SerializeField] private Color activeColor = Color.white;
    [SerializeField] private Color inactiveColor = Color.yellow;

    protected override void OnTrigger()
    {
        if(vrState)
        {
            PlayerInputController.VRPlayerInputType = targetPlayerInputType;
        }
        else
        {
            PlayerInputController.NonVRPlayerInputType = targetPlayerInputType;
        }
    }

    private void OnPlayerInputTypeUpdated(PlayerInputType _playerInputType)
    {
        bool _active = _playerInputType == targetPlayerInputType;
        Color _targetColor = _active ? activeColor : inactiveColor;
        Button.targetGraphic.color = _targetColor;
        Button.raycastTarget = !_active;
    }

    private void Start()
    {
        if (vrState)
        {
            OnPlayerInputTypeUpdated(PlayerInputController.VRPlayerInputType);
        }
        else
        {
            OnPlayerInputTypeUpdated(PlayerInputController.NonVRPlayerInputType);
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if(vrState)
        {
            PlayerInputController.VRPlayerInputTypeUpdatedEvent += OnPlayerInputTypeUpdated;
        }
        else
        {
            PlayerInputController.NonVRPlayerInputTypeUpdatedEvent += OnPlayerInputTypeUpdated;
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (vrState)
        {
            PlayerInputController.VRPlayerInputTypeUpdatedEvent -= OnPlayerInputTypeUpdated;
        }
        else
        {
            PlayerInputController.NonVRPlayerInputTypeUpdatedEvent -= OnPlayerInputTypeUpdated;
        }
    }
}
