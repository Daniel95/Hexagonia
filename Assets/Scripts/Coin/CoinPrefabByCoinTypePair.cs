using System;
using UnityEngine;

/// <summary>
/// ScriptContainer for the Coinprefab. Used as a List in CoinPrefabByCoinTypePair.cs
/// </summary>
[Serializable]
public class CoinPrefabByCoinTypePair
{
	public GameObject CoinPrefab;
	public CoinType CoinType;
}