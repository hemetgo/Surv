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

    [Header("Animation")]
    public Animations animation;

    [Header("Battle")]
    public int power = 1;
    [Range(0, 100)] public float critRate = 10;
    public int knockbackForce = 10;

    [Header("Craft")]
    public List<IngredientItem> recipe;

    public enum ItemType { Battle, Tool, Nature, Food, Furniture }
    public enum Animations { Default, Sword }
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
    
    public string GetLangType()
	{
		switch (PlayerPrefs.GetString("Lang"))
		{
            case "English":
                return itemType.ToString();
            case "Portugues":
                switch (itemType)
                {
                    case ItemType.Battle:
                        return "Batalha";
                    case ItemType.Food:
                        return "Alimento";
                    case ItemType.Furniture:
                        return "Mobília";
                    case ItemType.Nature:
                        return "Natureza";
                    case ItemType.Tool:
                        return "Ferramenta";
                    default:
                        return itemType.ToString();
                }
        }
        return itemType.ToString();
    }

    public bool UseDurability()
	{
        if (itemType == ItemType.Battle
            || itemType == ItemType.Tool)
		{
            return true;
		} 
        return false;
	}

    public int GetDurability()
	{
        if (itemType == ItemType.Battle)
		{
            WeaponData weapon = this as WeaponData;
            return weapon.durability;
		} else if (itemType == ItemType.Tool)
		{
            ToolData tool = this as ToolData;
            return tool.durability;
        }
        return 1;
	}
}
