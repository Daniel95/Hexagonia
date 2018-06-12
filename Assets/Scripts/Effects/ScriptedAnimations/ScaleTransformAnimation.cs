using System;
using UnityEngine;

/// <summary>
/// Scales an transform.
/// </summary>
public class ScaleTransformAnimation : LerpAnimation
{
    public override void StartAnimation(Action _animationStoppedEvent = null)
    {
        StartValue = transform.localScale.x;
        base.StartAnimation(_animationStoppedEvent);
    }

    public override void StopAnimation(bool _isCompleted)
    {
        transform.localScale = Vector3.one;
        base.StopAnimation(_isCompleted);
    }

    protected override void Apply(float _value)
    {
        transform.localScale = new Vector3(_value, _value, _value);
    }
}