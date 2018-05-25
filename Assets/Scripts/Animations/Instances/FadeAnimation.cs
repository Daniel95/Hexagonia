using System.Collections;
using UnityEngine;

public abstract class FadeAnimation : ScriptAnimation
{

    [SerializeField] private float minAlpha = 0;
    [SerializeField] private float maxAlpha = 0;

    private Coroutine fadingCoroutine;

    protected abstract void Fade(float _alpha);

    protected override IEnumerator Animation()
    {
        while (true)
        {

            yield return null;
        }
    }

}
