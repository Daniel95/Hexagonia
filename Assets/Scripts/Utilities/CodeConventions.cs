using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// No usings in code, always import usings above class
/// Remove unused usings
/// </summary>
public class CodeConventions : MonoBehaviour 
{
    public int PublicTest { get { return privateTest ; } set { privateTest = value; } }
    public int PublicTest2 { get { return privateTest ; } set { privateTest = value; } }
    public static CodeConventions Instance { get { return GetInstance(); } }

    public int PublicComplicatedTest
    {
        get
        {
            int test1 = 4 / 65;
            int test2 = test1 % 42;
            return test2;
        }
    }

    public static Action Event;

    #region Singleton
    private static CodeConventions instance;

	private static CodeConventions GetInstance()
	{
		if (instance == null)
		{
			instance = FindObjectOfType<CodeConventions>();
		}
		return instance;
	}
    #endregion

	private const string CONST_TEST = "TEST123";

    [SerializeField] [Range(0, 93)] private int serializeFieldTest;

    private int privateTest;

    /// <summary>
    ///  No comments in code, only above the method
    /// </summary>
    public void PublicExample()
    {
        bool _testBool = false;

        if (_testBool) { return; }
    }

    private void TodoExample()
    {
        //TODO: You can add TODO's as comment in the code
    }

    private void PrivateExample() 
    {
        privateTest = 1;
        int _interalTest = 2;
        _interalTest = 3;

        //No interal conventions for indexes variables in for loops
        for (int i = 0; i < _interalTest; i++)
        {

        }

        foreach (var _item in new List<int>(3)) 
        {

        }
    }

    private void OnEvent()
    {

    }

    private void OnEnable()
    {
        Event += OnEvent;
    }

    private void OnDisable()
    {
        Event -= OnEvent;
    }
}

public enum TestType
{
	First,
	Second,
	Third
}