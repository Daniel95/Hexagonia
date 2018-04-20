using System.Collections.Generic;
using UnityEngine;
using UnityToolbag;

public class CoinValueByCoinTypeLibrary : MonoBehaviour
{
	public static CoinValueByCoinTypeLibrary Instance { get { return GetInstance(); } }

	private const string COINVALUE_BY_COINTYPE_LIBRARY_PATH = "CoinTypeValueLibrary";

	[Reorderable] [SerializeField] private List<CoinValueByCoinTypePair> coinTypeValues;

	#region Singleton
	private static CoinValueByCoinTypeLibrary instance;

	private static CoinValueByCoinTypeLibrary GetInstance()
	{
		if(instance == null)
		{
			instance = Resources.Load<CoinValueByCoinTypeLibrary>(COINVALUE_BY_COINTYPE_LIBRARY_PATH);
		}
		return instance;
	}
#endregion

	public CoinValueByCoinTypePair GetCoin(CoinType _coinType)
	{
		CoinValueByCoinTypePair _coinValueData = null;
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