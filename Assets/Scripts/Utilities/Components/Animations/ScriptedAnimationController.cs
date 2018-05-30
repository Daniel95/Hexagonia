using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScriptedAnimationController : MonoBehaviour {

    public static bool AreAnimating { get; private set; }

    private static List<ScriptedAnimationController> scriptedAnimationControllers = new List<ScriptedAnimationController>();

    public bool IsAnimating { get { return scriptedAnimations.Exists(x => x.IsAnimating); } }

    private List<ScriptedAnimation> scriptedAnimations = new List<ScriptedAnimation>();

    public void StartAnimation(ScriptedAnimationType scriptedAnimationType, Action onAnimationCompleted = null)
    {
        ScriptedAnimation scriptedAnimation = scriptedAnimations.Find(x => x.Type == scriptedAnimationType);
        if(scriptedAnimation == null)
        {
            if(onAnimationCompleted != null)
            {
                onAnimationCompleted();
            }
            return;
        }

        onAnimationCompleted += UpdateAreAnimating;

        scriptedAnimation.StartAnimation(() => {
            if (onAnimationCompleted != null)
            {
                onAnimationCompleted();
            }
        });

        UpdateAreAnimating();
    }

    public void CancelAnimation(ScriptedAnimationType scriptedAnimationType)
    {
        List<ScriptedAnimation> targetScriptedAnimations = scriptedAnimations.FindAll(x => x.Type == scriptedAnimationType);
        foreach (ScriptedAnimation scriptedAnimation in targetScriptedAnimations)
        {
            scriptedAnimation.StopAnimation(false);
        }

        UpdateAreAnimating();
    }

    public bool CheckAnimationActive(ScriptedAnimationType scriptedAnimationType)
    {
        List<ScriptedAnimation> targetScriptedAnimations = scriptedAnimations.FindAll(x => x.Type == scriptedAnimationType);
        bool animationOfTypeActive = targetScriptedAnimations.Exists(x => x.IsAnimating);
        return animationOfTypeActive;
    }

    private ScriptedAnimation GetLevelNodeAnimation(ScriptedAnimationType scriptedAnimationType) {
        ScriptedAnimation scriptedAnimation = scriptedAnimations.Find(x => x.Type == scriptedAnimationType);
        return scriptedAnimation;
    }

    private void UpdateAreAnimating() {
        AreAnimating = scriptedAnimationControllers.Exists(x => x.IsAnimating);
    }

    private void OnEnable() {
        scriptedAnimationControllers.Add(this);
    }

    private void OnDisable() {
        scriptedAnimationControllers.Remove(this);
    }

    private void Awake() {
        scriptedAnimations = GetComponents<ScriptedAnimation>().ToList();
    }

}