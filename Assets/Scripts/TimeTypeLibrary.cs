using System.Collections.Generic;
using UnityEngine;
using UnityToolbag;

public class TimeTypeLibrary : MonoBehaviour
{
	[Reorderable] [SerializeField] private List<TimeTypeData> timeTypeValues;

	public static TimeTypeLibrary Instance { get { return GetInstance(); } }

	private const string TIME_TYPEE_LIBRARY_PATH = "Resources/TimeTypeLibrary";

	#region Singleton
	private static TimeTypeLibrary instance;

	private static TimeTypeLibrary GetInstance()
	{
		if (instance == null)
		{
			instance = Resources.Load<TimeTypeLibrary>(TIME_TYPEE_LIBRARY_PATH);
		}
		return instance;
	}
	#endregion

	public TimeTypeData GetTime(float _time)
	{
		Debug.Log("Change TimeType");

		TimeTypeData _timeValueData = null;
		for (int i = 0; i < timeTypeValues.Count; i++)
		{
			if (timeTypeValues[i].Time == _time)
			{
				_timeValueData = timeTypeValues[i];
				break;
			}
		}
		return _timeValueData;
	}
}