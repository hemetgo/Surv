using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item 
{
    public ItemData itemData;
    public int amount;
    [HideInInspector] public int durability;
    [HideInInspector] public int inventoryIndex;

    public delegate void UpdatedInventoryHandler();
    public event UpdatedInventoryHandler UpdatedInventory;

    public Item(ItemData data)
	{
        itemData = data;
    }

    public void SetInventoryIndex(int index)
    {
        inventoryIndex = index;
    }

    public int GetInventoryIndex()
    {
        return inventoryIndex;
    }

    public void RemoveDurability(HandManager hand)
	{
        durability -= 1;
        if (durability <= 0)
		{
            hand.RemoveItem(this);
		}
        UpdatedInventory();
    }

}