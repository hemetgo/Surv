using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item 
{
    public ItemData itemData;
    public int amount;
    [HideInInspector] public int inventoryIndex;

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
}