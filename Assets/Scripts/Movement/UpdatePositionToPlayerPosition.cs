using UnityEngine;

/// <summary>
/// Used to move objects to the player position without stuttering in the movement.	
/// </summary>
public class UpdatePositionToPlayerPosition //: MonoBehaviour
{
    [SerializeField] private Vector3 offset;

    private void UpdatePosition(Vector3 _playerPosition, Vector3 _playerDelta)
    {
        transform.position = _playerPosition + offset;
    }

    private void OnEnable()
    {
        PlayerMovement.OnMoved += UpdatePosition;    
    }

    private void OnDisable()
    {
        PlayerMovement.OnMoved -= UpdatePosition;
    }
}