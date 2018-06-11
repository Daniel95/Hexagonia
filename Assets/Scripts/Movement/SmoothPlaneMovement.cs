using UnityEngine;

/// <summary>
/// Moves the gameobject smootly along towards the targetPosition. Target Position is updated every frame by the PlayerInputController.
/// </summary>
public class SmoothPlaneMovement : MonoBehaviour
{
    [SerializeField] [Range(0, 20)] private float vrSpeed = 10f;
    [SerializeField] [Range(0, 20)] private float nonVRSpeed = 10f;
    [SerializeField] private Vector3 offset;
    
    protected Vector3 Delta;

    private float currentSpeed;

    protected virtual void MoveToTargetPosition(Vector3 _targetPosition)
    {
        Vector3 _targetPositionWithOffset = _targetPosition + offset;
        Delta = _targetPositionWithOffset - transform.position;
        Vector3 _direction = Delta.normalized;
        float _distance = Vector2.Distance(_targetPositionWithOffset, transform.position);
        float _deltaSpeed = currentSpeed * Time.deltaTime;

        Vector3 _positionIncrement = _direction * (_distance * _deltaSpeed);
        transform.position += _positionIncrement;
    }

    private void UpdateSpeedToVRSpeed()
    {
        if (VRSwitch.VRState)
        {
            currentSpeed = vrSpeed;
        }
        else
        {
            currentSpeed = nonVRSpeed;
        }
    }

    private void Awake()
    {
        UpdateSpeedToVRSpeed();
    }

    private void OnEnable()
    {
        PlayerInputController.InputEvent += MoveToTargetPosition;
        VRSwitch.SwitchedEvent += UpdateSpeedToVRSpeed;
    }

    private void OnDisable()
    {
        PlayerInputController.InputEvent -= MoveToTargetPosition;
        VRSwitch.SwitchedEvent += UpdateSpeedToVRSpeed;
    }
}