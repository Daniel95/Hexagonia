using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class PlayerTrail : MonoBehaviour
{

    [SerializeField] private int length;
    [SerializeField] private float speed;

    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = length;

        for (int i = 0; i < length; i++)
        {
            float zPosition = transform.position.z - (i * speed);
            lineRenderer.SetPosition(i, new Vector3(0, 0, zPosition));
        }
    }
	
	private void Update()
    {
        lineRenderer.SetPosition(0, transform.position);

        for (int i = length - 1; i > 0; i--)
        {
            Vector3 _previousPosition = lineRenderer.GetPosition(i - 1);

            Vector3 _position = new Vector3(_previousPosition.x, _previousPosition.y, transform.position.z - (i * speed));
            lineRenderer.SetPosition(i, _position);
        }
	}

}
