using System;
using UnityEngine;

public class ScaleTransformAnimation : LerpAnimation
{
    public override void StartAnimation(Action animationStoppedEvent = null)
    {
        StartValue = transform.localScale.x;
        base.StartAnimation(animationStoppedEvent);
    }

    public override void StopAnimation(bool isCompleted)
    {
        transform.localScale = Vector3.one;
        base.StopAnimation(isCompleted);
    }

    protected override void Apply(float _value)
    {
        transform.localScale = new Vector3(_value, _value, _value);
    }
}