using System.Collections.Generic;
using UnityEngine;
using UnityToolbag;

public class CoinSpawnChancesByTimeLibrary : MonoBehaviour
{
	[Reorderable] [SerializeField] private List<CoinTypeBySpawnChancesByTimePair> coinTypeBySpawnChancesByTimePairs;

	public static CoinSpawnChancesByTimeLibrary Instance { get { return GetInstance(); } }

	private const string COIN_SPAWN_CHANCES_BY_TIME_LIBRARY = "CoinSpawnChancesByTimeLibrary";

	#region Singleton
	private static CoinSpawnChancesByTimeLibrary instance;

	private static CoinSpawnChancesByTimeLibrary GetInstance()
	{
		if (instance == null)
		{
			instance = Resources.Load<CoinSpawnChancesByTimeLibrary>(COIN_SPAWN_CHANCES_BY_TIME_LIBRARY);
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
            CoinTypeBySpawnChancesByTimePair _currentCoinTypeBySpawnChancesByTimePair = coinTypeBySpawnChancesByTimePairs[i];

            if (i >= coinTypeBySpawnChancesByTimePairs.Count - 1)
            {
                _coinTypeBySpawnChancesByTimePair = _currentCoinTypeBySpawnChancesByTimePair;
            }
            else
            {
                float _currentTime = _currentCoinTypeBySpawnChancesByTimePair.Time;
                float _nextTime = coinTypeBySpawnChancesByTimePairs[i + 1].Time;

                if (_time >= _currentTime && _time < _nextTime)
                {
                    _coinTypeBySpawnChancesByTimePair = _currentCoinTypeBySpawnChancesByTimePair;
                    break;
                }
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
            _coinTypeBySpawnChancesByTimePair.CoinTypeBySpawnChancePairs.ForEach(x => _combinedChance += x.Chance);

            if (_combinedChance != 1)
            {
                Debug.LogError("Combined spawn chance of coinTypeBySpawnChancesByTimePair with time " + _coinTypeBySpawnChancesByTimePair.Time + " is " + _combinedChance + ", should be 1");
            }
        }
    }

}