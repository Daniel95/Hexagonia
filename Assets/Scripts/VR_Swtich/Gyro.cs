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
        Initialization();
    }

    private void Initialization()
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

        if (!XRSettings.enabled)
        {
            GyroCamera();
            LockRotation();
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

    private Quaternion GyroToUnity(Quaternion q)
    {
        return new Quaternion(q.x * YSensitivity, q.y * XSensitivity, q.z, -q.w);
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