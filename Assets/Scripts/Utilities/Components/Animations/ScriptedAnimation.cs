using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ScriptedAnimationController))]
public abstract class ScriptedAnimation : MonoBehaviour {

    public ScriptedAnimationType Type { get { return type; } }

    [SerializeField] private ScriptedAnimationType type;

    public bool IsAnimating { get { return AnimationCoroutine != null; } }

    protected Coroutine AnimationCoroutine;

    private Action animationStoppedEvent;

    public virtual void StartAnimation(Action animationStoppedEvent = null) {
        if (IsAnimating) {
            StopCoroutine(AnimationCoroutine);
            StopAnimation(false);
        }

        this.animationStoppedEvent = animationStoppedEvent;

        AnimationCoroutine = StartCoroutine(Animate());
    }

    public virtual void StopAnimation(bool _isCompleted)
    {
        if(AnimationCoroutine != null)
        {
            StopCoroutine(AnimationCoroutine);
            AnimationCoroutine = null;
        }

        if (animationStoppedEvent != null)
        {
            animationStoppedEvent();
        }

        animationStoppedEvent = null;
    }

    protected abstract IEnumerator Animate();

    private void OnDestroy()
    {
        StopAnimation(false);
    }

}