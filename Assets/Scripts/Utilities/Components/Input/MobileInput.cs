using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Input mobile handles mobile input logic and the input events of InputBase
/// </summary>
public class MobileInput : PlatformBaseInput
{
    private bool DownOnUI { get { return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId); } }

    protected override IEnumerator InputUpdate() {
        Vector2 lastTouchPosition = new Vector2();
        float touchDownTime = 0;
        Vector2 inputPosition = new Vector2();
        bool startedTouching;
        while (true)
        {
            if(Input.touchCount != 0)
            {
                startedTouching = !DownOnUI && Input.GetTouch(0).phase == TouchPhase.Began;
                inputPosition = Camera.main.ScreenToViewportPoint(Input.GetTouch(0).position);
            }
            else
            {
                startedTouching = false;
            }

            if (startedTouching)
            {
                TouchState = TouchStates.TouchDown;
                StartDownPosition = lastTouchPosition = CurrentDownPosition = inputPosition;
                touchDownTime = Time.time;

                if (DownInputEvent != null)
                {
                    DownInputEvent(CurrentDownPosition);
                }

                if (InputEvent != null)
                {
                    InputEvent(CurrentDownPosition);
                }
            }

            if (TouchState != TouchStates.None)
            {
                Down = Input.touchCount > 0 && Input.GetTouch(0).phase != TouchPhase.Ended && Input.GetTouch(0).phase != TouchPhase.Canceled;
                if (Down)
                {
                    CurrentDownPosition = inputPosition;
                    if (InputEvent != null)
                    {
                        InputEvent(CurrentDownPosition);
                    }

                    if (TouchState == TouchStates.TouchDown)
                    {
                        float timePassedSinceTouchDown = Time.time - touchDownTime;
                        if (timePassedSinceTouchDown > TimeBeforeTappedExpired)
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

                        Vector2 delta = CurrentDownPosition - lastTouchPosition;

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
                    else if (TouchState == TouchStates.Dragging)
                    {
                        Vector2 direction = (inputPosition - StartDownPosition).normalized;
                        if (ReleaseInDirectionInputEvent != null)
                        {
                            ReleaseInDirectionInputEvent(direction);
                        }
                    }

                    if (Input.GetTouch(0).phase == TouchPhase.Canceled)
                    {
                        if (CanceledInputEvent != null)
                        {
                            CanceledInputEvent();
                        }
                    }

                    TouchState = TouchStates.None;
                }

                lastTouchPosition = CurrentDownPosition;
            }

            yield return null; 
        }
    }

    private void UpdateInput()
    {
        bool _enable = PlatformHelper.PlatformIsMobile && !VRSwitch.VRState;
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