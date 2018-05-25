using System.Collections;
using UnityEngine;

public abstract class FadeAnimation : ScriptedAnimation
{

    [SerializeField] private float targetAlpha = 1;

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
