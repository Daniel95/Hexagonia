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
        UpdateRaycastCollisions();
    }

    private void UpdateColliders()
    {
        topLeftToBottomRightCollider.transform.position = (PlayerTrails.Instance.TopLeftPosition + PlayerTrails.Instance.BottomRightPosition) / 2f;
        topLeftToBottomRightCollider.transform.rotation = Quaternion.LookRotation(PlayerTrails.Instance.TopLeftPosition - PlayerTrails.Instance.BottomRightPosition, Vector3.forward);

        topRightToBottomLeftCollider.transform.position = (PlayerTrails.Instance.TopRightWingPosition + PlayerTrails.Instance.BottomLeftPosition) / 2f;
        topRightToBottomLeftCollider.transform.rotation = Quaternion.LookRotation(PlayerTrails.Instance.TopRightWingPosition - PlayerTrails.Instance.BottomLeftPosition, Vector3.forward);
    }

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
