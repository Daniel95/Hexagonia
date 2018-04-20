using UnityEngine;
using System.Collections;
using System;

public class CoroutineHelper : MonoBehaviour
{

    private static CoroutineHelper instance;

    public static Coroutine Start(IEnumerator routine)
    {
        return GetInstance().StartLocalCoroutine(routine);
    }

    public static void Stop(Coroutine coroutine)
    {
        GetInstance().StopLocalCoroutine(coroutine);
    }

    public static void Delay(int frames, Action onDelayed)
    {
        GetInstance().StartLocalCoroutine(GetInstance().DelayOverTime(onDelayed, frames));
    }

    public Coroutine StartLocalCoroutine(IEnumerator routine)
    {
        return StartCoroutine(routine);
    }

    public void StopLocalCoroutine(Coroutine routine)
    {
        StopCoroutine(routine);
    }

    private static CoroutineHelper GetInstance()
    {
        if (instance == null)
        {
            GameObject go = new GameObject("CoroutineHelper");
            instance = go.AddComponent<CoroutineHelper>();
        }
        return instance;
    }

    private IEnumerator DelayOverTime(Action onDelayed, int frames)
    {
        for (int i = 0; i < frames; i++)
        {
            yield return null;
        }

        if (onDelayed != null)
        {
            onDelayed();
        }
    }

}