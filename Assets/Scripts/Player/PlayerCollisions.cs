using System;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    public static Action DiedEvent;
    public static Action<GameObject> CollisionEvent;

    [SerializeField] private GameObject dyingPlayer;
    [SerializeField] private LayerMask collisionLayermask;

    private int lastHitFrame;
    private bool hitObstacleInFront;
    private Collider lastObstacleInFrontCollider;
    private float obstaclePreviousZPosition;

    private void Update()
    {
        if (hitObstacleInFront && lastObstacleInFrontCollider != null)
        {
            Vector3 _obstacleColliderMin = lastObstacleInFrontCollider.bounds.min;
            Vector3 _obstacleColliderMax = lastObstacleInFrontCollider.bounds.max;
            _obstacleColliderMax.z = obstaclePreviousZPosition;

            bool _isInPreviousCollider = transform.position.IsInBetween(_obstacleColliderMin, _obstacleColliderMax);

            if (_isInPreviousCollider)
            {
                Hit(lastObstacleInFrontCollider.gameObject);
            }

            lastObstacleInFrontCollider = null;
        }

        RaycastHit _obstacleHit;
        hitObstacleInFront = Physics.Raycast(transform.position, transform.forward, out _obstacleHit, 3, collisionLayermask);

        if (hitObstacleInFront)
        {
            obstaclePreviousZPosition = _obstacleHit.point.z;
            lastObstacleInFrontCollider = _obstacleHit.collider;
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
