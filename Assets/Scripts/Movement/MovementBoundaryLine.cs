using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class MovementBoundaryLine : MonoBehaviour {

    public enum PositionType
    {
        Top,
        Bottom,
    }

    [SerializeField] private PositionType positionType;
    [SerializeField] [Range(0, 1)] private float threshold = 0.4f;
    [SerializeField] private float yOffset = 0.2f;
    [SerializeField] private float maximumOpacity = 0.75f;

    private float movementPlaneBottomYPosition;
    private float movementPlaneTopPosition;
    private float ratioPositionTypeMultiplier;
    private SpriteRenderer spriteRenderer;

    private void Update()
    {
        if (Player.Instance == null) { return; }

        float _topBottomOffset = movementPlaneTopPosition - movementPlaneBottomYPosition;

        float _playerPlaneLocalYPosition = Player.Instance.transform.position.y - movementPlaneBottomYPosition;
        float _ratioInPlane = _playerPlaneLocalYPosition / _topBottomOffset;
        float _ratioToPositionType = Mathf.Abs(_ratioInPlane - ratioPositionTypeMultiplier);

        float _currentWithThreshold = Mathf.Clamp01(_ratioToPositionType - threshold);
        float _maxWithThreshold = 1f - threshold;
        float _ratioWithThreshold = _currentWithThreshold / _maxWithThreshold;

        Color _color = spriteRenderer.material.color;
        _color.a = _ratioWithThreshold * maximumOpacity;
        spriteRenderer.material.color = _color;
    }

    private void Start()
    {
        Vector3 halfPlaneSize = LookPositionOnPlane.Instance.Size / 2;

        if (positionType == PositionType.Top)
        {
            transform.position = new Vector3(transform.position.x, LookPositionOnPlane.Instance.transform.position.y + halfPlaneSize.y + yOffset, transform.position.z);
        }
        else if(positionType == PositionType.Bottom)
        {
            transform.position = new Vector3(transform.position.x, LookPositionOnPlane.Instance.transform.position.y - halfPlaneSize.y + yOffset, transform.position.z);
            ratioPositionTypeMultiplier = 1;
        }

        movementPlaneBottomYPosition = LookPositionOnPlane.Instance.transform.position.y - halfPlaneSize.y;
        movementPlaneTopPosition = LookPositionOnPlane.Instance.transform.position.y + halfPlaneSize.y;

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

}