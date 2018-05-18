using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class VRModeButtonListener : GazeButton
{
    private readonly Color defaultColor = Color.white;
    private readonly Color pressedColor = Color.yellow;

    [SerializeField] private GameObject eventSystemGameobject;

    protected override void OnGazeFilled()
    {
        OnClick();
    }

    public void OnClick()
    {
        if (VRSwitch.Instance.Switch())
        {
            buttonImage.color = pressedColor;
            eventSystemGameobject.SetActive(false);
        }
        else if (!VRSwitch.Instance.Switch())
        {
            buttonImage.color = defaultColor;
            eventSystemGameobject.SetActive(true);
        }
    }

    
}
