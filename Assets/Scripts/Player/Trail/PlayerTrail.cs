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

    private List<float> localZPositions = new List<float>();
    private LineRenderer lineRenderer;

    private void UpdateLineRenderer(Vector3 _playerPosition)
    {
        for (int i = length - 1; i > 0; i--)
        {
            Vector3 _previousPosition = lineRenderer.GetPosition(i - 1);
            float zPosition = transform.position.z + localZPositions[i];
            Vector3 _position = new Vector3(_previousPosition.x, _previousPosition.y, zPosition);
            lineRenderer.SetPosition(i, _position);
        }

        lineRenderer.SetPosition(0, transform.position);
    }

    private void Awake()
    {
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
        PlayerSmoothPlaneMovement.OnMoved += UpdateLineRenderer;       
    }

    private void OnDisable()
    {
        PlayerSmoothPlaneMovement.OnMoved -= UpdateLineRenderer;
    }
}