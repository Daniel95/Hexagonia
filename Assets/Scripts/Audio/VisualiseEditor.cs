#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(Visualise))]
public class VisualiseEditor : Editor
{
    private int index;
    
    private Visualise visualise;

    private void Awake()
    {
        visualise = (Visualise)target;
    }

    public enum Options
    {
       Light = 0,
       Transform = 1,
    }

    public override void OnInspectorGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("What to visualise", GUILayout.Width(150));
        index = EditorGUILayout.Popup(index, Enum.GetNames(typeof(Options)));
        GUILayout.EndHorizontal();

        switch (index)
        {
            case (int)Options.Light:
                visualise.Option = Options.Light;
                visualise.LightSource = (Light)EditorGUILayout.ObjectField(visualise.LightSource, typeof(Light), true);
                break;
            case (int)Options.Transform:
                visualise.Option = Options.Transform;
                break;
        }
    }

}
#endif
