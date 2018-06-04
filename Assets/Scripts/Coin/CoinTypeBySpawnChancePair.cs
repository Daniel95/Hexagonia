using System;

/// <summary>
/// ScriptContainer pairs CoinType with SpawnChance. Used in CoinTypeBySpawnChancesPairByTimePair.cs
/// </summary>
[Serializable]
public class CoinTypeBySpawnChancePair
{
    public float Chance;
    public CoinType CoinType;
}
