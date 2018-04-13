using System.Collections.Generic;
using UnityEngine;
using UnityToolbag;

public class CoinTypeValueLibrary : MonoBehaviour
{
	public static CoinTypeValueLibrary Instance { get { return GetInstance(); } }

	private const string COIN_TYPE_VALUE_LIBRARY_PATH = "Resources/CoinTypeValueLibrary";

	[Reorderable] [SerializeField] private List<CoinTypeValueData> coinTypeValues;

	#region Singleton
	private static CoinTypeValueLibrary instance;

	private static CoinTypeValueLibrary GetInstance()
	{
		if(instance == null)
		{
			instance = Resources.Load<CoinTypeValueLibrary>(COIN_TYPE_VALUE_LIBRARY_PATH);
		}
		return instance;
	}
#endregion

	public CoinTypeValueData GetCoin(CoinType _coinType)
	{
		CoinTypeValueData _coinValueData = null;
		for (int i = 0; i < coinTypeValues.Count; i++)
		{
			if(coinTypeValues[i].CoinType == _coinType)
			{
				_coinValueData = coinTypeValues[i];
				break;
			}
		}
		return _coinValueData;
	}
}