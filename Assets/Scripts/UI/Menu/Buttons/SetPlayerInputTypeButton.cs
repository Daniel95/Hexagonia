﻿using UnityEngine;

/// <summary>
/// Sets the player input type in the PlayerInputController to targetPlayerInputType when clicked or gazed.
/// </summary>
public class SetPlayerInputTypeButton : GazeButton
{
    private const string ACTIVE = "active";

    [SerializeField] private PlayerInputType targetPlayerInputType;
    [SerializeField] private bool vrState;
    [SerializeField] private Color activeColor = Color.white;
    [SerializeField] private Color inactiveColor = Color.yellow;

    private Animator animator;
    private bool activeState = false;
    
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

    protected override void OnEnable()
    {
        base.OnEnable();

        SetActiveState(activeState);

        if (vrState)
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

    private void OnPlayerInputTypeUpdated(PlayerInputType _playerInputType)
    {
        bool _active = _playerInputType == targetPlayerInputType;
        Color _targetColor = _active ? activeColor : inactiveColor;
        Button.targetGraphic.color = _targetColor;
        Button.targetGraphic.raycastTarget = !_active;
        SetActiveState(_active);
    }

    private void SetActiveState(bool _active)
    {
        animator.SetBool(ACTIVE, _active);
        activeState = _active;
    }

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
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
}