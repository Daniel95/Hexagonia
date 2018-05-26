using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GazeButton : MonoBehaviour {

    [SerializeField] protected Image buttonImage;
    [SerializeField] private Image gazeFillImage;

    private float gazeSpeed = 1f; //0.006f old gazeSpeed without Time.deltaTime

    private Coroutine increaseGazeFillAmountOverTimeCoroutine;

    protected virtual void OnGazeFilled() { }

    private void StartGazeHover(GameObject _hoveredGameObject)
    {
        if (_hoveredGameObject == buttonImage.gameObject)
        {
            if (increaseGazeFillAmountOverTimeCoroutine != null)
            {
                StopCoroutine(increaseGazeFillAmountOverTimeCoroutine);
            }
            increaseGazeFillAmountOverTimeCoroutine = StartCoroutine(IncreaseGazeFillAmountOverTime());
        }
    }

    private void StopGazeHover(GameObject _hoveredGameObject)
    {
        if (_hoveredGameObject == buttonImage.gameObject)
        {
            if(increaseGazeFillAmountOverTimeCoroutine != null)
            {
                StopCoroutine(increaseGazeFillAmountOverTimeCoroutine);
                increaseGazeFillAmountOverTimeCoroutine = null;
            }
            gazeFillImage.fillAmount = 0;
        }
    }

    private IEnumerator IncreaseGazeFillAmountOverTime()
    {
        while (gazeFillImage.fillAmount < 1)
        {
            gazeFillImage.fillAmount += gazeSpeed * Time.deltaTime;
            yield return null;
        }

        OnGazeFilled();

        increaseGazeFillAmountOverTimeCoroutine = null;
    }

    protected virtual void OnEnable()
    {
        GvrReticlePointer.OnButtonEnterEvent += StartGazeHover;
        GvrReticlePointer.OnButtonExitEvent += StopGazeHover;
    }

    protected virtual void OnDisable()
    {
        GvrReticlePointer.OnButtonEnterEvent -= StartGazeHover;
        GvrReticlePointer.OnButtonExitEvent -= StopGazeHover;
    }
}