using System.Collections.Generic;
using UnityEngine;
using UnityToolbag;

public class CoinPrefabByCoinTypeLibrary : MonoBehaviour
{
	public static CoinPrefabByCoinTypeLibrary Instance { get { return GetInstance(); } }

	private const string COINPREFAB_BY_COINTYPE_LIBRARY_PATH = "CoinTypeValueLibrary";

	[Reorderable] [SerializeField] private List<CoinPrefabByCoinTypePair> coinPrefabByCoinTypePairs;

	#region Singleton
	private static CoinPrefabByCoinTypeLibrary instance;

	private static CoinPrefabByCoinTypeLibrary GetInstance()
	{
		if(instance == null)
		{
			instance = Resources.Load<CoinPrefabByCoinTypeLibrary>(COINPREFAB_BY_COINTYPE_LIBRARY_PATH);
		}
		return instance;
	}
#endregion

	public GameObject GetCoinPrefab(CoinType _coinType)
	{
		GameObject _coinPrefab = null;
		for (int i = 0; i < coinPrefabByCoinTypePairs.Count; i++)
		{
			if(coinPrefabByCoinTypePairs[i].CoinType == _coinType)
			{
				_coinPrefab = coinPrefabByCoinTypePairs[i].CoinPrefab;
				break;
			}
		}
		return _coinPrefab;
	}
}