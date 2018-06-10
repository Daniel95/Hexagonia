using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Base input class which contains all input events that the game can listen to.
/// </summary>
public abstract class PlatformBaseInput : MonoBehaviour
{
    public static Vector2 StartDownPosition { get; protected set; }
    public static Vector2 CurrentDownPosition { get; protected set; }
    public static bool Down { get; protected set; }

    public enum TouchStates { Holding, Dragging, TouchDown, None }

    /// <summary>
    /// When the user pressed down.
    /// </summary>
    public static Action<Vector2> DownInputEvent;
    /// <summary>
    /// When the user pressed down.
    /// </summary>
    public static Action<Vector2> InputEvent;
    /// <summary>
    /// When the user pressed up.
    /// </summary>
    public static Action<Vector2> UpInputEvent;
    /// <summary>
    /// When the user pressed up within a certain amout of time and without moving too much.
    /// </summary>
    public static Action<Vector2> TapInputEvent;
    /// <summary>
    /// When the user pressed too long for a tap.
    /// </summary>
    public static Action TappedExpiredInputEvent;
    /// <summary>
    /// When the user drags.
    /// parameters: screenPosition and delta
    /// </summary>
    public static Action<Vector2, Vector2> DraggingInputEvent;
    /// <summary>
    /// When the user stops with dragging.
    /// </summary>
    public static Action CancelDragInputEvent;
    /// <summary>
    /// When the user continuesly presses down without moving.
    /// </summary>
    public static Action<Vector2> HoldingInputEvent;
    /// <summary>
    /// When the user pressed up after dragging.
    /// </summary>
    public static Action<Vector2> ReleaseInDirectionInputEvent;
    /// <summary>
    /// When the user stopped with their input.
    /// </summary>
    public static Action CanceledInputEvent;

    /// <summary>
    /// The amount of distance the user has to move before it is considered a drag.
    /// </summary>
    [SerializeField] protected float DragTreshhold = 0.1f;
    /// <summary>
    /// The time it takes to go from a tap to holding.
    /// </summary>
    [SerializeField] protected float TimeBeforeTappedExpired = 0.15f;

    /// <summary>
    /// The TouchState the input is in.
    /// </summary>
    protected TouchStates TouchState = TouchStates.None;
    /// <summary>
    /// The input update loop.
    /// </summary>
    protected Coroutine InputUpdateCoroutine;

    /// <summary>
    /// Enables or disables the input loop.
    /// </summary>
    /// <param name="enable"></param>
    public void EnableInput(bool enable)
    {
        if (enable)
        {
            if (InputUpdateCoroutine == null)
            {
                StartInputUpdate();
            }
        }
        else
        {
            if (InputUpdateCoroutine != null)
            {
                StopCoroutine(InputUpdateCoroutine);
                InputUpdateCoroutine = null;
            }
        }
    }

    protected abstract IEnumerator InputUpdate();

    protected virtual void StartInputUpdate() {
        InputUpdateCoroutine = StartCoroutine(InputUpdate());
    }
}