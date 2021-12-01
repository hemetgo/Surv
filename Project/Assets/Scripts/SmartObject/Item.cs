using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item 
{
    public string itemName;
    public Sprite icon;
    public ItemType itemType;
    public int stackLimit;
    public int amount;
    public string description;
    public float value;

    public int inventoryIndex;

    public enum ItemType { Equipment, Tool, Material, Consumable, Food, Furniture }
    public enum ToolType { Axe, Pickaxe, Shovel }

    public Item()
	{
        itemName = "";
        amount = 0;
        value = 0;

		switch (itemType)
        {
            case ItemType.Equipment:
                stackLimit = 1;
                break;
            case ItemType.Tool:
                stackLimit = 1;
                break;
            default:
                stackLimit = 24;
                break;
        }
	}


    public void SetInventoryIndex(int index)
    {
        inventoryIndex = index;
    }

    public int GetInventoryIndex()
    {
        return inventoryIndex;
    }

    #region Tools
    public GameObject GetPrefab()
    {
        string path = "";
        if (itemType == ItemType.Furniture) path = "Furnitures";
        foreach(GameObject prefab in Resources.LoadAll<GameObject>(path))
        {
            if (prefab.name == itemName) return prefab;
        }

        return null;
    }

    #endregion
}