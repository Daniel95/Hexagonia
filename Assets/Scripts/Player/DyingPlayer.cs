using System;
using System.Collections;
using UnityEngine;

public class DyingPlayer : MonoBehaviour
{
    public static Action AnimationEndEvent;

    [SerializeField] private Animator animator;

    private void Awake()
    {
        StartCoroutine(Die(animator.GetCurrentAnimatorClipInfo(0)[0].clip.length));
    }

    private IEnumerator Die(float _delay)
    {
        yield return new WaitForSeconds(_delay);
        if (AnimationEndEvent != null)
        {
            AnimationEndEvent();
        }
        Destroy(gameObject);
    }
}
