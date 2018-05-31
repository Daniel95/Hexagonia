using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// Updates the camera rotation to the Gyroscope of this device in Non VR mode.
/// </summary>
public class Gyro : MonoBehaviour
{
    [Range(0, 5)] [SerializeField] private float ySensitivity = 1;
    [Range(0, 5)] [SerializeField] private float xSensitivity = 1;

    private Quaternion baseGyro;

    private void Start()
    {
        Initialization(null, Scenes.Main);
    }

    private void Initialization(Scenes? _scenes1, Scenes _scenes2)
    {
        Input.gyro.enabled = true;
        baseGyro = Input.gyro.attitude;
    }

    private void Update()
    {
        if (baseGyro.y == 0)
        {
            baseGyro = GyroToUnity(Input.gyro.attitude);
        }

        if (!VRSwitch.VRState)
        {
            GyroCamera();
            LockRotation();
        }
    }

    private void GyroCamera()
    {
        Quaternion _rotation = GyroToUnity(Input.gyro.attitude);
        transform.rotation = _rotation;
        transform.rotation = transform.rotation * Quaternion.Euler(90, baseGyro.eulerAngles.y, 0);
    }

    private void LockRotation()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
    }

    private Quaternion GyroToUnity(Quaternion q)
    {
        return new Quaternion(q.x * ySensitivity, q.y * xSensitivity, q.z, -q.w);
    }

    private void OnEnable()
    {
        SceneLoader.SceneSwitchStartedEvent += Initialization;
    }

    private void OnDisable()
    {
        SceneLoader.SceneSwitchStartedEvent -= Initialization;
    }
}