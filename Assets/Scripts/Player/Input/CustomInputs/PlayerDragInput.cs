﻿using System.Collections;
using UnityEngine;

public class PlayerDragInput : PlayerBaseInput
{
    [SerializeField] private float dragSpeed = 1;
    [SerializeField] private JoyStickUI joyStickUI;

    private Vector2 startInputPosition;
    private Vector2 screenCenter;

    public override void Activate()
    {
        TargetPosition = LookPositionOnPlane.Instance.transform.position;

        screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);

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
            Vector2 _deltaFromStartTouchPosition = PlatformBaseInput.CurrentDownPosition - PlatformBaseInput.StartDownPosition;
            Vector3 _targetScreenPosition = screenCenter + _deltaFromStartTouchPosition;
            TargetPosition = Camera.main.ScreenToWorldPoint(new Vector3(_targetScreenPosition.x, _targetScreenPosition.y, LookPositionOnPlane.Instance.transform.position.z));

            TargetPosition = LookPositionOnPlane.Instance.ClampToPlane(TargetPosition);

            if (TargetPositionUpdatedEvent != null)
            {
                TargetPositionUpdatedEvent(TargetPosition);
            }

            yield return null;
        }
    }
}
