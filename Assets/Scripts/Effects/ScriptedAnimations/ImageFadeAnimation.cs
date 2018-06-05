using System;
using UnityEngine;
using UnityEngine.UI;

public class ImageFadeAnimation : LerpAnimation
{
    [SerializeField] private Image image;

    public override void StartAnimation(Action animationStoppedEvent = null)
    {
        StartValue = image.color.a;
        base.StartAnimation(animationStoppedEvent);
    }

    public override void StopAnimation(bool isCompleted)
    {
        base.StopAnimation(isCompleted);
    }

    protected override void Apply(float _value)
    {
        Color tempColor = image.color;
        tempColor.a = _value;
        image.color = tempColor;
    }
}
