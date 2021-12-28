using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Inventory
{
	public delegate void ItemCollectedHandler(Item item);
	public event ItemCollectedHandler ItemCollected;

	public bool main;
	public List<Item> itemList;


	// Construtor
	public Inventory(int length)
	{
		itemList = new List<Item>();

		for (int i = 0; i < length; i++)
		{
			itemList.Add(new Item(Resources.Load<ItemData>("ItemData/_Empty")));
		}
	}

	// Adiciona item ao inventário
	public void AddItem(Item item)
	{
		// Verifica se já existe um slot para esse item
		foreach(Item it in itemList)
		{
			if (it.itemData == item.itemData) 
			{
				if (it.amount < it.itemData.GetStackLimit())
				{
					it.amount += item.amount;

					if (ItemCollected != null)
					{
						ItemCollected(item);
					}
					return;
				}
			} 
		}

		// Procura o primeiro slot vazio para esse item
		for (int i = 0; i < itemList.Count; i++)
		{
			Item it = itemList[i];

			if (it.amount <= 0)
			{
				item.SetInventoryIndex(i);
				itemList[i] = item;
				if (ItemCollected != null)
				{
					ItemCollected(item);
				}
				return;
			}
		}

		// Apenas adiciona o item ao inventário
		item.SetInventoryIndex(itemList.Count);
		itemList.Add(item);
		
		
		// Notifica de que um item foi coletado
		if (ItemCollected != null)
		{
			ItemCollected(item);
		}
	}
	
	// Remove item do inventário
	public void RemoveItem(Item removeItem)
	{
		int toRemove = removeItem.amount;
		foreach(Item item in itemList)
		{
			if (item.itemData == removeItem.itemData)
			{
				// Se tiver o suficiente nesse slot, remove dele
				if (item.amount >= toRemove)
				{
					item.amount -= toRemove;
					return;
				}
				// Se nao remove o que der e espera o proximo
				else
				{
					toRemove -= item.amount;
					item.amount = 0;
				}
			}
		}
	}

	// Retorna a lista de itens do inventário
	public List<Item> GetItemList()
	{
		return itemList;
	}

	// Retorna a quantidade do respectivo item no inventario
	public int GetInventoryAmount(ItemData itemData)
	{
		int amount = 0;
		foreach (Item item in itemList)
		{
			if (item.itemData == itemData) amount += item.amount;
		}

		return amount;
	}

	public bool IsFull()
	{
		foreach (Item item in itemList)
		{
			if (item.amount <= 0)
			{
				return false;
			}
		}

		return true;
	}
}