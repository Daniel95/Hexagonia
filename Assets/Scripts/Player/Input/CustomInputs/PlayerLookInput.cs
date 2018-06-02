using System.Collections;
using UnityEngine;

public class PlayerLookInput : PlayerInputBase
{
    private Coroutine lookPositionUpdateCoroutine;

    public override void Activate()
    {
        lookPositionUpdateCoroutine = StartCoroutine(LookPositionUpdate());
    }

    public override void Deactivate()
    {
        StopCoroutine(lookPositionUpdateCoroutine);
    }

    private IEnumerator LookPositionUpdate()
    {
        bool _hit;

        while (true)
        {
            TargetPosition = LookPositionOnPlane.Instance.GetLookPosition(out _hit);
            if (_hit && TargetPositionUpdatedEvent != null)
            {
                TargetPositionUpdatedEvent(TargetPosition);
            }
            yield return null;
        }
    }
}
