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

    private void UpdateColliderTransforms()
    {
        topLeftToBottomRightCollider.transform.position = (PlayerSpriteDetailPositions.Instance.TopLeftWingPosition + PlayerSpriteDetailPositions.Instance.BottomRightWingPosition) / 2f;
        topLeftToBottomRightCollider.transform.rotation = Quaternion.LookRotation(PlayerSpriteDetailPositions.Instance.TopLeftWingPosition - PlayerSpriteDetailPositions.Instance.BottomRightWingPosition, Vector3.forward);

        topRightToBottomLeftCollider.transform.position = (PlayerSpriteDetailPositions.Instance.TopRightWingPosition + PlayerSpriteDetailPositions.Instance.BottomLeftWingPosition) / 2f;
        topRightToBottomLeftCollider.transform.rotation = Quaternion.LookRotation(PlayerSpriteDetailPositions.Instance.TopRightWingPosition - PlayerSpriteDetailPositions.Instance.BottomLeftWingPosition, Vector3.forward);
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

    private void Update()
    {
        UpdateRaycastCollisions();
    }

    private void OnEnable()
    {
        PlayerSpriteDetailPositions.PositionsUpdatedEvent += UpdateColliderTransforms;
    }

    private void OnDisable()
    {
        PlayerSpriteDetailPositions.PositionsUpdatedEvent -= UpdateColliderTransforms;
    }
}
