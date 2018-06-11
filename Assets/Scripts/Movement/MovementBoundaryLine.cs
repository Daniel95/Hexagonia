using System.Collections;
using UnityEngine;

/// <summary>
/// Shows the top and bottom boundary of the level, if the player gets close the boundary will fade in. Only visible if the player is near the boundary.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class MovementBoundaryLine : MonoBehaviour
{
    public enum PositionType
    {
        Top,
        Bottom
    }

    [SerializeField] private PositionType positionType;
    [SerializeField] private float yOffset = 0.2f;
    [SerializeField] [Range(0, 1)] private float fadeInThreshold = 0.4f;
    [SerializeField] [Range(0, 1)] private float minAlpha = 0;
    [SerializeField] [Range(0, 1)] private float maxAlpha = 0.75f;

    private float movementPlaneBottomYPosition;
    private float movementPlaneTopPosition;
    private float ratioPositionTypeMultiplier;
    private SpriteRenderer spriteRenderer;
    private Coroutine fadeCoroutine;

    private IEnumerator FadeCoroutine()
    {
        while(true)
        {
            float _topBottomOffset = movementPlaneTopPosition - movementPlaneBottomYPosition;

            float _playerPlaneLocalYPosition = PlayerMovement.Position.y - movementPlaneBottomYPosition;
            float _ratioInPlane = _playerPlaneLocalYPosition / _topBottomOffset;
            float _ratioToPositionType = Mathf.Abs(_ratioInPlane - ratioPositionTypeMultiplier);

            float _currentWithThreshold = Mathf.Clamp01(_ratioToPositionType - fadeInThreshold);
            float _maxWithThreshold = 1f - fadeInThreshold;
            float _ratioWithThreshold = _currentWithThreshold / _maxWithThreshold;

            float _minMaxRange = maxAlpha - minAlpha;
            float _alpha = minAlpha + (_ratioWithThreshold * _minMaxRange);

            Color _color = spriteRenderer.material.color;
            _color.a = _alpha;
            spriteRenderer.material.color = _color;

            yield return null;
        }
    }

    private void StopFadeCoroutine()
    {
        if(fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
    }

    private void Initiate()
    {
        Vector3 _halfPlaneSize = LookPositionOnPlane.Instance.Size / 2;

        if (positionType == PositionType.Top)
        {
            transform.position = new Vector3(transform.position.x, LookPositionOnPlane.Instance.transform.position.y + _halfPlaneSize.y + yOffset, transform.position.z);
        }
        else if(positionType == PositionType.Bottom)
        {
            transform.position = new Vector3(transform.position.x, LookPositionOnPlane.Instance.transform.position.y - _halfPlaneSize.y + yOffset, transform.position.z);
            ratioPositionTypeMultiplier = 1;
        }

        movementPlaneBottomYPosition = LookPositionOnPlane.Instance.transform.position.y - _halfPlaneSize.y;
        movementPlaneTopPosition = LookPositionOnPlane.Instance.transform.position.y + _halfPlaneSize.y;

        spriteRenderer = GetComponent<SpriteRenderer>();

        fadeCoroutine = StartCoroutine(FadeCoroutine());
    }

    private void OnEnable()
    {
        PlayerCollisions.DiedEvent += StopFadeCoroutine;
        LookPositionOnPlane.InitiatedEvent += Initiate;
    }

    private void OnDisable()
    {
        PlayerCollisions.DiedEvent -= StopFadeCoroutine;
        LookPositionOnPlane.InitiatedEvent -= Initiate;
    }
}