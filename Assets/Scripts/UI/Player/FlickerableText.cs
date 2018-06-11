using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FlickerableText : MonoBehaviour
{
    [SerializeField] protected Text Text;
    [SerializeField] private Color flickerColor;
    [SerializeField] private float flickerTime;

    private Coroutine flickerEffectCoroutine;
    private Color startColor;

    protected void StartFlicker()
    {
        if (flickerEffectCoroutine == null)
        {
            flickerEffectCoroutine = StartCoroutine(FlickerEffect());
        }
    }

    protected virtual void Awake()
    {
        startColor = Text.color;
    }

    private IEnumerator FlickerEffect()
    {
        Text.color = flickerColor;
        yield return new WaitForSeconds(flickerTime);
        Text.color = startColor;
        flickerEffectCoroutine = null;
    }
}
