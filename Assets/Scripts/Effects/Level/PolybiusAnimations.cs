using UnityEngine;

/// <summary>
/// Controls the polybuisAnimation that is triggered by PlayerCollisions.DiedEvent.
/// </summary>
[RequireComponent(typeof(Animator))]
public class PolybiusAnimations : MonoBehaviour
{
    private const string POLYBIUS_DEAD = "Polybius dead";

    private int rightStateIndex = Animator.StringToHash(POLYBIUS_DEAD);
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