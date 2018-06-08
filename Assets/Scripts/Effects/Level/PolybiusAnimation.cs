using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PolybiusAnimation : MonoBehaviour
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
        Player.DiedEvent += OnDiedEvent;
    }

    private void OnDisable()
    {
        Player.DiedEvent -= OnDiedEvent;
    }
}