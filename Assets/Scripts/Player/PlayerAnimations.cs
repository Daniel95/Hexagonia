using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    public static Vector2 Ratio { get { return ratio; } }

    private static Vector2 ratio;

    [SerializeField] private Animator animator;
    [SerializeField] [Range(0, 30)] private float animateSensitivity = 3;
    [SerializeField] [Range(0, 1)] private float turnAnimateThreshold = 0.05f;

    private int middleStateIndex = Animator.StringToHash("middle");
    private int rightStateIndex = Animator.StringToHash("right");
    private int leftStateIndex = Animator.StringToHash("left");
    private int upStateIndex = Animator.StringToHash("up");
    private int downStateIndex = Animator.StringToHash("down");
    private bool playingMiddleState;

    private void UpdateAnimation(Vector3 _playerPosition, Vector3 _playerDelta)
    {
        ratio = VectorHelper.Divide(_playerDelta, LookPositionOnPlane.Instance.Size) * animateSensitivity;

        float _absRatioX = Mathf.Abs(ratio.x);
        float _absRatioY = Mathf.Abs(ratio.y);
        if (_absRatioX > turnAnimateThreshold && _absRatioX > _absRatioY)
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
        else if (_absRatioY > turnAnimateThreshold)
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
    }

    private void OnEnable()
    {
        PlayerMovement.OnMoved += UpdateAnimation;
    }

    private void OnDisable()
    {
        PlayerMovement.OnMoved -= UpdateAnimation;
    }
}
