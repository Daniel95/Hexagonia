using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// This animation can be used to lerp certain values by overriding this class.
/// </summary>
public abstract class LerpAnimation : ScriptedAnimation
{
    [SerializeField] [Range(0, 1)] protected float TargetValue = 1;
    [SerializeField] protected float Speed = 1;

    protected float StartValue = 1;
    protected float CurrentValue;

    protected abstract void Apply(float _value);

    public override void StartAnimation(Action _animationStoppedEvent = null)
    {
        base.StartAnimation(_animationStoppedEvent);
    }

    protected override IEnumerator Animate()
    {
        float _progress = 0;
        CurrentValue = StartValue;
        while (CurrentValue != TargetValue)
        {
            _progress += Speed * Time.deltaTime;
            CurrentValue = Mathf.Lerp(StartValue, TargetValue, _progress);
            Apply(CurrentValue);
            yield return null;
        }

        Apply(TargetValue);
        StartValue = TargetValue;

        StopAnimation(true);
    }
}