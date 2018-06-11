using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using System;

public class WaitForVideoPrepared : MonoBehaviour
{
    public static Action StartIntroEvent;

    [SerializeField] private VideoPlayer player;

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

        if (StartIntroEvent != null)
        {
            StartIntroEvent();
        }
    }
}
