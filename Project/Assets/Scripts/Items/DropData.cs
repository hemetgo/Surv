
using UnityEngine;

[System.Serializable]
public class DropData
{
	public ItemData itemData;
	public Vector2Int amountRange;
	[Range(0, 100)] public float dropChance;
	
	private int amount;

	public void SetAmount()
	{
		amount = Random.Range(amountRange.x, amountRange.y);
	}

	public int GetAmount()
	{
		return amount;
	}
}

