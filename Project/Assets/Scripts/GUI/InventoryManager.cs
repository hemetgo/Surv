using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
	[Header("Settings")]
	public int inventorySize;
	public float inventoryDropTime;

	[Header("Game Objects")]
	public Transform inventorySlotContainer;
	public Transform itemBarContainer;

	[Header("Item Bar")]
	public ItemBarManager itemBar; 

	[Header("Feedbacks")]
	public FeedbackManager feedbackManager;

	[Header("Prefabs")]
	public GameObject inventorySlotPrefab;
	public GameObject slotHolderPrefab;

	[Header("Data")]
	public Inventory inventory;
	[HideInInspector] public List<InventorySlot> inventorySlotList;

	// Aux
	public bool isDropping;
	[HideInInspector] public InventorySlot dragSlot = null;
	[HideInInspector] public InventorySlot dropSlot = null;

	private void Start()
	{
		StartInventory();
		GenerateSlots();
	}

	public void StartInventory()
	{
		inventory = new Inventory(inventorySize);
		inventory.ItemCollected += OnItemCollected;
		itemBar.handManager.ItemDropped += OnItemDropped;
		FindObjectOfType<HandManager>().ItemPlaced += OnItemPlaced;
	}

	public void RefreshInventory()
	{
		// Increment item bar and inventory
		for (int i = 0; i < inventorySize; i++)
		{
			Item item = inventory.GetItemList()[i];

			InventorySlot slot = inventorySlotList[i];
			slot.SetManager(this);
			slot.item = item;
			item.inventoryIndex = i;
			slot.RefreshSlot();
		}

		itemBar.RefreshItemBar();
	}

	public void DropItem(Item item)
    {
		FindObjectOfType<HandManager>().DropItem(item);
    }

	private void GenerateSlots()
	{
		for (int i = 0; i < inventorySize; i++)
		{
			InventorySlot slot;
			if(i < 10)
			{
				slot = Instantiate(inventorySlotPrefab.gameObject, itemBarContainer.transform).GetComponentInChildren<InventorySlot>();
			}
			else
			{
				slot = Instantiate(inventorySlotPrefab.gameObject, inventorySlotContainer.transform).GetComponentInChildren<InventorySlot>();
			}

			if (i < 10) itemBar.itemBarSlots.Add(slot);

			slot.SetManager(this);
			slot.slotId = i;
			slot.item = new Item();
			inventorySlotList.Add(slot);
		}

		itemBar.gameObject.SetActive(true);
	}

	public void SetDropping(bool isDropping)
	{
		Debug.Log("Drop? " + isDropping);
		this.isDropping = isDropping;
	}

	#region Listeners
	private void OnItemCollected(Item item)
	{
		feedbackManager.ShowFeedback("+" + item.amount + " " + item.itemData.GetItemName());
		RefreshInventory();
		itemBar.RefreshItemBar();
	}

	private void OnItemDropped(Item item)
	{
		if (inventory.GetItemList()[item.GetInventoryIndex()].amount <= 0)
		{
			inventory.GetItemList()[item.GetInventoryIndex()] = new Item();
		}
		else
		{
			inventory.GetItemList()[item.GetInventoryIndex()] = item;
		}

		RefreshInventory();
		itemBar.RefreshItemBar();
	}

	private void OnItemPlaced(Item item)
    {
		if (inventory.GetItemList()[item.GetInventoryIndex()].amount <= 0)
		{
			inventory.GetItemList()[item.GetInventoryIndex()] = new Item();
		} 
		else
        {
			inventory.GetItemList()[item.GetInventoryIndex()] = item;
		}

		RefreshInventory();
		itemBar.RefreshItemBar();
	}
	#endregion

	
}
