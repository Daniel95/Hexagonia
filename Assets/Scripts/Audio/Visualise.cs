/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visualise : MonoBehaviour
{
    public VisualiseEditor.Options Option
    {
        set
        {
            option = value;
        }
    }
    public Light LightSource
    {
        set
        {
            lightSource = value;
        }
        get
        {
            return lightSource;
        }
    }

    private VisualiseEditor.Options option;
    private Light lightSource;
    
    private void Update()
    {
        switch (option)
        {
            case VisualiseEditor.Options.Light:
                lightSource.intensity = Random.Range(0, 100);
                break;
            case VisualiseEditor.Options.Transform:
                break;
        }
    }
}
*/