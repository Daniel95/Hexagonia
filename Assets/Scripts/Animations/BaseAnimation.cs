using System;
using UnityEngine;

public class BaseAnimation : MonoBehaviour {

    [SerializeField] [Range(0, 1)] protected float Speed;

    private Action AnimationCompleted;

    public void StartAnimation(Action _animationCompleted)
    {
        //_animationCompleted = 
    }

    public void StopAnimation()
    {

    }

}
