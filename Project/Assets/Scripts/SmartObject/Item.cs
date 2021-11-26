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
    public float value;

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


    public void UseFurniture(HandManager hand, GameObject furniture, Vector3 position)
	{
        furniture.transform.position = position;

        if (Input.mouseScrollDelta.y > 0)
		{
            furniture.transform.Rotate(0, 90, 0);
		}
        if (Input.mouseScrollDelta.y < 0)
        {
            furniture.transform.Rotate(0, -90, 0);
        }

        if (Input.GetButtonDown("Fire1"))
		{
            hand.handItemData = new Item();
		}
    }

}