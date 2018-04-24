using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] [Range(0, 1)] private float speed = 0.3f;

    [SerializeField] private Animator animator;
    [SerializeField] [Range(0, 1)] private float animateThreshold = 0.05f;
    [SerializeField] private string xAnimatorParameter = "X";
    [SerializeField] private string yAnimatorParameter = "Y";

    private void UpdateTargetPosition(Vector3 _targetPosition)
    {
        if (transform.position == _targetPosition)
            return;

        Vector3 _delta = _targetPosition - transform.position;
        Vector3 _direction = _delta.normalized;
        float _distance = Vector3.Distance(_targetPosition, transform.position);

        Animate(_delta);

        transform.position += _direction * (_distance * speed);
    }

    private void OnEnable()
    {
        PlaneMovement.MovePointOnPlaneEvent += UpdateTargetPosition;
    }

    private void OnDisable()
    {
        PlaneMovement.MovePointOnPlaneEvent -= UpdateTargetPosition;
    }

    private void Animate(Vector2 _delta)
    {
        Vector2 _ratio = VectorHelper.Divide(_delta, (Vector2)PlaneMovement.Instance.Size);

        int x = 0; 
        if (Mathf.Abs(_ratio.x) > animateThreshold)
        {
            x = RoundingHelper.InvertOnNegativeCeil(_ratio.x);
        }

        int y = 0;
        if(Mathf.Abs(_ratio.y) > animateThreshold)
        {
           y = RoundingHelper.InvertOnNegativeCeil(_ratio.y);
        }

        Vector2Int _roundedDirection = VectorHelper.Round(_ratio);

        animator.SetInteger(xAnimatorParameter, x);
        animator.SetInteger(yAnimatorParameter, y);
    }

}