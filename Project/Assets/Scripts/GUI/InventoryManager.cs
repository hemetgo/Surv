using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
	[Header("Inventory")]
	public int slotsCount;
	public Transform inventorySlotContainer;
	public GameObject inventorySlotPrefab;
    public Inventory inventory;
	private GameObject inventoryInterface;
	[HideInInspector] public List<InventorySlot> inventorySlotList;

	[Header("Item Bar")]
	public ItemBarManager itemBar; 

	[Header("Feedbacks")]
	public FeedbackManager feedbackManager;

	// Aux
	public InventorySlot dragSlot;
	public InventorySlot dropSlot;

	private void Start()
	{
		inventoryInterface = gameObject.transform.GetChild(0).gameObject;
		StartInventory();

		GenerateSlots();
	}

	private void Update()
	{
		ShowInventory();
	}


	public void StartInventory()
	{
		inventory = new Inventory();
		inventory.ItemCollected += OnItemCollected;
		itemBar.handManager.ItemDropped += OnItemDropped;
	}

	public void RefreshInventory()
	{
		// Clear item bar
		foreach (ItemBarSlot slot in itemBar.itemBarSlots)
		{
			slot.SetItem(new Item());
		}

		// Increment 
		for (int i = 0; i < inventory.GetItemList().Count; i++)
		{
			Item item = inventory.GetItemList()[i];

			if ( i < 10)
			{
				itemBar.itemBarSlots[i].SetItem(item);
			}
			else
			{
				InventorySlot slot = inventorySlotList[i-10];
				slot.SetManager(this);
				slot.item = item;
				slot.RefreshSlot();
			}
		}
	}

	private void ShowInventory()
	{
		if (Input.GetButtonDown("Inventory"))
		{
			inventoryInterface.SetActive(!inventoryInterface.active);

			if (inventoryInterface.active)
			{
				Time.timeScale = 0;
				Cursor.lockState = CursorLockMode.None;
			}
			else
			{
				Time.timeScale = 1;
				Cursor.lockState = CursorLockMode.Locked;
			}
		}
	}

	public void ResumeGame()
	{
		Time.timeScale = 1;
	}

	private void GenerateSlots()
	{
		for (int i = 0; i < slotsCount; i++)
		{
			InventorySlot slot = Instantiate(inventorySlotPrefab.gameObject, inventorySlotContainer.transform).GetComponentInChildren<InventorySlot>();
			slot.SetManager(this);
			slot.slotId = i + 10;
			slot.item = new Item();
			inventorySlotList.Add(slot);
		}
	}


	#region Listeners
	private void OnItemCollected(Item item)
	{
		feedbackManager.ShowFeedback("+" + item.amount + " " + item.itemName);
		RefreshInventory();
		itemBar.RefreshItemBar();
	}

	private void OnItemDropped(Item item)
	{
		for (int i = 0; i < inventory.GetItemList().Count; i++)
		{
			Item itemList = inventory.GetItemList()[i];
			if (itemList.itemName.Equals(item.itemName))
			{
				if (item.amount <= 0)
				{
					inventory.GetItemList()[i] = new Item();
					break;
				}
			}
		}

		RefreshInventory();
		itemBar.RefreshItemBar();
	}
	#endregion

	#region Tools
	public void ToggleCursor()
	{
		if (Cursor.lockState == CursorLockMode.Locked)
		{
			Time.timeScale = 0;
			Cursor.lockState = CursorLockMode.None;
		} else
		{
			Time.timeScale = 1; 
			Cursor.lockState = CursorLockMode.Locked;
		}
	}
	#endregion

}
