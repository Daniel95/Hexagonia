using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuRotator : MonoBehaviour
{
    [Serializable]
    public class MainMenuSectionRotation
    {
        public MainMenuSectionType MainMenuSectionType;
        public float YRotation;
    }

    public static MainMenuRotator Instance { get { return GetInstance(); } }

    public static Action SwitchedEvent;

    #region Singleton
    private static MainMenuRotator instance;

    private static MainMenuRotator GetInstance()
    {
        if(instance == null)
        {
            instance = FindObjectOfType<MainMenuRotator>();
        }
        return instance;
    }
    #endregion

    [SerializeField] private List<MainMenuSectionRotation> mainMenuSectionRotations;
    [SerializeField] private List<MainMenuSection> mainMenuSections;
    [SerializeField] private MainMenuSectionType startMainMenuSection;
    [SerializeField] private float rotateSpeed = 1;
    [SerializeField] private float swipeThreshold = 1;

    private MainMenuSection currentMainMenuSection;
    private Coroutine rotateToSectionUpdateCoroutine;

    public void RotateToSection(MainMenuSectionType _mainMenuSectionType)
    {
        MainMenuSectionRotation _mainMenuSectionRotation = mainMenuSectionRotations.Find(x => x.MainMenuSectionType == _mainMenuSectionType);
        
        if(_mainMenuSectionRotation == null)
        {
            Debug.LogError("MainMenuSectionRotation with MainMenuSectionType " + _mainMenuSectionType + " does not exist!");
            return;
        }

        if (rotateToSectionUpdateCoroutine != null) { return; }

        float _targetYRotation = _mainMenuSectionRotation.YRotation;

        Vector3 _currentEulerRotation = transform.rotation.eulerAngles;
        Vector3 _targetEulerRotation = new Vector3(_currentEulerRotation.x, _targetYRotation, _currentEulerRotation.z);
        Quaternion _targetRotation = new Quaternion
        {
            eulerAngles = _targetEulerRotation
        };


        rotateToSectionUpdateCoroutine = StartCoroutine(RotateTo(transform.rotation, _targetRotation, rotateSpeed));

        currentMainMenuSection = mainMenuSections.Find(x => x.MainMenuSectionType == _mainMenuSectionType);

        if (SwitchedEvent != null)
        {
            SwitchedEvent();
        }
    }

    private IEnumerator RotateTo(Quaternion _startRotation, Quaternion _targetRotation, float _speed, Action _onRotationCompleted = null)
    {
        float _time = 0;
        float _progress = 0;
        while (_time < 1)
        {
            _time += _speed * Time.deltaTime;
            _progress = Mathf.SmoothStep(0, 1, _time);
            transform.rotation = Quaternion.Lerp(_startRotation, _targetRotation, _progress);

            yield return null;
        }

        rotateToSectionUpdateCoroutine = null;
    }

    private void OnDraggingInput(Vector2 _inputPosition, Vector2 _delta)
    {
        float _xDragDistance = _inputPosition.x - PlatformBaseInput.StartDownPosition.x;

        if(Mathf.Abs(_xDragDistance) < swipeThreshold) { return; }

        if(currentMainMenuSection.LeftExists && _xDragDistance > 0)
        {
            RotateToSection(currentMainMenuSection.LeftMainMenuSectionType);
        }
        else if(currentMainMenuSection.RightExists && _xDragDistance < 0)
        {
            RotateToSection(currentMainMenuSection.RightMainMenuSectionType);
        }

    }

    private void Awake()
    {
        currentMainMenuSection = mainMenuSections.Find(x => x.MainMenuSectionType == startMainMenuSection);
    }

    private void OnEnable()
    {
        PlatformBaseInput.DraggingInputEvent += OnDraggingInput;
    }

    private void OnDisable()
    {
        PlatformBaseInput.DraggingInputEvent -= OnDraggingInput;
    }
}

