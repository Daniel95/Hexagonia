using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class VRModeButton : GazeButton
{
    public static Action InitializedEvent;

    private readonly Color defaultColor = Color.white;
    private readonly Color pressedColor = Color.yellow;

    private EventSystem[] eventSystems;

    private void Start()
    {
        eventSystems = Resources.FindObjectsOfTypeAll<EventSystem>();
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
            for (int i = 0; i < eventSystems.Length; i++)
            {
                if (eventSystems[i].transform.name == "EventSystem")
                {
                    eventSystems[i].gameObject.SetActive(false);
                }
            }
        }
        else if (!VRSwitch.Instance.Switch())
        {
            buttonImage.color = defaultColor;
            for (int i = 0; i < eventSystems.Length; i++)
            {
                if (eventSystems[i].transform.name == "EventSystem")
                {
                    eventSystems[i].gameObject.SetActive(true);
                }
            }
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
