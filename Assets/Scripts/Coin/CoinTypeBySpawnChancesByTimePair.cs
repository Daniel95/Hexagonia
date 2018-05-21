using System;
using System.Collections.Generic;

[Serializable]
public class CoinTypeBySpawnChancesByTimePair
{
	public float Time;
	public List<CoinTypeBySpawnChancePair> CoinTypeBySpawnChancePair;

    public CoinType GetRandomCoin()
    {
        CoinType _coinType = CoinType.Common;
        float _randomNumber = UnityEngine.Random.value;

        for (int i = 0; i < CoinTypeBySpawnChancePair.Count; i++)
        {

        }

        return _coinType;
    }

}