using UnityEngine;

/// <summary>
/// Displays the current song's audio spectrum average as scaled cubes
/// </summary>
public class SpectrumCubes : MonoBehaviour
{
    [SerializeField] GameObject cubePrefab;
    [SerializeField] float audioScale = 100;
    [SerializeField] float defaultScale = 2;

	private float circleDegrees = 360f;
    private GameObject[] sampleCubes = new GameObject[512];
    
	private void Awake ()
    {
        InstantiateCubes();
    }

    private void InstantiateCubes()
    {
        for (int i = 0; i < sampleCubes.Length; i++)
        {
            float _angle = circleDegrees / sampleCubes.Length;

            sampleCubes[i] = Instantiate(cubePrefab, transform.position, Quaternion.identity, transform);
            sampleCubes[i].name = "SampleCube " + i;

            transform.eulerAngles = new Vector3(0, _angle * i, 0);
            sampleCubes[i].transform.position = Vector3.forward * 100;
        }
    }
   
    private void UpdateSampleScale(AudioPeer _audioPeer)
    {
        for (int i = 0; i < sampleCubes.Length; i++)
        {
            if (sampleCubes[i] != null)
            {
                sampleCubes[i].transform.localScale = new Vector3(1, (_audioPeer.Samples[i] * audioScale) + defaultScale, 1);
            }
        }
    }

    private void OnEnable()
    {
        AudioPeer.TransmitAudioData += UpdateSampleScale;
    }
    private void DisEnable()
    {
        AudioPeer.TransmitAudioData -= UpdateSampleScale;
    }
}