using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Item", fileName = "New Item")]
public class ItemData : ScriptableObject
{
    public LangString itemName;
    public Sprite icon;
    public Rarity rarity;
    public ItemType itemType;
    public AreaLangString description;
    public float value;
    public GameObject drop;

    [Header("Battle")]
    public int power = 1;
    public int knockbackForce = 10;

    public List<IngredientItem> recipe;

    public enum ItemType { Battle, Tool, Nature, Food, Furniture }
    public enum Rarity { Common, Uncommon, Rare, Epic, Legendary}

    public int GetStackLimit()
    { 
		switch (itemType)
		{
            case ItemType.Battle: return 1;
            case ItemType.Tool: return 1;
            default: return 50;
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
