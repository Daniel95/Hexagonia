using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Input pc handles mobile input logic and the input events of InputBase
/// </summary>
public class PCInput : PlatformBaseInput
{
    private bool DownOnUI { get { return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(); } }

    [SerializeField] private KeyCode input = KeyCode.Mouse0;

    protected override IEnumerator InputUpdate()
    {
        Vector2 lastInputPosition = new Vector2();
        float startDownTime = 0;
        Vector2 inputPosition = new Vector2();
        bool startedTouching;

        while (true)
        {
            startedTouching = !DownOnUI && Input.GetKeyDown(input);

            inputPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);

            if (startedTouching)
            {
                TouchState = TouchStates.TouchDown;
                StartDownPosition = lastInputPosition = CurrentDownPosition = inputPosition;
                startDownTime = Time.time;

                if (DownInputEvent != null)
                {
                    DownInputEvent(CurrentDownPosition);
                }

                if(InputEvent != null)
                {
                    InputEvent(CurrentDownPosition);
                }
            }

            if (TouchState != TouchStates.None)
            {
                Down = !Input.GetKeyUp(input);
                if (Down)
                {
                    CurrentDownPosition = inputPosition;

                    if (InputEvent != null)
                    {
                        InputEvent(CurrentDownPosition);
                    }

                    if (TouchState == TouchStates.TouchDown)
                    { 
                        if (Time.time - startDownTime > TimeBeforeTappedExpired)
                        {
                            if (TappedExpiredInputEvent != null)
                            {
                                TappedExpiredInputEvent();
                            }
                        }
                    }

                    float distance = Vector2.Distance(CurrentDownPosition, StartDownPosition);
                    if (distance >= DragTreshhold)
                    {
                        TouchState = TouchStates.Dragging;
                        Vector2 delta = CurrentDownPosition - lastInputPosition;
                        if (DraggingInputEvent != null)
                        {
                            DraggingInputEvent(CurrentDownPosition, delta);
                        }
                    }
                    else 
                    {
                        if (TouchState == TouchStates.Dragging)
                        {
                            if (CancelDragInputEvent != null)
                            {
                                CancelDragInputEvent();
                            }
                        }
                        else 
                        {
                            TouchState = TouchStates.Holding;

                            if (HoldingInputEvent != null)
                            {
                                HoldingInputEvent(CurrentDownPosition);
                            }
                        }
                    }
                }
                else
                {
                    if (UpInputEvent != null)
                    {
                        UpInputEvent(inputPosition);
                    }

                    if (TouchState == TouchStates.TouchDown)
                    {
                        if (TapInputEvent != null)
                        {
                            TapInputEvent(inputPosition);
                        }
                    }
                    else if (TouchState != TouchStates.Holding)
                    {
                        Vector2 direction = ((Vector2)inputPosition - lastInputPosition).normalized;

                        if (ReleaseInDirectionInputEvent != null)
                        {
                            ReleaseInDirectionInputEvent(direction);
                        }
                    }

                    TouchState = TouchStates.None;
                }
            }

            lastInputPosition = CurrentDownPosition;

            yield return null;
        }
    }

    private void UpdateInput()
    {
        bool _enable = !PlatformHelper.PlatformIsMobile && !VRSwitch.VRState;
        EnableInput(_enable);
    }

    private void OnEnable()
    {
        SceneLoader.FadeSceneInCompletedEvent += UpdateInput;
        VRSwitch.SwitchedEvent += UpdateInput;
    }

    private void OnDisable()
    {
        SceneLoader.FadeSceneInCompletedEvent -= UpdateInput;
        VRSwitch.SwitchedEvent -= UpdateInput;
    }
}