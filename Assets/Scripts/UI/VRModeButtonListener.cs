using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class VRModeButtonListener : GazeButton
{
    public static Action InitializedEvent;

    private readonly Color defaultColor = Color.white;
    private readonly Color pressedColor = Color.yellow;

    [SerializeField] private GameObject eventSystemGameobject;

    private void Start()
    {
        if (InitializedEvent != null)
        {
            InitializedEvent();
        }
    }

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

    protected override void OnEnable()
    {
        base.OnEnable();
        VRSwitch.VRModeSwitchedEvent += OnClick;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        VRSwitch.VRModeSwitchedEvent -= OnClick;
    }
    
}
