using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ScriptedAnimationController))]
public abstract class ScriptedAnimation : MonoBehaviour {

    public static Action AnimationCompletedEvent;

    public ScriptedAnimationType Type { get { return type; } }

    [SerializeField] private ScriptedAnimationType type;

    public bool IsAnimating { get { return AnimationCoroutine != null; } }

    protected Coroutine AnimationCoroutine;

    private Action animationStoppedEvent;

    public void StartAnimation(Action animationStoppedEvent = null) {
        if (IsAnimating) {
            StopCoroutine(AnimationCoroutine);
            StopAnimation(false);
        }

        this.animationStoppedEvent = animationStoppedEvent;

        AnimationCoroutine = StartCoroutine(Animation());
    }

    public virtual void StopAnimation(bool isCompleted)
    {
        if (isCompleted)
        {
            if (AnimationCompletedEvent != null)
            {
                AnimationCompletedEvent();
            }
        }

        AnimationCoroutine = null;
        if (animationStoppedEvent != null)
        {
            animationStoppedEvent();
        }

        animationStoppedEvent = null;
    }

    protected abstract IEnumerator Animation();

    private void OnDestroy()
    {
        StopAnimation(false);
    }

}