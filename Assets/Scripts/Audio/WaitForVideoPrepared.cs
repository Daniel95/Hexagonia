using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class WaitForVideoPrepared : MonoBehaviour
{
    [SerializeField] private VideoPlayer player;
    [SerializeField] private AudioSource source;

    void Awake ()
    {
        StartCoroutine(WaitForVideoRead());
	}

    IEnumerator WaitForVideoRead()
    {
        while (!player.isPrepared)
        {
            yield return null;
        }

        player.Play();
        source.Play();
    }
}
