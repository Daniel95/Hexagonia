using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityToolbag;

public class CoinSpawnChancesByTimeLibrary : MonoBehaviour
{
	[Reorderable] [SerializeField] private List<CoinTypeBySpawnChancesByTimePair> coinTypeBySpawnChancesByTimePairs;

	public static CoinSpawnChancesByTimeLibrary Instance { get { return GetInstance(); } }

	private const string TIME_BY_COINTYPE_LIBRARY_PATH = "TimeLibrary";

	#region Singleton
	private static CoinSpawnChancesByTimeLibrary instance;

	private static CoinSpawnChancesByTimeLibrary GetInstance()
	{
		if (instance == null)
		{
			instance = Resources.Load<CoinSpawnChancesByTimeLibrary>(TIME_BY_COINTYPE_LIBRARY_PATH);
		}
		return instance;
	}
	#endregion

    private CoinType coinType;

	public List<CoinType> GetCoinTypesToSpawn(int _coinTypesAmount, float _time)
	{
        CoinTypeBySpawnChancesByTimePair _coinTypeBySpawnChancesByTimePair = null;

        for (int i = 0; i < coinTypeBySpawnChancesByTimePairs.Count; i++)
		{
			if (coinTypeBySpawnChancesByTimePairs[i].Time >= _time)
			{
                _coinTypeBySpawnChancesByTimePair = coinTypeBySpawnChancesByTimePairs[i];
				break;
			}
		}

        List<CoinType> _coinTypes = new List<CoinType>();

        for (int i = 0; i < _coinTypesAmount; i++)
        {
            CoinType _coinType = _coinTypeBySpawnChancesByTimePair.GetRandomCoin();
            _coinTypes.Add(_coinType);
        }

		return _coinTypes;
	}

    [ContextMenu("CheckCombinedChance")]
    private void CheckCombinedChance()
    {
        for (int i = 0; i < coinTypeBySpawnChancesByTimePairs.Count; i++)
        {
            float _combinedChance = 0;

            CoinTypeBySpawnChancesByTimePair _coinTypeBySpawnChancesByTimePair = coinTypeBySpawnChancesByTimePairs[i];
            _coinTypeBySpawnChancesByTimePair.CoinTypeBySpawnChancePair.ForEach(x => _combinedChance += x.Chance);

            if (_combinedChance != 1)
            {
                Debug.LogError("Combined spawn chance of coinTypeBySpawnChancesByTimePair with time " + _coinTypeBySpawnChancesByTimePair.Time + " is " + _combinedChance + ", should be 1");
            }
        }
    }

}