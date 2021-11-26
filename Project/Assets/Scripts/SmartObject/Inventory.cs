using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Inventory
{
	public delegate void ItemCollectedHandler(Item item);
	public event ItemCollectedHandler ItemCollected;

	public List<Item> itemList;


	// Construtor
	public Inventory()
	{
		itemList = new List<Item>();
	}

	// Adiciona item ao invent�rio
	public void AddItem(Item item)
	{
		// Verifica se j� existe um slot para esse item
		foreach(Item it in itemList)
		{
			if (it.itemName == item.itemName)
			{
				if (it.amount < it.stackLimit)
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
				itemList[i] = item;
				if (ItemCollected != null)
				{
					ItemCollected(item);
				}
				return;
			}
		}

		// Apenas adiciona o item ao invent�rio
		itemList.Add(item);
		
		
		// Notifica de que um item foi coletado
		if (ItemCollected != null)
		{
			ItemCollected(item);
		}
	}
	

	// Retorna a lista de itens do invent�rio
	public List<Item> GetItemList()
	{
		return itemList;
	}
}