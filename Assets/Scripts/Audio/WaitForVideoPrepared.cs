using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class WaitForVideoPrepared : MonoBehaviour
{
    [SerializeField] private VideoPlayer player;
    [SerializeField] private AudioSource source;

    private void PlaySound()
    {
        StartCoroutine(WaitForVideoRead());
    }

    private IEnumerator WaitForVideoRead()
    {
        while (!player.isPrepared)
        {
            yield return null;
        }

        player.Play();
        source.Play();
    }

    private void OnEnable()
    {
        CameraHolderIntro.OnCompletedMobileIntoCardboardEvent += PlaySound;
    }

    private void OnDisable()
    {
        CameraHolderIntro.OnCompletedMobileIntoCardboardEvent -= PlaySound;
    }
}
