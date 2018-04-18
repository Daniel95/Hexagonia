using UnityEngine;

public class FrequencyCubes : MonoBehaviour
{
    [SerializeField] private GameObject cubePrefab;
    private GameObject[] cubes = new GameObject[8];

    [Space(5)]

    [SerializeField] private bool useBuffer = true;

    [Space(5)]

    [SerializeField] private float scaleMultiplier = 100;
    [SerializeField] private float startScale = 2;
    [SerializeField] private float xZScale = 1;
    [SerializeField] private float offset = 1;

    private void Start()
    {
        for (int i = 0; i < cubes.Length; i++)
        {
            cubes[i] = Instantiate(cubePrefab, transform);
            cubes[i].transform.localPosition = new Vector3(-cubes.Length / 2 + (offset * i), 0, 0);
        }
    }

    private void Awake()
    {
        AudioPeer.Instance.TransmitAudioData += UpdateCubes;
    }

    private void UpdateCubes (AudioPeer _audioPeer)
    {
        for (int i = 0; i < cubes.Length; i++)
        {
            if (useBuffer)
            {
                cubes[i].transform.localScale = new Vector3(xZScale, _audioPeer.BandBuffer[i] * scaleMultiplier + startScale, xZScale);
            }
            else
            {
                cubes[i].transform.localScale = new Vector3(xZScale, _audioPeer.FreqBand[i] * scaleMultiplier + startScale, xZScale);
            }
        }
	}
}
