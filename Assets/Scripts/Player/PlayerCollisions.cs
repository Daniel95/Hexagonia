using System;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    public static Action DiedEvent;
    public static Action<GameObject> CollisionEvent;

    [SerializeField] private GameObject dyingPlayer;
    [SerializeField] private LayerMask collisionLayermask;
    [SerializeField] private Collider topLeftToBottomRightCollider;
    [SerializeField] private Collider topRightToBottomLeftCollider;

    private int lastHitFrame;
    private bool hitObjectInFront;
    private Collider lastObjectInFrontCollider;
    private float objectPreviousZPosition;

    private void Update()
    {
        UpdateColliders();
        //UpdateRaycastCollisions();
    }

    private void UpdateColliders()
    {
        //DebugHelper.SetDebugPosition(PlayerTrails.Instance.TopLeftPosition, "TopLeftPosition", false, Color.green);
        //DebugHelper.SetDebugPosition(PlayerTrails.Instance.TopRightWingPosition, "TopRightWingPosition", false, Color.blue);
        //DebugHelper.SetDebugPosition(PlayerTrails.Instance.BottomLeftPosition, "BottomLeftPosition", false, Color.yellow);
        //DebugHelper.SetDebugPosition(PlayerTrails.Instance.BottomRightPosition, "BottomRightPosition", false, Color.red);

        //float _topLeftToBottomRightAngle = Vector3.Angle(PlayerTrails.Instance.TopLeftPosition, PlayerTrails.Instance.BottomRightPosition);
        //float _topRightToBottomLeftAngle = Vector3.Angle(PlayerTrails.Instance.TopRightWingPosition, PlayerTrails.Instance.BottomLeftPosition);

        topLeftToBottomRightCollider.transform.position = (PlayerTrails.Instance.TopLeftPosition + PlayerTrails.Instance.BottomRightPosition) / 2f;
        topLeftToBottomRightCollider.transform.rotation = Quaternion.LookRotation(PlayerTrails.Instance.TopLeftPosition - PlayerTrails.Instance.BottomRightPosition, Vector3.forward);

        topRightToBottomLeftCollider.transform.position = (PlayerTrails.Instance.TopRightWingPosition + PlayerTrails.Instance.BottomLeftPosition) / 2f;
        topRightToBottomLeftCollider.transform.rotation = Quaternion.LookRotation(PlayerTrails.Instance.TopRightWingPosition - PlayerTrails.Instance.BottomLeftPosition, Vector3.forward);
    }

    //private void UpdateColliders()
    //{
    //    DebugHelper.SetDebugPosition(PlayerTrails.Instance.TopLeftPosition, "TopLeftPosition", false, Color.green);
    //    DebugHelper.SetDebugPosition(PlayerTrails.Instance.TopRightWingPosition, "TopRightWingPosition", false, Color.blue);
    //    DebugHelper.SetDebugPosition(PlayerTrails.Instance.BottomLeftPosition, "BottomLeftPosition", false, Color.yellow);
    //    DebugHelper.SetDebugPosition(PlayerTrails.Instance.BottomRightPosition, "BottomRightPosition", false, Color.red);

    //    float _topLeftToBottomRightAngle = Vector3.Angle(PlayerTrails.Instance.TopLeftPosition, PlayerTrails.Instance.BottomRightPosition);
    //    float _topRightToBottomLeftAngle = Vector3.Angle(PlayerTrails.Instance.TopRightWingPosition, PlayerTrails.Instance.BottomLeftPosition);

    //    Vector3 _topLeftToBottomRightColliderEulerAngles = topLeftToBottomRightCollider.transform.eulerAngles;
    //    Vector3 _topRightToBottomLeftColliderEulerAngles = topRightToBottomLeftCollider.transform.eulerAngles;

    //    topLeftToBottomRightCollider.transform.localRotation = Quaternion.Euler(new Vector3(_topLeftToBottomRightColliderEulerAngles.x, _topLeftToBottomRightColliderEulerAngles.y, _topLeftToBottomRightAngle));
    //    topRightToBottomLeftCollider.transform.localRotation = Quaternion.Euler(new Vector3(_topRightToBottomLeftColliderEulerAngles.x, _topRightToBottomLeftColliderEulerAngles.y, -_topRightToBottomLeftAngle));
    //}

    //private static void SetPos(Transform _transform, Vector3 _start, Vector3 _end)
    //{
    //    Vector3 dir = _end - _start;
    //    Vector3 mid = (dir) / 2f + _start;
    //    _transform.position = mid;
    //    _transform.rotation = Quaternion.FromToRotation(Vector3.up, dir);
    //    //transform.localScale.y = new Vector3(transform.localScale.x, dir.magnitude, transform.localScale.z);
    //}

    private void UpdateRaycastCollisions()
    {
        if (hitObjectInFront && lastObjectInFrontCollider != null)
        {
            Vector3 _obstacleColliderMin = lastObjectInFrontCollider.bounds.min;
            Vector3 _obstacleColliderMax = lastObjectInFrontCollider.bounds.max;
            _obstacleColliderMax.z = objectPreviousZPosition;

            bool _isInPreviousCollider = transform.position.IsInBetween(_obstacleColliderMin, _obstacleColliderMax);

            if (_isInPreviousCollider)
            {
                Hit(lastObjectInFrontCollider.gameObject);
            }

            lastObjectInFrontCollider = null;
        }

        RaycastHit _obstacleHit;
        hitObjectInFront = Physics.Raycast(transform.position, transform.forward, out _obstacleHit, 3, collisionLayermask);

        if (hitObjectInFront)
        {
            objectPreviousZPosition = _obstacleHit.point.z;
            lastObjectInFrontCollider = _obstacleHit.collider;
        }
    }

    private void OnTriggerEnter(Collider _otherCollider)
    {
        Hit(_otherCollider.gameObject);
    }

    private void Hit(GameObject _collidingGameObject)
    {
        if (Time.frameCount == lastHitFrame) { return; }
        lastHitFrame = Time.frameCount;

        if (CollisionEvent != null)
        {
            CollisionEvent(_collidingGameObject);
        }

        if (_collidingGameObject.CompareTag(Tags.Obstacle))
        {
            Die();
        }
    }

    private void Die()
    {
        SpawnDyingPlayer();
        if (DiedEvent != null)
        {
            DiedEvent();
        }
        Destroy(gameObject);
    }

    private void SpawnDyingPlayer()
    {
        Instantiate(dyingPlayer, transform.position, transform.rotation);
    }
}
