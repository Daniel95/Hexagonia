using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuRotator : MonoBehaviour
{

    public static Action SwitchedEvent;

    [Serializable]
    public class MainMenuSectionRotation
    {
        public MainMenuSectionType MainMenuSectionType;
        public float YRotation;
    }

    public static MainMenuRotator Instance { get { return GetInstance(); } }

    private static MainMenuRotator instance;

    private static MainMenuRotator GetInstance()
    {
        if(instance == null)
        {
            instance = FindObjectOfType<MainMenuRotator>();
        }
        return instance;
    }

    [SerializeField] private List<MainMenuSectionRotation> mainMenuSectionRotations;
    [SerializeField] private float rotateSpeed = 1;

    private Coroutine rotateToSectionUpdateCoroutine;

    public void RotateToSection(MainMenuSectionType _mainMenuSectionType)
    {
        MainMenuSectionRotation _mainMenuSectionRotation = mainMenuSectionRotations.Find(x => x.MainMenuSectionType == _mainMenuSectionType);
        
        if(_mainMenuSectionRotation == null)
        {
            Debug.LogError("MainMenuSectionRotation with MainMenuSectionType " + _mainMenuSectionType + " does not exist!");
            return;
        }

        float _targetYRotation = _mainMenuSectionRotation.YRotation;

        Vector3 _currentEulerRotation = transform.rotation.eulerAngles;
        Vector3 _targetEulerRotation = new Vector3(_currentEulerRotation.x, _targetYRotation, _currentEulerRotation.z);
        Quaternion _targetRotation = new Quaternion
        {
            eulerAngles = _targetEulerRotation
        };

        if (rotateToSectionUpdateCoroutine != null)
        {
            StopCoroutine(rotateToSectionUpdateCoroutine);
        }

        rotateToSectionUpdateCoroutine = StartCoroutine(RotateTo(transform.rotation, _targetRotation, rotateSpeed));

        if (SwitchedEvent != null)
        {
            SwitchedEvent();
        }
    }

    private IEnumerator RotateTo(Quaternion _startRotation, Quaternion _targetRotation, float _speed, Action _onRotationCompleted = null)
    {
        float _time = 0;

        while (_time < 1)
        {
            _time += _speed * Time.deltaTime;
            transform.rotation = Quaternion.Lerp(_startRotation, _targetRotation, _time);

            yield return null;
        }

        rotateToSectionUpdateCoroutine = null;
    }
}

