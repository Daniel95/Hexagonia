using UnityEngine;

/// <summary>
/// Controls the polybuisAnimation that is triggered PlayerCollisions.DiedEvent.
/// </summary>
[RequireComponent(typeof(Animator))]
public class PolybiusAnimations : MonoBehaviour
{
    private int rightStateIndex = Animator.StringToHash("Polybius dead");
    private Animator animator;

    private void OnDiedEvent()
    {
        animator.Play(rightStateIndex);
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        PlayerCollisions.DiedEvent += OnDiedEvent;
    }

    private void OnDisable()
    {
        PlayerCollisions.DiedEvent -= OnDiedEvent;
    }
}