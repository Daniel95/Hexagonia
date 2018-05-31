using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public static PlayerInput Instance { get { return GetInstance(); } }

    #region Singleton
    private static PlayerInput instance;

    private static PlayerInput GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<PlayerInput>();
        }
        return instance;
    }
    #endregion

    /// <summary>
    /// Parameters: Delta
    /// </summary>
    public static Action<Vector3> InputEvent;

    [SerializeField] private PlayerInputType vrPlayerInputType = PlayerInputType.Look;
    [SerializeField] private PlayerInputType nonVRPlayerInputType = PlayerInputType.Drag;
    [SerializeField] private float tiltSpeed = 1;
    [SerializeField] private float dragSpeed = 1;

    private PlayerInputType currentPlayerInputType;
    private Vector3 inputPosition;
    private static new bool enabled;

    public void SetState(bool _enabled)
    {
        enabled = _enabled;
        bool _previousState = enabled;

        if (enabled == _previousState) { return; }

        if (enabled)
        {
            UpdateInput();
        }
        else
        {
            StopCurrentInput();
        }
    }

    private void LookInput(Vector3 _lookPosition)
    {
        if(InputEvent != null)
        {
            InputEvent(_lookPosition);
        }
    }

    private void TiltInput(Vector3 _lookPosition)
    {
        inputPosition.x += (Input.acceleration.x * tiltSpeed) * Time.deltaTime;
        inputPosition.y = _lookPosition.y;
        inputPosition.z = _lookPosition.z;

        inputPosition = LookPositionOnPlane.Instance.ClampToPlane(inputPosition);

        if (InputEvent != null)
        {
            InputEvent(inputPosition);
        }
    }

    private void DragInput(Vector2 _dragPosition, Vector2 _delta)
    {
        inputPosition += ((Vector3)_delta * dragSpeed) * Time.deltaTime;
        inputPosition = LookPositionOnPlane.Instance.ClampToPlane(inputPosition);

        if (InputEvent != null)
        {
            InputEvent(inputPosition);
        }
    }

    private void UpdateInput()
    {
        if(!enabled) { return; }

        if(VRSwitch.VRState)
        {
            StartInput(vrPlayerInputType);
        }
        else
        {
            StartInput(nonVRPlayerInputType);
        }
    }

    private void StartInput(PlayerInputType _playerInputType)
    {
        StopCurrentInput();

        switch (_playerInputType)
        {
            case PlayerInputType.Look:
                LookPositionOnPlane.LookPositionUpdatedEvent += LookInput;
                break;
            case PlayerInputType.Tilt:
                Input.gyro.enabled = true;
                LookPositionOnPlane.LookPositionUpdatedEvent += TiltInput;
                break;
            case PlayerInputType.Drag:
                InputBase.DraggingInputEvent += DragInput;
                break;
        }

        currentPlayerInputType = _playerInputType;
    }

    private void StopCurrentInput()
    {
        switch (currentPlayerInputType)
        {
            case PlayerInputType.Look:
                LookPositionOnPlane.LookPositionUpdatedEvent -= LookInput;
                break;
            case PlayerInputType.Tilt:
                Input.gyro.enabled = false;
                LookPositionOnPlane.LookPositionUpdatedEvent -= TiltInput;
                break;
            case PlayerInputType.Drag:
                InputBase.DraggingInputEvent -= DragInput;
                break;
        }
    }

    private void OnSceneSwitchCompleted(Scenes? _previousScene, Scenes _nextScene)
    {
        SetState(VRSwitch.VRState);
    }

    private void OnEnable()
    {
        VRSwitch.SwitchedEvent += UpdateInput;
        SceneLoader.SceneSwitchCompletedEvent += OnSceneSwitchCompleted;
    }

    private void OnDisable()
    {
        VRSwitch.SwitchedEvent -= UpdateInput;
        SceneLoader.SceneSwitchCompletedEvent -= OnSceneSwitchCompleted;
    }
}
