using UnityEngine;

public class UpdatePositionToPlayerPosition : MonoBehaviour {

    [SerializeField] private Vector3 offset;

    private void UpdatePosition(Vector3 _position) {
        transform.position = _position + offset;
    }

    private void OnEnable() {
        PlayerSmoothPlaneMovement.OnMoved += UpdatePosition;    
    }

    private void OnDisable() {
        PlayerSmoothPlaneMovement.OnMoved -= UpdatePosition;
    }

}
