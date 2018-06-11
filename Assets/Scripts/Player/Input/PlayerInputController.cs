using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to activate and deactivate an player input type.
/// </summary>
public class PlayerInputController : MonoBehaviour
{
    /// <summary>
    /// The selected VR input type when UseDefaultVRPlayerInputType is false.
    /// </summary>
    public static PlayerInputType VRPlayerInputType
    {
        get
        {
            int _playerInputTypeIndex = PlayerPrefs.GetInt(VR_PLAYER_INPUT_TYPE, 0);
            return (PlayerInputType)_playerInputTypeIndex;
        }
        set
        {
            UseDefaultVRPlayerInputType = false;
            PlayerPrefs.SetInt(VR_PLAYER_INPUT_TYPE, (int)value);
            PlayerPrefs.Save();

            if(VRPlayerInputTypeUpdatedEvent != null)
            {
                VRPlayerInputTypeUpdatedEvent(value);
            }
        }
    }

    /// <summary>
    /// The selected Non VR input type when UseDefaultNonVRPlayerInputType is false.
    /// </summary>
    public static PlayerInputType NonVRPlayerInputType
    {
        get
        {
            int _playerInputTypeIndex = PlayerPrefs.GetInt(NON_VR_PLAYER_INPUT_TYPE, 0);
            return (PlayerInputType)_playerInputTypeIndex;
        }
        set
        {
            UseDefaultNonVRPlayerInputType = false;
            PlayerPrefs.SetInt(NON_VR_PLAYER_INPUT_TYPE, (int)value);
            PlayerPrefs.Save();

            if (NonVRPlayerInputTypeUpdatedEvent != null)
            {
                NonVRPlayerInputTypeUpdatedEvent(value);
            }
        }
    }

    public static PlayerInputController Instance { get { return GetInstance(); } }

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
    public static Action<PlayerInputType> VRPlayerInputTypeUpdatedEvent;
    public static Action<PlayerInputType> NonVRPlayerInputTypeUpdatedEvent;
    public static Action<Vector3> InputEvent;

    private static bool UseDefaultVRPlayerInputType
    {
        get { return Convert.ToBoolean(PlayerPrefs.GetInt(USE_DEFAULT_VR_PLAYER_INPUT_TYPE, 1)); }
        set { PlayerPrefs.SetInt(USE_DEFAULT_VR_PLAYER_INPUT_TYPE, Convert.ToInt16(value)); }
    }

    private static bool UseDefaultNonVRPlayerInputType
    {
        get { return Convert.ToBoolean(PlayerPrefs.GetInt(USE_DEFAULT_NON_VR_PLAYER_INPUT_TYPE, 1)); }
        set { PlayerPrefs.SetInt(USE_DEFAULT_NON_VR_PLAYER_INPUT_TYPE, Convert.ToInt16(value)); }
    }

    private const string VR_PLAYER_INPUT_TYPE = "VRPlayerInputType";
	private const string NON_VR_PLAYER_INPUT_TYPE = "NonVRPlayerInputType";
    private const string USE_DEFAULT_VR_PLAYER_INPUT_TYPE = "UseDefaultVRPlayerInputType";
    private const string USE_DEFAULT_NON_VR_PLAYER_INPUT_TYPE = "UseDefaultNonVRPlayerInputType";

    public bool State { get { return state; } }

    [SerializeField] List<PlayerBaseInput> playerInputs;
    [SerializeField] private PlayerInputType defaultVRPlayerInputType = PlayerInputType.Look;
    [SerializeField] private PlayerInputType defaultNonVRPlayerInputType = PlayerInputType.Drag;

    private Coroutine inputUpdateCoroutine;
    private PlayerBaseInput currentPlayerInputBase;
    private bool state;

    /// <summary>
    /// Enables/Disables the input controller.
    /// </summary>
    /// <param name="_enabled"></param>
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
            if(!UseDefaultVRPlayerInputType)
            {
                StartInput(VRPlayerInputType);
            } 
            else
            {
                StartInput(defaultVRPlayerInputType);
            }
        }
        else
        {
            if (!UseDefaultNonVRPlayerInputType)
            {
                StartInput(NonVRPlayerInputType);
            }
            else
            {
                StartInput(defaultNonVRPlayerInputType);
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

    private void Initiate()
    {
        SetState(true);
    }

    private void OnEnable()
    {
        PlayerCollisions.DiedEvent += StopCurrentInput;
        LookPositionOnPlane.InitiatedEvent += Initiate;
    }

    private void OnDisable()
    {
        PlayerCollisions.DiedEvent -= StopCurrentInput;
        LookPositionOnPlane.InitiatedEvent -= Initiate;
    }
}