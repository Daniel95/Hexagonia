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

    private static List<float> localZPositions = new List<float>();

    private LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = length;

        for (int i = 0; i < length; i++)
        {
            float zPosition = i * -speed;
            localZPositions.Add(zPosition);
        }
    }
	
	private void Update()
    {
        UpdateLineRenderer();
    }
    
    private void UpdateLineRenderer()
    {
        lineRenderer.SetPosition(0, transform.position);

        for (int i = length - 1; i > 0; i--)
        {
            Vector3 _previousPosition = lineRenderer.GetPosition(i - 1);

            float zPosition = transform.position.z + localZPositions[i];
            Vector3 _position = new Vector3(_previousPosition.x, _previousPosition.y, zPosition);
            lineRenderer.SetPosition(i, _position);
        }
    }
}