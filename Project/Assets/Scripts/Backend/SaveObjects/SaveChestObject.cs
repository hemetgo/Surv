using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class SaveChestObject : SaveComponent
{
    public List<SaveItem> itemList;

    public SaveChestObject(ChestObject chest)
	{
        itemList = SaveItems(chest.itemList);
	}

    public override void LoadComponent(GameObject gameObject)
    {
        ChestObject component = gameObject.GetComponent<ChestObject>();
        component.itemList = LoadItems();
    }

    private List<SaveItem> SaveItems(List<Item> items)
	{
        List<SaveItem> saveItems = new List<SaveItem>();

        foreach(Item item in items)
		{
            SaveItem save = new SaveItem(item);
            saveItems.Add(save);
		}

        return saveItems;
	}

    private List<Item> LoadItems()
	{
        List<Item> items = new List<Item>();

        foreach(SaveItem save in itemList)
		{
            Item item = new Item(save.GetItemData());
            item.amount = save.loadAmount;
            item.durability = save.loadDurability;
            items.Add(item);
		}

        return items;
	}

   
}
