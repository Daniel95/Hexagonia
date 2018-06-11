using System;

/// <summary>
/// A class that pairs the CoinType with it's spawnChance.
/// </summary>
[Serializable]
public class CoinTypeBySpawnChancePair
{
    public float Chance;
    public CoinType CoinType;
}
