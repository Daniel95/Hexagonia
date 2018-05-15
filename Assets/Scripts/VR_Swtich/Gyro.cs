using UnityEngine;
using UnityEngine.XR;

public class Gyro : MonoBehaviour
{
    private Quaternion baseGyro;

    [Range(0,5)]
    public float YSensitivity = 1;

    [Range(0, 5)]
    public float XSensitivity = 1;

    void Start()
    {
        Input.gyro.enabled = true;
        baseGyro = Input.gyro.attitude;
    }

    private void OnGUI()
    {
        GUIStyle _myStyle = new GUIStyle
        {
            fontSize = 100,
        };
        /*
        GUILayout.Label("gyro: " + Input.gyro, _myStyle);
        GUILayout.Label("gyro x: " + Input.gyro.attitude.x, _myStyle);
        GUILayout.Label("gyro y: " + Input.gyro.attitude.y, _myStyle);
        GUILayout.Label("gyro z: " + Input.gyro.attitude.z, _myStyle);
        */
        GUILayout.Label("base x: " + baseGyro.eulerAngles.x, _myStyle);
        GUILayout.Label("base y: " + baseGyro.eulerAngles.y, _myStyle);
        GUILayout.Label("base z: " + baseGyro.eulerAngles.z, _myStyle);
    }

    protected void Update()
    {
        if(baseGyro.y == 0)
            baseGyro = GyroToUnity(Input.gyro.attitude);

        if (!XRSettings.enabled)
        {
            GyroCamera();
            LockRotation();
            AdjustForBaseY();
        }
    }

    void GyroCamera()
    {
        Quaternion _rotation = GyroToUnity(Input.gyro.attitude);

        transform.rotation = _rotation;
        transform.rotation = transform.rotation * Quaternion.Euler(90, baseGyro.eulerAngles.y, 0);
    }

    private void LockRotation()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
    }

    private void AdjustForBaseY()
    {
        //transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + baseGyro.eulerAngles.y, transform.rotation.eulerAngles.z);
    }

    private Quaternion GyroToUnity(Quaternion q)
    {
        return new Quaternion(q.x * YSensitivity, q.y * XSensitivity, q.z, -q.w);
    }
}