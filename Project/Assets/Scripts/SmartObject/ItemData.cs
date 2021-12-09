using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item", fileName = "New Item")]
public class ItemData : ScriptableObject
{
    public LangString itemName = new LangString();


    public Sprite icon;
    public Rarity rarity;
    public ItemType itemType;
    public AreaLangString description = new AreaLangString();
    public float value;
    public GameObject drop;

    public List<IngredientItem> recipe;


    public enum ItemType { Equipment, Tool, Material, Consumable, Food, Furniture }
    public enum ToolType { Axe, Pickaxe, Shovel }
    public enum Rarity { Common, Uncommon, Rare, Epic, Legendary}

    public int GetStackLimit()
	{
		switch (itemType)
		{
            case ItemType.Equipment: return 1;
            case ItemType.Tool: return 1;
            default: return 24;
		}
	}

    public string GetItemName()
	{
        return itemName.GetString();
	}

    public string GetItemDescription()
	{
        return description.GetString();
    }
}
