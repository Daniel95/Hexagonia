using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GazeButton : MonoBehaviour {

    [SerializeField] protected Image buttonImage;
    [SerializeField] private Image gazeFillImage;

    [SerializeField] private float gazeSpeed = 0.006f;

    private Coroutine increaseGazeFillAmountOverTimeCoroutine;

    protected virtual void OnGazeFilled() { }

    private void StartGazeHover(GameObject _hoveredGameObject)
    {
        Debug.Log(_hoveredGameObject);
        if (_hoveredGameObject == buttonImage.gameObject)
        {
            increaseGazeFillAmountOverTimeCoroutine = StartCoroutine(IncreaseGazeFillAmountOverTime());
        }
    }

    private void StopGazeHover(GameObject _hoveredGameObject)
    {
        if (_hoveredGameObject == buttonImage.gameObject)
        {
            StopCoroutine(increaseGazeFillAmountOverTimeCoroutine);
            gazeFillImage.fillAmount = 0;
        }
    }

    private IEnumerator IncreaseGazeFillAmountOverTime()
    {
        while (gazeFillImage.fillAmount < 1)
        {
            gazeFillImage.fillAmount += gazeSpeed;
            yield return null;
        }

        OnGazeFilled();

        increaseGazeFillAmountOverTimeCoroutine = null;
    }

    private void OnEnable()
    {
        GvrReticlePointer.OnButtonEnterEvent += StartGazeHover;
        GvrReticlePointer.OnButtonExitEvent += StopGazeHover;
    }

    private void OnDisable()
    {
        GvrReticlePointer.OnButtonEnterEvent -= StartGazeHover;
        GvrReticlePointer.OnButtonExitEvent -= StopGazeHover;
    }
}