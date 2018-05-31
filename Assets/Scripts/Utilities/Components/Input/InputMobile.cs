using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Input mobile handles mobile input logic and the input events of InputBase
/// </summary>
public class InputMobile : InputBase
{
    Vector2 lastTouchPosition = new Vector2();
    Vector2 startTouchPosition = new Vector2();
    float touchDownTime = 0;

    private void Update()
    {
        if(!PlatformHelper.PlatformIsMobile) { return; }

        bool startedTouching = Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
        if (startedTouching)
        {
            TouchState = TouchStates.TouchDown;
            startTouchPosition = lastTouchPosition = Input.GetTouch(0).position;
            touchDownTime = Time.time;

            if (DownInputEvent != null)
            {
                DownInputEvent(startTouchPosition);
            }
        }

        if (TouchState != TouchStates.None)
        {
            bool isTouching = Input.touchCount > 0 && Input.GetTouch(0).phase != TouchPhase.Ended && Input.GetTouch(0).phase != TouchPhase.Canceled;
            if (isTouching)
            {
                Vector2 currentTouchPosition = Input.GetTouch(0).position;

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

                float distance = Vector2.Distance(currentTouchPosition, startTouchPosition);
                if (distance > DragTreshhold)
                {
                    TouchState = TouchStates.Dragging;

                    Vector2 delta = currentTouchPosition - lastTouchPosition;

                    if (DraggingInputEvent != null)
                    {
                        DraggingInputEvent(currentTouchPosition, delta);
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
                            HoldingInputEvent(Input.GetTouch(0).position);
                        }
                    }
                }

                lastTouchPosition = currentTouchPosition;
            }
            else
            {
                if (UpInputEvent != null)
                {
                    UpInputEvent(Input.GetTouch(0).position);
                }

                if (TouchState == TouchStates.TouchDown)
                {
                    if (TapInputEvent != null)
                    {
                        TapInputEvent(Input.GetTouch(0).position);
                    }
                }
                else if (TouchState == TouchStates.Dragging)
                {
                    Vector2 direction = (Input.GetTouch(0).position - startTouchPosition).normalized;
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
        }
    }
}