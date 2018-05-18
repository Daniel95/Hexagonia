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
    [SerializeField] [Range(0, 30)] private float animateSensitivity = 3;

    private int rightStateIndex = Animator.StringToHash("right");
    private int leftStateIndex = Animator.StringToHash("left");
    private int upStateIndex = Animator.StringToHash("up");
    private int downStateIndex = Animator.StringToHash("down");

    private void Animate(Vector3 _targetPosition)
    {
        Vector2 _delta = _targetPosition - transform.position;
        Vector2 _ratio = VectorHelper.Divide(_delta, (Vector2)LookPositionOnPlane.Instance.Size) * animateSensitivity;

        if(Mathf.Abs(_ratio.x) > Mathf.Abs(_ratio.y)) {
            if (_ratio.x > 0) {
                animator.Play(rightStateIndex, 0, _ratio.x);
            } else {
                animator.Play(leftStateIndex, 0, _ratio.x * -1);
            }
        } else {
            if (_ratio.y > 0) {
                animator.Play(rightStateIndex, 0, _ratio.y);
            } else {
                animator.Play(leftStateIndex, 0, _ratio.y * -1);
            }
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
