using System.Collections.Generic;
using UnityEngine;
using UnityToolbag;

/// <summary>
/// Sets the coinPrefab by the coinType, can be done in the editor.
/// </summary>
public class CoinPrefabByCoinTypeLibrary : MonoBehaviour 
{
	public static CoinPrefabByCoinTypeLibrary Instance { get { return GetInstance(); } }

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

	private const string COINPREFAB_BY_COINTYPE_LIBRARY_PATH = "CoinTypeValueLibrary";

	[Reorderable] [SerializeField] private List<CoinPrefabByCoinTypePair> coinPrefabByCoinTypePairs;

	/// <summary>
	/// Gets the coinPrefab with the coinType in it.
	/// </summary>
	/// <param name="_coinType"></param>
	/// <returns>_coinPrefab</returns>
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