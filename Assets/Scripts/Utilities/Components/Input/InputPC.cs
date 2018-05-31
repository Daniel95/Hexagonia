using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Input pc handles mobile input logic and the input events of InputBase
/// </summary>
public class InputPC : InputBase
{
    [SerializeField] private KeyCode input = KeyCode.Mouse0;

    private float startDownTime;
    private Coroutine inputUpdateCoroutine;

    /// <summary>
    /// Start the pc input loop.
    /// </summary>
    protected override void StartInputUpdate()
    {
        base.StartInputUpdate();
        InputUpdateCoroutine = StartCoroutine(InputUpdate());
    }

    private IEnumerator InputUpdate()
    {
        Vector2 lastInputPosition = new Vector2();
        Vector2 mouseStartPosition = new Vector2();

        while (true)
        {
            bool startedTouching = Input.GetKeyDown(input) && !EventSystem.current.IsPointerOverGameObject();

            if (startedTouching)
            {
                TouchState = TouchStates.TouchDown;
                mouseStartPosition = lastInputPosition = Input.mousePosition;
                startDownTime = Time.time;

                if (DownInputEvent != null)
                {
                    DownInputEvent(mouseStartPosition);
                }
            }

            if (TouchState != TouchStates.None)
            {
                if (!Input.GetKeyUp(input))
                {
                    Vector2 currentMousePosition = Input.mousePosition;

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

                    float distance = Vector2.Distance(currentMousePosition, mouseStartPosition);
                    if (distance > DragTreshhold)
                    {
                        TouchState = TouchStates.Dragging;
                        Vector2 delta = currentMousePosition - lastInputPosition;
                        if (DraggingInputEvent != null)
                        {
                            DraggingInputEvent(currentMousePosition, delta);
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
                                HoldingInputEvent(currentMousePosition);
                            }
                        }
                    }

                    lastInputPosition = currentMousePosition;
                }
                else
                {
                    if (UpInputEvent != null)
                    {
                        UpInputEvent(Input.mousePosition);
                    }

                    if (TouchState == TouchStates.TouchDown)
                    {
                        if (TapInputEvent != null)
                        {
                            TapInputEvent(Input.mousePosition);
                        }
                    }
                    else if (TouchState != TouchStates.Holding)
                    {
                        Vector2 direction = ((Vector2)Input.mousePosition - lastInputPosition).normalized;

                        if (ReleaseInDirectionInputEvent != null)
                        {
                            ReleaseInDirectionInputEvent(direction);
                        }
                    }

                    TouchState = TouchStates.None;
                }
            }

            yield return null;
        }
    }

    private void Awake()
    {
        if (!PlatformHelper.PlatformIsMobile)
        {
            EnableInput(true);
        }
    }
}