using System.Collections.Generic;
using UnityEngine;
using UnityToolbag;

public class CoinTypeByTimeLibrary : MonoBehaviour
{
	[Reorderable] [SerializeField] private List<TimeByCoinTypePair> timeTypeValues;

	public static CoinTypeByTimeLibrary Instance { get { return GetInstance(); } }

	private const string TIME_BY_COINTYPE_LIBRARY_PATH = "TimeLibrary";

	#region Singleton
	private static CoinTypeByTimeLibrary instance;

	private static CoinTypeByTimeLibrary GetInstance()
	{
		if (instance == null)
		{
			instance = Resources.Load<CoinTypeByTimeLibrary>(TIME_BY_COINTYPE_LIBRARY_PATH);
		}
		return instance;
	}
	#endregion

	public CoinType GetCoinType(float _time)
	{
		CoinType _coinType = CoinType.Common;

		for (int i = 0; i < timeTypeValues.Count; i++)
		{
			if (timeTypeValues[i].Time <= _time)
			{
				_coinType = timeTypeValues[i].CoinType;
				break;
			}
		}
		return _coinType;
	}
}