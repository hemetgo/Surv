using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item 
{
    public ItemData itemData;
    public int amount;
    public int inventoryIndex;

    public Item()
	{
        itemData =  new ItemData();
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