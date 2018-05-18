using UnityEngine;
using UnityEngine.SceneManagement;
using System;

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
    [SerializeField] private new Animation animation;
    [SerializeField] [Range(0, 1)] private float animateThreshold = 0.05f;
    [SerializeField] private string xAnimatorParameter = "X";
    [SerializeField] private string yAnimatorParameter = "Y";

    private AnimationState right;
    private AnimationState left;
    private AnimationState up;
    private AnimationState down;

    private void Animate(Vector3 _targetPosition)
    {
        Vector2 _delta = _targetPosition - transform.position;
        Vector2 _ratio = VectorHelper.Divide(_delta, (Vector2)LookPositionOnPlane.Instance.Size);

        if(Mathf.Abs(_ratio.x) > Mathf.Abs(_ratio.y)) {
            int _x = RoundingHelper.InvertOnNegativeCeil(_ratio.x);
            animator.SetInteger(xAnimatorParameter, _x);

            if (_ratio.x > 0) {
                float time = _ratio.x * right.length;
                right.time = time;
            } else {
                float time = (_ratio.x * -1) * left.length;
                right.time = time;
            }
        } else {
            int _y = RoundingHelper.InvertOnNegativeCeil(_ratio.y);
            animator.SetInteger(yAnimatorParameter, _y);

            if (_ratio.y > 0) {
                float time = _ratio.y * up.length;
                right.time = time;
            } else {
                float time = (_ratio.y * -1) * down.length;
                right.time = time;
            }
        }
    }

    /*
    private void Animate(Vector3 _targetPosition)
    {
        Vector2 _delta = _targetPosition - transform.position;
        Vector2 _ratio = VectorHelper.Divide(_delta, (Vector2)LookPositionOnPlane.Instance.Size);

        int _x = 0;
        if (Mathf.Abs(_ratio.x) > animateThreshold)
        {
            _x = RoundingHelper.InvertOnNegativeCeil(_ratio.x);
        }

        int _y = 0;
        if (Mathf.Abs(_ratio.y) > animateThreshold)
        {
            _y = RoundingHelper.InvertOnNegativeCeil(_ratio.y);
        }

        animator.SetInteger(xAnimatorParameter, _x);
        animator.SetInteger(yAnimatorParameter, _y);
    }
    */

    private void Awake() {
        right = animation["right"];
        left = animation["left"];
        up = animation["up"];
        down = animation["down"];
    }

    private void OnEnable()
    {
        Debug.Log(right.length);
        Debug.Log(left.length);
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
            Destroy(gameObject);
			if(PlayerDiedEvent != null)
			{
				PlayerDiedEvent();
			}
			CoroutineHelper.Delay(60, () =>
            {
                SceneManager.LoadScene(0);
            });
        }
    }

}
