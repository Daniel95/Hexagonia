using System.Collections;
using UnityEngine;

public class PlayerDragInput : PlayerBaseInput
{
    [SerializeField] private float dragSpeed = 1;
    [SerializeField] private JoyStickUI joyStickUI;

    public override void Activate()
    {
        TargetPosition = Player.Instance.transform.position;
        if (TargetPositionUpdatedEvent != null)
        {
            TargetPositionUpdatedEvent(TargetPosition);
        }

        joyStickUI.Activate();
        base.Activate();
    }

    public override void Deactivate()
    {
        joyStickUI.Deactivate();
        base.Deactivate();
    }

    protected override IEnumerator InputUpdate()
    {
        while(true)
        {
            if (PlatformBaseInput.Down) {
                Vector3 _playerPosition = Player.Instance.transform.position;

                Vector3 _startDownWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(PlatformBaseInput.StartDownPosition.x, PlatformBaseInput.StartDownPosition.y, _playerPosition.z));
                Vector3 _currentDownPositionWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(PlatformBaseInput.CurrentDownPosition.x, PlatformBaseInput.CurrentDownPosition.y, _playerPosition.z));
                Vector3 _deltaFromStartTouchPosition = _currentDownPositionWorldPosition - _startDownWorldPosition;

                Vector3 _deltaWithSpeed = _deltaFromStartTouchPosition * dragSpeed;

                TargetPosition = _playerPosition + _deltaWithSpeed;
                TargetPosition = LookPositionOnPlane.Instance.ClampToPlane(TargetPosition);
            }

            if (TargetPositionUpdatedEvent != null)
            {
                TargetPositionUpdatedEvent(TargetPosition);
            }

            yield return null;
        }
    }
}
