using System;
using UnityEngine;

/// <summary>
/// Controls the animations, collisions and contains several events related to the player.
/// </summary>
public class Player : MonoBehaviour
{
    public static Action DiedEvent;
    public static Action<GameObject> TriggerCollisionEvent;
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

	public Vector2 Ratio { get { return ratio; } }

    [SerializeField] private GameObject dyingPlayer;
    [SerializeField] private Animator animator;
    [SerializeField] [Range(0, 30)] private float animateSensitivity = 3;
    [SerializeField] [Range(0, 1)] private float turnAnimateThreshold = 0.05f;

    private int middleStateIndex = Animator.StringToHash("middle");
    private int rightStateIndex = Animator.StringToHash("right");
    private int leftStateIndex = Animator.StringToHash("left");
    private int upStateIndex = Animator.StringToHash("up");
    private int downStateIndex = Animator.StringToHash("down");
    private bool playingMiddleState;
    private bool hitThisframe;
	private Vector2 delta;
	private Vector2 ratio;
    private float absRatioX;
    private float absRatioY;
    private Vector3 previousTargetPosition;

    private void Animate(Vector3 _targetPosition)
    {
        delta = (_targetPosition - previousTargetPosition) / Time.deltaTime;
        ratio = VectorHelper.Divide(delta, (Vector2)LookPositionOnPlane.Instance.Size) * animateSensitivity;

        absRatioX = Mathf.Abs(ratio.x);
        absRatioY = Mathf.Abs(ratio.y);
        if (absRatioX > turnAnimateThreshold && absRatioX > absRatioY)
        {
            playingMiddleState = false;
            if (ratio.x > 0)
            {
                animator.Play(rightStateIndex, 0, ratio.x);
            }
            else
            {
                animator.Play(leftStateIndex, 0, ratio.x * -1);
            }
        }
        else if (absRatioY > turnAnimateThreshold)
        {
            playingMiddleState = false;
            if (ratio.y > 0)
            {
                animator.Play(upStateIndex, 0, ratio.y);
            }
            else
            {
                animator.Play(downStateIndex, 0, ratio.y * -1);
            }
        }
        else if (!playingMiddleState)
        {
            playingMiddleState = true;
            animator.Play(middleStateIndex);
        }

        previousTargetPosition = _targetPosition;
    }

    private void OnEnable()
    {
        PlayerInputController.InputEvent += Animate;
    }

    private void OnDisable()
    {
        PlayerInputController.InputEvent -= Animate;
    }

    private void OnTriggerEnter(Collider _otherCollider)
    {
        if (hitThisframe) { return; }
        hitThisframe = true;
        CoroutineHelper.DelayFrames(1, () => { hitThisframe = false; });

        if (TriggerCollisionEvent != null)
        {
            TriggerCollisionEvent(_otherCollider.gameObject);
        }

        if (_otherCollider.tag == Tags.Obstacle)
        {
            LookPositionOnPlane.Instance.enabled = false;
            SpawnDyingPlayer();
            if (DiedEvent != null)
            {
                DiedEvent();
            }
            Destroy(gameObject);
        }
    }

    private void SpawnDyingPlayer()
    {
        Instantiate(dyingPlayer, transform.position, transform.rotation);
    }
}