using UnityEngine;
using System;

public class Gyroscope : MonoBehaviour
{
    [SerializeField] private float filter = 1;

    private float zDisplay = 0;

    Vector3 previousAcceleration = Vector3.one;

    protected void OnGUI()
    {
        GUIStyle _myStyle = new GUIStyle
        {
            fontSize = 100,
        };

        GUILayout.Label("Acceleration x: " + Input.acceleration.x, _myStyle);
        //GUILayout.Label("Acceleration y: " + Input.acceleration.y, myStyle);
        GUILayout.Label("Acceleration z: " + Input.acceleration.z, _myStyle);
        GUILayout.Label("z: " + zDisplay, _myStyle);
    }
    
    private void Update()
    {
        Vector3 _acceleration = Input.acceleration;

        _acceleration = Vector3.Lerp(previousAcceleration, _acceleration, filter * Time.deltaTime);

        previousAcceleration = _acceleration;

        _acceleration = _acceleration * 180;

        float _z = float.Parse(Math.Round(-_acceleration.z, 1).ToString());
        zDisplay = _z;
        Camera.main.transform.rotation = Quaternion.Euler(_z, /*acceleration.x * 18*/0, 0);
    }
}