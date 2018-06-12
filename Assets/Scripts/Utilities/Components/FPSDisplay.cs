using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class FPSDisplay : MonoBehaviour
{
    // Attach this to any object to make a frames/second indicator.
    //
    // It calculates frames/second over each updateInterval,
    // so the display does not keep changing wildly.
    //
    // It is also fairly accurate at very low FPS counts (<10).
    // We do this not by simply counting frames per interval, but
    // by accumulating FPS for each frame. This way we end up with
    // corstartRect overall FPS even if the interval renders something like
    // 5.5 frames.

    public static bool FPSCounterEnabled
    {
        get { return Convert.ToBoolean(PlayerPrefs.GetInt(FPS_COUNTER_ENABLED, 0)); }
        set { PlayerPrefs.SetInt(FPS_COUNTER_ENABLED, Convert.ToInt16(value)); }
    }

    private const string FPS_COUNTER_ENABLED = "FPSCounterEnabled";

    [SerializeField] private float frequency = 0.5F; // The update frequency of the fps
    [SerializeField] private int nbDecimal = 1; // How many decimal do you want to display

    private float accum = 0f; // FPS accumulated over the interval
    private int frames = 0; // Frames drawn over the interval
    private string sFPS = ""; // The fps formatted into a string.
    private Text text;

    private Coroutine fpsCoroutine;

    private void Awake()
    {
        text = GetComponent<Text>();
        SetFPSCounter(FPSCounterEnabled);
    }

    private void SetFPSCounter(bool _enabled)
    {
        if (_enabled && fpsCoroutine == null)
        {
            fpsCoroutine = StartCoroutine(FPS());
        }
        else
        {
            if (fpsCoroutine != null)
            {
                StopCoroutine(fpsCoroutine);
                fpsCoroutine = null;
            }
            text.text = "";
        }
    }

    private void Update()
    {
        accum += Time.timeScale / Time.deltaTime;
        ++frames;
    }

    private IEnumerator FPS()
    {
        // Infinite loop executed every "frenquency" secondes.
        while (true)
        {
            // Update the FPS
            float fps = accum / frames;
            sFPS = fps.ToString("f" + Mathf.Clamp(nbDecimal, 0, 10));

            text.text = "FPS: " + sFPS;

            accum = 0.0F;
            frames = 0;

            yield return new WaitForSeconds(frequency);
        }
    }

}