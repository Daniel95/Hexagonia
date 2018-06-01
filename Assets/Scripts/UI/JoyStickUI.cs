using UnityEngine;

public class JoyStickUI : MonoBehaviour
{
    [SerializeField] private Transform indicator;
    [SerializeField] private float distance = 0.8f;

    public void SetDragDirection(Vector2 direction)
    {
        indicator.position = new Vector3(transform.position.x + direction.x * distance, transform.position.y + direction.y * distance, transform.position.z - 1);
    }
}
