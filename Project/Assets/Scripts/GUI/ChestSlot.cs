using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChestSlot : MonoBehaviour
{
    [Header("Data")]
	public int slotId;
	public Item item;

	[Header("Graphics")]
	public Image itemImage;
	public TextMeshProUGUI txtAmount;
	public Sprite emptyImage;

	[Header("Infos")]
	public GameObject durabilityData;
	public Image durabilityBar;
	public TextMeshProUGUI txtName;

	// aux
	private ChestManager chestManager;
	private bool isOver;

	private void Start()
	{
		durabilityData.SetActive(false);
	}

	private void Update()
	{
		if (isOver && !IsEmpty())
		{
			if (Input.GetButtonDown("Fire1"))
			{
				Item i = new Item(item);
				i.amount = 1;
				FindObjectOfType<InventoryManager>().inventory.AddItem(i);
				
				item.amount -= 1;
				RefreshSlot();
				FindObjectOfType<InventoryManager>().RefreshInventory();
				FindObjectOfType<InventoryManager>().itemBar.RefreshItemBar();
			}

			if (Input.GetButtonDown("Fire2"))
			{
				Item i = new Item(item); 
				FindObjectOfType<InventoryManager>().inventory.AddItem(i);

				item.amount = 0;
				RefreshSlot();
				FindObjectOfType<InventoryManager>().RefreshInventory();
				FindObjectOfType<InventoryManager>().itemBar.RefreshItemBar();
			}
		}
	}

	public void PointerExit()
	{
		isOver = false;
		txtName.gameObject.SetActive(false);
	}

	public void PointerEnter()
	{
		isOver = true;
		if (item.amount > 0) txtName.gameObject.SetActive(true);
	}

	public bool IsEmpty()
	{
		if (item == null) return true;
		else
		{
			if (item.amount <= 0) return true;
			if (item.itemData.itemName.english.Equals("")) return true;
		}
		return false;
	}

	public void RefreshSlot()
	{
		item.inventoryIndex = slotId;

		if (item.amount > 0)
		{
			itemImage.gameObject.SetActive(true);
			txtAmount.gameObject.SetActive(true);

			itemImage.sprite = item.itemData.icon;
			txtName.text = item.itemData.GetItemName();

			if (item.itemData.UseDurability())
			{
				txtAmount.text = "";
				if (item.durability < item.itemData.GetDurability())
				{
					durabilityData.SetActive(true);
					durabilityBar.fillAmount = (float)item.durability / (float)item.itemData.GetDurability();
				}
			}
			else
			{
				durabilityData.SetActive(false);
				txtAmount.text = "" + item.amount;
			}
		}
		else
		{
			itemImage.sprite = emptyImage;
			durabilityData.SetActive(false);
			txtAmount.gameObject.SetActive(false);
			txtAmount.text = "";
		}
	}

	public void SetItem(Item item)
	{
		this.item = item;
		RefreshSlot();
	}

	public void SetManager(ChestManager manager)
	{
		chestManager = manager;
	}
}
