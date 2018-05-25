using System;
using System.Collections;
using UnityEngine;

public abstract class LerpAnimation : ScriptedAnimation
{

    protected float StartValue = 1;

    [SerializeField] private float targetValue = 1;
    [SerializeField] private float speed = 1;

    private float currentValue;

    protected abstract void Apply(float _value);

    public override void StartAnimation(Action _animationStoppedEvent = null)
    {
        base.StartAnimation(_animationStoppedEvent);
    }

    public override void StopAnimation(bool _isCompleted)
    {
        base.StopAnimation(_isCompleted);
    }

    protected override IEnumerator Animate()
    {
        float _progress = 0;
        while (currentValue < 0)
        {
            _progress += speed * Time.deltaTime;
            currentValue = Mathf.Lerp(StartValue, targetValue, _progress);
            Apply(currentValue);
            yield return null;
        }

        Apply(targetValue);
        StartValue = targetValue;

        StopAnimation(true);
    }

}
