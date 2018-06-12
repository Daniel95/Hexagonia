using System.Collections;

/// <summary>
/// Contains the logic for look controls for the player.
/// </summary>
public class PlayerLookInput : PlayerBaseInput
{
    private bool hit;

    protected override IEnumerator InputUpdate()
    {
        while (true)
        {
            TargetPosition = LookPositionOnPlane.Instance.GetLookPosition(out hit);
            if (hit && TargetPositionUpdatedEvent != null)
            {
                TargetPositionUpdatedEvent(TargetPosition);
            }
            yield return null;
        }
    }
}