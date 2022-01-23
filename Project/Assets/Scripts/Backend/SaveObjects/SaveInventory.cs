using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class SaveInventory : SaveComponent
{
    public List<SaveItem> itemList;

    public SaveInventory(InventoryManager inventoryManager)
	{
        itemList = SaveInventoryItems(inventoryManager.inventory);
	}

    public override void LoadComponent(GameObject gameObject)
    {
        InventoryManager component = gameObject.GetComponent<InventoryManager>();
        
        component.LoadInventory(LoadInventoryItems());
	}

    private List<SaveItem> SaveInventoryItems(Inventory inventory)
	{
        List<SaveItem> saveItems = new List<SaveItem>();

        foreach(Item item in inventory.itemList)
		{
            SaveItem save = new SaveItem(item);
            saveItems.Add(save);
		}

        return saveItems;
	}

    private List<Item> LoadInventoryItems()
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
