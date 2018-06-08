using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Trail used by the player, because the player does not move on it's Z axis, the trail needs to mimic this movement.
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class PlayerTrail : MonoBehaviour
{
    [SerializeField] private int length = 30;
    [SerializeField] private float speed = 0.15f;
    [SerializeField] [Range(0, 1)] private float followSpeed = 0.7f;

    private List<float> localZPositions = new List<float>();
    private LineRenderer lineRenderer;
    private Vector3 trailPosition;

    private void UpdateLineRenderer(Vector3 _playerPosition, Vector3 _playerDelta)
    {
        for (int i = length - 1; i > 0; i--)
        {
            Vector3 _previousPosition = lineRenderer.GetPosition(i - 1);
            float zPosition = transform.position.z + localZPositions[i];
            Vector3 _position = new Vector3(_previousPosition.x, _previousPosition.y, zPosition);
            lineRenderer.SetPosition(i, _position);
        }

        float _distance = Vector3.Distance(trailPosition, transform.position);
        Vector3 _offset = transform.position - trailPosition;
        Vector3 _direction = _offset.normalized;
        Vector3 _translation = _direction * (_distance * followSpeed);

        trailPosition += _translation;

        lineRenderer.SetPosition(0, trailPosition);
    }

    private void Awake()
    {
        trailPosition = transform.position;

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = length;

        for (int i = 0; i < length; i++)
        {
            float zPosition = i * -speed;
            localZPositions.Add(zPosition);
        }
    }
	
    private void OnEnable()
    {
        PlayerMovement.OnMoved += UpdateLineRenderer;       
    }

    private void OnDisable()
    {
        PlayerMovement.OnMoved -= UpdateLineRenderer;
    }
}