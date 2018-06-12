using System.Collections.Generic;
using UnityEngine;
using UnityToolbag;

/// <summary>
/// There will be a chance for higher value coins to spawn. The 'Legendary' coin has a lower chance of spawning compared to the 'Uncommon' coin.
/// </summary>
public class CoinSpawnChancesByTimeLibrary : MonoBehaviour
{
	public static CoinSpawnChancesByTimeLibrary Instance { get { return GetInstance(); } }

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

    private const string COIN_SPAWN_CHANCES_BY_TIME_LIBRARY = "CoinSpawnChancesByTimeLibrary";
    
    [Reorderable] [SerializeField] private List<CoinTypeBySpawnChancesPairByTimePair> coinTypeBySpawnChancesByTimePairs;

    private CoinType coinType;

	/// <summary>
	/// Spawns coins with a chance by time. 'common' coins will always be spawned on random locations.
	/// </summary>
	/// <param name="_coinTypesAmount"></param>
	/// <param name="_time"></param>
	/// <returns></returns>
	public List<CoinType> GetCoinTypesToSpawn(int _coinTypesAmount, float _time)
	{
        CoinTypeBySpawnChancesPairByTimePair _coinTypeBySpawnChancesByTimePair = null;

        for (int i = 0; i < coinTypeBySpawnChancesByTimePairs.Count; i++)
		{
            CoinTypeBySpawnChancesPairByTimePair _currentCoinTypeBySpawnChancesByTimePair = coinTypeBySpawnChancesByTimePairs[i];

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

            CoinTypeBySpawnChancesPairByTimePair _coinTypeBySpawnChancesByTimePair = coinTypeBySpawnChancesByTimePairs[i];
            _coinTypeBySpawnChancesByTimePair.CoinTypeBySpawnChancePairs.ForEach(x => _combinedChance += x.Chance);

            if (_combinedChance != 1)
            {
                Debug.LogError("Combined spawn chance of coinTypeBySpawnChancesByTimePair with time " + _coinTypeBySpawnChancesByTimePair.Time + " is " + _combinedChance + ", should be 1");
            }
        }
    }
}