using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Base behaviour of the gazebutton, that can be used to interact with buttons in VR.
/// </summary>
[RequireComponent(typeof(Button))]
public abstract class GazeButton : MonoBehaviour 
{
    [SerializeField] private Image gazeFillImage;

    protected Button Button;

    private float gazeSpeed = 1f;
    private Coroutine increaseGazeFillAmountOverTimeCoroutine;
    private int lastTriggeredFrameCount;
    private GameObject targetGameObject;

    protected abstract void OnTrigger();

    public void SetInteractable(bool _interactable)
    {
        Button.interactable = _interactable;
    }

    public void TryToTrigger()
    {
        if(!Button.interactable) { return; }
        if(lastTriggeredFrameCount == Time.frameCount) { return; }

        lastTriggeredFrameCount = Time.frameCount;

        OnTrigger();
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

    private void StartGazeHover(GameObject _hoveredGameObject)
    {
        if (_hoveredGameObject == targetGameObject)
        {
            if (increaseGazeFillAmountOverTimeCoroutine != null)
            {
                StopCoroutine(increaseGazeFillAmountOverTimeCoroutine);
            }
            AudioEffectManager.Instance.PlayEffect(AudioEffectType.ButtonEnter);
            increaseGazeFillAmountOverTimeCoroutine = StartCoroutine(IncreaseGazeFillAmountOverTime());
        }
    }

    private void StopGazeHover(GameObject _hoveredGameObject)
    {
        if (_hoveredGameObject == targetGameObject)
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

        TryToTrigger();

        increaseGazeFillAmountOverTimeCoroutine = null;
    }

    private void Awake()
    {
        Button = GetComponent<Button>();
        targetGameObject = Button.targetGraphic.gameObject;
    }
}