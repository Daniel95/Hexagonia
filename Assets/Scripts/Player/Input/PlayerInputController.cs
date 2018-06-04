using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    public static PlayerInputController Instance { get { return GetInstance(); } }
    public static bool State { get { return state; } }

    #region Singleton
    private static PlayerInputController instance;

    private static PlayerInputController GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<PlayerInputController>();
        }
        return instance;
    }
    #endregion

    /// <summary>
    /// Parameters: TargetPosition
    /// </summary>
    public static Action<Vector3> InputEvent;

    private static bool state;

    [SerializeField] List<PlayerBaseInput> playerInputs;
    [SerializeField] private PlayerInputType vrPlayerInputType = PlayerInputType.Tilt;
    [SerializeField] private PlayerInputType nonVRPlayerInputType = PlayerInputType.Drag;

    private Coroutine inputUpdateCoroutine;
    private PlayerBaseInput currentPlayerInputBase;

    public void SetState(bool _enabled)
    {
        bool _previousState = state;
        state = _enabled;

        if (state == _previousState) { return; }

        if (state)
        {
            UpdateInput();
        }
        else
        {
            StopCurrentInput();
        }
    }

    private void UpdateInput()
    {
        if(!state) { return; }

        if(VRSwitch.VRState)
        {
            if(!DebugLibrary.UseDefaultVRPlayerInputType)
            {
                StartInput(DebugLibrary.VRPlayerInputType);
            } 
            else
            {
                StartInput(vrPlayerInputType);
            }
        }
        else
        {
            if (!DebugLibrary.UseDefaultNonVRPlayerInputType)
            {
                StartInput(DebugLibrary.NonVRPlayerInputType);
            }
            else
            {
                StartInput(nonVRPlayerInputType);
            }
        }
    }

    private void OnTargetPositionUpdated(Vector3 _targetPosition)
    {
        if (InputEvent != null)
        {
            InputEvent(_targetPosition);
        }
    }

    private void StartInput(PlayerInputType _playerInputType)
    {
        PlayerBaseInput _playerInputBase = playerInputs.Find(x => x.PlayerInputType == _playerInputType);

        if(_playerInputBase == null)
        {
            Debug.LogError("No PlayerInput of type " + _playerInputType, gameObject);
            return;
        }

        StopCurrentInput();

        currentPlayerInputBase = _playerInputBase;
        currentPlayerInputBase.TargetPositionUpdatedEvent += OnTargetPositionUpdated;
        currentPlayerInputBase.Activate();
    }

    private void StopCurrentInput()
    {
        if(inputUpdateCoroutine != null)
        {
            StopCoroutine(inputUpdateCoroutine);
            inputUpdateCoroutine = null;
        }

        if(currentPlayerInputBase != null)
        {
            currentPlayerInputBase.TargetPositionUpdatedEvent -= OnTargetPositionUpdated;
            currentPlayerInputBase.Deactivate();
            currentPlayerInputBase = null;
        }
    }

    private void Awake()
    {
        SetState(true);
    }

    private void OnEnable()
    {
        Player.DiedEvent += StopCurrentInput;
    }

    private void OnDisable()
    {
        Player.DiedEvent -= StopCurrentInput;
    }
}