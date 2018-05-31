using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DyingPlayer : MonoBehaviour
{
    public static Action AnimationEnd;

    [SerializeField] private Animator animator;

    private int deathStateIndex = Animator.StringToHash("death");

    private void Awake()
    {
        StartCoroutine(Die(animator.GetCurrentAnimatorClipInfo(0)[0].clip.length));
    }

    private IEnumerator Die(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (AnimationEnd != null)
        {
            AnimationEnd();
        }
        Destroy(gameObject);
    }
}
