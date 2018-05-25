using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScriptAnimator : MonoBehaviour {

    public static bool AreAnimating { get; private set; }

    private static List<ScriptAnimator> scriptAnimators = new List<ScriptAnimator>();

    public bool IsAnimating { get { return scriptAnimations.Exists(x => x.IsAnimating); } }

    private List<ScriptAnimation> scriptAnimations = new List<ScriptAnimation>();

    private bool inAnimationIsCompleted;
    private bool outAnimationIsCompleted;

    public bool CheckAnimationActive(AnimationType animationType)
    {
        List<ScriptAnimation> levelNodeAnimationsOfType = scriptAnimations.FindAll(x => x.AnimationType == animationType);
        bool animationOfTypeActive = levelNodeAnimationsOfType.Exists(x => x.IsAnimating);
        return animationOfTypeActive;
    }

    public void CancelAnimation(AnimationType animationType)
    {
        List<ScriptAnimation> levelNodeAnimationsOfType = scriptAnimations.FindAll(x => x.AnimationType == animationType);
        foreach (ScriptAnimation scriptAnimation in levelNodeAnimationsOfType)
        {
            scriptAnimation.StopAnimation(false);
        }

        UpdateAreAnimating();
    }

    public void StartAnimation(AnimationType animationType, Action onAnimationCompleted = null)
    {
        ScriptAnimation animation = scriptAnimations.Find(x => x.AnimationType == animationType);
        if(animation == null)
        {
            if(onAnimationCompleted != null)
            {
                onAnimationCompleted();
            }
            return;
        }

        AreAnimating = true;

        animation.StartAnimation(() => {
            if (onAnimationCompleted != null)
            {
                onAnimationCompleted();
            }
            UpdateAreAnimating();
        });
    }

    private ScriptAnimation GetLevelNodeAnimation(AnimationType animationType) {
        ScriptAnimation levelNodeAnimation = scriptAnimations.Find(x => x.AnimationType == animationType);
        return levelNodeAnimation;
    }

    private void UpdateAreAnimating() {
        AreAnimating = scriptAnimators.Exists(x => x.IsAnimating);
    }

    private void OnEnable() {
        scriptAnimators.Add(this);
    }

    private void OnDisable() {
        scriptAnimators.Remove(this);
    }

    private void Awake() {
        scriptAnimations = GetComponents<ScriptAnimation>().ToList();
    }

}