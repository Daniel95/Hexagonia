using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Action PlayerDiedEvent;

    public static Player Instance { get { return GetInstance(); } }

    #region Singleton
    private static Player instance;

    private static Player GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<Player>();
        }
        return instance;
    }
    #endregion

    [SerializeField] private Animator animator;
    [SerializeField] [Range(0, 30)] private float animateXSensitivity = 6;
    [SerializeField] [Range(0, 30)] private float animateYSensitivity = 8;
    [SerializeField] [Range(0, 1)] private float turnAnimateThreshold = 0.05f;

    private int middleStateIndex = Animator.StringToHash("middle");
    private int rightStateIndex = Animator.StringToHash("right");
    private int leftStateIndex = Animator.StringToHash("left");
    private int upStateIndex = Animator.StringToHash("up");
    private int downStateIndex = Animator.StringToHash("down");
    private bool playingMiddleState;

    private void Animate(Vector3 _targetPosition)
    {
        Vector2 _delta = _targetPosition - transform.position;
        Vector2 _ratio = VectorHelper.Multiply(VectorHelper.Divide(_delta, (Vector2)LookPositionOnPlane.Instance.Size), new Vector2(animateXSensitivity, animateYSensitivity));

        float absRatioX = Mathf.Abs(_ratio.x);
        float absRatioY = Mathf.Abs(_ratio.y);
        if (absRatioX > turnAnimateThreshold && absRatioX > absRatioY)
        {
            playingMiddleState = false;
            if (_ratio.x > 0)
            {
                animator.Play(rightStateIndex, 0, _ratio.x);
            }
            else
            {
                animator.Play(leftStateIndex, 0, _ratio.x * -1);
            }
        }
        else if (absRatioY > turnAnimateThreshold)
        {
            playingMiddleState = false;
            if (_ratio.y > 0)
            {
                animator.Play(upStateIndex, 0, _ratio.y);
            }
            else
            {
                animator.Play(downStateIndex, 0, _ratio.y * -1);
            }
        }
        else if (!playingMiddleState)
        {
            playingMiddleState = true;
            animator.Play(middleStateIndex);
        }
    }

    private void OnEnable()
    {
        LookPositionOnPlane.LookPositionUpdatedEvent += Animate;
    }

    private void OnDisable()
    {
        LookPositionOnPlane.LookPositionUpdatedEvent -= Animate;
    }

    private void OnTriggerEnter(Collider _otherCollider)
    {
        if (_otherCollider.tag == Tags.Obstacle)
        {
            LookPositionOnPlane.Instance.enabled = false;

            if (PlayerDiedEvent != null)
            {
                PlayerDiedEvent();
            }
            Destroy(gameObject);
        }
    }

}
