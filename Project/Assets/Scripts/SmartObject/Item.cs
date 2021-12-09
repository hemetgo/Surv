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

    public enum ItemType { Equipment, Tool, Material, Consumable, Food, Furniture }
    public enum ToolType { Axe, Pickaxe, Shovel }

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