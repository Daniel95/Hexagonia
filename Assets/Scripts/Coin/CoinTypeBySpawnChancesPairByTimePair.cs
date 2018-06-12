using System;
using System.Collections.Generic;

/// <summary>
/// CoinTypeBySpawnChancesPairByTimePair pairs a CoinType with a spawn chance. The chance increases over time.
/// </summary>
[Serializable]
public class CoinTypeBySpawnChancesPairByTimePair
{
	public float Time;
	public List<CoinTypeBySpawnChancePair> CoinTypeBySpawnChancePairs;

	/// <summary>
	/// Gets a random coin to spawn with a chance.
	/// </summary>
	/// <returns>_coinType</returns>
    public CoinType GetRandomCoin()
    {
        CoinType _coinType = CoinType.Common;
        float _randomNumber = UnityEngine.Random.value;
        float _combinedChange = 0;

        for (int i = 0; i < CoinTypeBySpawnChancePairs.Count; i++)
        {
            CoinTypeBySpawnChancePair _coinTypeBySpawnChancePair = CoinTypeBySpawnChancePairs[i];
            float _nextCombinedChance = _combinedChange + _coinTypeBySpawnChancePair.Chance;

            if (_randomNumber >= _combinedChange && _randomNumber < _nextCombinedChance)
            {
                _coinType = _coinTypeBySpawnChancePair.CoinType;
                break;
            }

            _combinedChange = _nextCombinedChance;
        }
        return _coinType;
    }
}