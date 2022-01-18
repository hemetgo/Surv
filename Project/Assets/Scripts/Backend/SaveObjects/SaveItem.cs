using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class SaveItem
{
    public string loadName;
    public int loadAmount;
    public int loadDurability;

    public SaveItem(Item item)
	{
        loadName = item.itemData.itemName.english;
        loadAmount = item.amount;
		loadDurability = item.durability;
	}

    public ItemData GetItemData()
    {
		// Search in Battle
		foreach (ItemData itemData in Resources.LoadAll<ItemData>("ItemData/Battle/"))
		{
			if (itemData.itemName.english == loadName) return itemData;
		}
		// Search in Consumable
		foreach (ItemData itemData in Resources.LoadAll<ItemData>("ItemData/Consumable/"))
		{
			if (itemData.itemName.english == loadName) return itemData;
		}
		// Search in Decoration
		foreach (ItemData itemData in Resources.LoadAll<ItemData>("ItemData/Decoration/"))
		{
			if (itemData.itemName.english == loadName) return itemData;
		}
		// Search in Material
		foreach (ItemData itemData in Resources.LoadAll<ItemData>("ItemData/Material/"))
		{
			if (itemData.itemName.english == loadName) return itemData;
		}
		// Search in Miscellaneous
		foreach (ItemData itemData in Resources.LoadAll<ItemData>("ItemData/Miscellaneous/"))
		{
			if (itemData.itemName.english == loadName) return itemData;
		}
		// Search in Tool
		foreach (ItemData itemData in Resources.LoadAll<ItemData>("ItemData/Tool/"))
		{
			if (itemData.itemName.english == loadName) return itemData;
		}

		return null;
	}
}
