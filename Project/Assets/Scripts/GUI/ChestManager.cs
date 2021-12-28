using System.Collections;
using System.Linq;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ChestManager : MonoBehaviour
{
	public int chestSize;
	public ChestObject chest = null;

	[Header("Objects")]
    public Transform chestSlotsContainer;
    public List<ChestSlot> chestSlots;

    [Header("Prefabs")]
    public GameObject chestSlotPrefab;



    // Start is called before the first frame update
    void Start()
    {
		GenerateSlots();
		RefreshChest();
	}

	private void GenerateSlots()
	{
		for (int i = 0; i < chestSize; i++)
		{
			ChestSlot slot;
			slot = Instantiate(chestSlotPrefab.gameObject, chestSlotsContainer.transform).GetComponentInChildren<ChestSlot>();
			slot.SetManager(this);
			slot.slotId = i;
			slot.item = new Item(Resources.Load<ItemData>("ItemData/_Empty"));
			chestSlots.Add(slot);
		}
	}

	public void RefreshChest()
	{
		if (chest != null)
		{
			// Increment item bar and inventory
			for (int i = 0; i < chest.itemList.Count; i++)
			{
				Item item = chest.itemList[i];

				ChestSlot slot = chestSlots[i];
				slot.SetManager(this);
				slot.item = item;
				slot.RefreshSlot();
			}
		}
	}

	public void AddItem(Item item)
	{
		// Verifica se já existe um slot para esse item
		foreach (Item it in chest.itemList)
		{
			if (it.itemData == item.itemData)
			{
				if (it.amount < it.itemData.GetStackLimit())
				{
					it.amount += item.amount;
					return;
				}
			}
		}

		// Procura o primeiro slot vazio para esse item
		for (int i = 0; i < chest.itemList.Count; i++)
		{
			Item it = chest.itemList[i];

			if (it.amount <= 0)
			{
				item.SetInventoryIndex(i);
				chest.itemList[i] = item;
				return;
			}
		}

		// Apenas adiciona o item ao inventário
		item.SetInventoryIndex(chest.itemList.Count);
		chest.itemList.Add(item);

		RefreshChest();
	}

}
