using System.Collections;
using UnityEngine;

public class PlayerDragInput : PlayerBaseInput
{
    [SerializeField] private float dragSpeed = 1;
    [SerializeField] private JoyStickUI joyStickUI;

    public override void Activate()
    {
        PlatformBaseInput.UpInputEvent += OnUpInput;

        bool _hit;
        Vector3 _center = LookPositionOnPlane.Instance.GetLookPosition(out _hit);
        if (_hit)
        {
            TargetPosition = _center;
            if (TargetPositionUpdatedEvent != null)
            {
                TargetPositionUpdatedEvent(TargetPosition);
            }
        }

        joyStickUI.Activate();
        base.Activate();
    }

    public override void Deactivate()
    {
        PlatformBaseInput.UpInputEvent -= OnUpInput;
        joyStickUI.Deactivate();
        base.Deactivate();
    }

    protected override IEnumerator InputUpdate()
    {
        while(true)
        {
            bool _hit;
            Vector3 _center = LookPositionOnPlane.Instance.GetLookPosition(out _hit);
            if (!_hit) { yield return null; }

            Vector3 _startDownWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(PlatformBaseInput.StartDownPosition.x, PlatformBaseInput.StartDownPosition.y, _center.z));
            Vector3 _currentDownPositionWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(PlatformBaseInput.CurrentDownPosition.x, PlatformBaseInput.CurrentDownPosition.y, _center.z));
            Vector3 _deltaFromStartTouchPosition = _currentDownPositionWorldPosition - _startDownWorldPosition;

            Vector3 _deltaWithSpeed = _deltaFromStartTouchPosition * dragSpeed;

            TargetPosition = Player.Instance.transform.position + _deltaWithSpeed;
            TargetPosition = LookPositionOnPlane.Instance.ClampToPlane(TargetPosition);

            DebugHelper.SetDebugPosition(TargetPosition, "TargetPosition");
            if (TargetPositionUpdatedEvent != null)
            {
                TargetPositionUpdatedEvent(TargetPosition);
            }

            yield return null;
        }
    }

    private void OnUpInput(Vector2 _inputPosition)
    {
        bool _hit;
        Vector3 _center = LookPositionOnPlane.Instance.GetLookPosition(out _hit);
        if (!_hit) { return; }
        TargetPosition = _center;
    }
}
