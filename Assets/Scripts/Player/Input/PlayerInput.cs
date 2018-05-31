using System;
using System.Collections;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    /// <summary>
    /// Parameters: Delta
    /// </summary>
    public Action<Vector3> InputEvent;

    [SerializeField] private PlayerInputType vrPlayerInputType = PlayerInputType.Look;
    [SerializeField] private PlayerInputType nonVRPlayerInputType = PlayerInputType.Drag;

    private PlayerInputType currentPlayerInputType;
    private Coroutine inputCoroutine;

    private void LookInput(Vector3 _lookPosition, Vector3 _delta)
    {
        if(InputEvent != null)
        {
            InputEvent(_delta);
        }
    }

    private void TiltInput(Vector3 _lookPosition, Vector3 _delta)
    {
        Vector3 _tiltInput = new Vector3(Input.acceleration.x, _delta.y, _delta.z);

        if (InputEvent != null)
        {
            InputEvent(_tiltInput);
        }
    }

    private IEnumerator DragInput()
    {
        while (true)
        {
            yield return null;
        }
    }

    private void UpdateInput()
    {
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
        StopCurrent();

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
                inputCoroutine = StartCoroutine(DragInput());
                break;
        }

        currentPlayerInputType = _playerInputType;
    }

    private void StopCurrent()
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
                if (inputCoroutine != null)
                {
                    StopCoroutine(inputCoroutine);
                }
                break;
        }

        if (inputCoroutine != null)
        {
            StopCoroutine(inputCoroutine);
        }
    }

    private void Awake()
    {
        UpdateInput();
    }

    private void OnEnable()
    {
        VRSwitch.SwitchedEvent += UpdateInput;
    }

    private void OnDisable()
    {
        VRSwitch.SwitchedEvent -= UpdateInput;
    }
}
