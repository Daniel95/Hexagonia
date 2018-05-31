using System;
using System.Collections;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public static PlayerInput Instance { get { return GetInstance(); } }
    public static bool State { get { return state; } }
    public static PlayerInputType CurrentPlayerInputType { get { return currentPlayerInputType; } }

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
    /// Parameters: TargetPosition
    /// </summary>
    public static Action<Vector3> InputEvent;

    private static PlayerInputType currentPlayerInputType;
    private static bool state;

    [SerializeField] private PlayerInputType vrPlayerInputType = PlayerInputType.Look;
    [SerializeField] private PlayerInputType nonVRPlayerInputType = PlayerInputType.Drag;
    [SerializeField] private float tiltSpeed = 1;
    [SerializeField] private float dragSpeed = 1;

    private Vector3 inputPosition;
    private Coroutine inputUpdateCoroutine;

    public void SetState(bool _enabled)
    {
        state = _enabled;
        bool _previousState = state;

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

    private IEnumerator InputUpdate()
    {
        while (true)
        {
            if (InputEvent != null)
            {
                InputEvent(inputPosition);
            }

            yield return null;
        }
    }

    private void LookInput(Vector3 _lookPosition)
    {
        inputPosition = _lookPosition;
    }

    private void TiltInput(Vector3 _lookPosition)
    {
        inputPosition.x += (Input.acceleration.x * tiltSpeed) * Time.deltaTime;
        inputPosition.y = _lookPosition.y;
        inputPosition.z = _lookPosition.z;

        inputPosition = LookPositionOnPlane.Instance.ClampToPlane(inputPosition);
    }

    private void DragInput(Vector2 _dragPosition, Vector2 _delta)
    {
        Vector2 _deltaSinceTouched = _dragPosition - InputBase.StartDownPosition;

        inputPosition += ((Vector3)_deltaSinceTouched * dragSpeed) * Time.deltaTime;
        inputPosition = LookPositionOnPlane.Instance.ClampToPlane(inputPosition);
    }

    private void UpdateInput()
    {
        if(!state) { return; }

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

        inputUpdateCoroutine = StartCoroutine(InputUpdate());

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
                inputPosition = LookPositionOnPlane.Instance.transform.position;

                if (InputEvent != null)
                {
                    InputEvent(inputPosition);
                }

                InputBase.DraggingInputEvent += DragInput;
                break;
        }

        currentPlayerInputType = _playerInputType;
    }

    private void StopCurrentInput()
    {
        if(inputUpdateCoroutine != null)
        {
            StopCoroutine(inputUpdateCoroutine);
        }

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

    private void Start()
    {
        SetState(true);
        UpdateInput();
    }

    private void OnSceneSwitchCompleted(Scenes? _previousScene, Scenes _nextScene)
    {
        SetState(VRSwitch.VRState);
        UpdateInput();
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
