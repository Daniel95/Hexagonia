using System;
using UnityEngine;

public class PlayerDiedAnimation : MonoBehaviour
{
    public static Action CompletedEvent;

    [SerializeField] private Animator animator;

    private void Awake()
    {
        float _animationLength = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;

        CoroutineHelper.DelayTime(_animationLength, () => {
            if (CompletedEvent != null)
            {
                CompletedEvent();
            }
            Destroy(gameObject);
        });
    }
}
