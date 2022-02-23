
using UnityEngine;

[System.Serializable]
public class DropData
{
	public ItemData itemData;
	public Vector2Int amountRange;
	public int amount;
	[Range(0, 100)] public float dropChance;
	
}

