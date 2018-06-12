using System;

/// <summary>
/// CoinTypeBySpawnChancePair pairs the CoinType with a spawn chance.
/// </summary>
[Serializable]
public class CoinTypeBySpawnChancePair
{
    public float Chance;
    public CoinType CoinType;
}