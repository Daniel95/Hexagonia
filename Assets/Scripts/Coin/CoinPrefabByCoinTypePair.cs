using System;
using UnityEngine;

/// <summary>
/// ScriptContainer t pair the CoinPrefab with the CoinType. Used in CoinPrefabByCoinTypeLibrary.cs.
/// </summary>
[Serializable]
public class CoinPrefabByCoinTypePair
{
	public GameObject CoinPrefab;
	public CoinType CoinType;
}