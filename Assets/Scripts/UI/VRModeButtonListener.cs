using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRModeButtonListener : MonoBehaviour
{
    private Color defaultColor = Color.white;
    private Color pressedColor = Color.yellow;

    [SerializeField] private GameObject eventSystemGameobject;

    [SerializeField] private Image buttonImage;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ToggleVRMode()
    {
        if (VRSwitch.Instance.Switch())
        {
            buttonImage.color = pressedColor;
            eventSystemGameobject.SetActive(false);


        }
        else if(!VRSwitch.Instance.Switch())
        {
            buttonImage.color = defaultColor;
            eventSystemGameobject.SetActive(true);


        }

    }
}
