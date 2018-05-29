using UnityEngine;

/// <summary>
/// Displays the current song's audio frequency as 512 scaled cubes.
/// </summary>
public class FrequencyCubes : MonoBehaviour
{
	[SerializeField] private GameObject cubePrefab;
	[SerializeField] private bool useBuffer = true;
    [SerializeField] private float scaleMultiplier = 100;
    [SerializeField] private float startScale = 2;
    [SerializeField] private float xZScale = 1;
    [SerializeField] private float offset = 1;

	private GameObject[] cubes = new GameObject[8];

	private void Start()
    {
        InstantiateCubes();
    }

    private void InstantiateCubes()
    {
        for (int i = 0; i < cubes.Length; i++)
        {
            cubes[i] = Instantiate(cubePrefab, transform);
            cubes[i].transform.localPosition = new Vector3(-cubes.Length / 2 + (offset * i), 0, 0);
            cubes[i].name = "SampleCube " + i;
        }
    }

    private void UpdateCubes()
    {
        for (int i = 0; i < cubes.Length; i++)
        {
            if (useBuffer)
            {
                cubes[i].transform.localScale = new Vector3(xZScale, AudioPeer.BandBuffer[i] * scaleMultiplier + startScale, xZScale);
            }
            else
            {
                cubes[i].transform.localScale = new Vector3(xZScale, AudioPeer.FreqBand[i] * scaleMultiplier + startScale, xZScale);
            }
        }
	}
    
    private void OnEnable()
    {
        AudioPeer.TransmitAudioData += UpdateCubes;
    }

    private void OnDisable()
    {
        AudioPeer.TransmitAudioData -= UpdateCubes;
    }
}