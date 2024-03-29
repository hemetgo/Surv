using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
	[Header("Data")]
	public int slotId;
	public Item item;
	
	[Header("Graphics")]
	public Image itemImage;
    public TextMeshProUGUI txtAmount;
	public Sprite emptyImage;
	public Vector2 itemBarImageSize;

	[Header("Infos")]
	public GameObject durabilityData;
	public Image durabilityBar;
	public TextMeshProUGUI txtName;

	// aux
	private InventoryManager inventoryManager;
	private GameObject slotHolder;
    private bool isDraging;
	private Vector2 startPos;

	private void Start()
	{
		inventoryManager = FindObjectOfType<InventoryManager>();
		durabilityData.SetActive(false);
	}

	private void Update()
	{
		if (isDraging)
		{
			slotHolder.transform.position = Input.mousePosition;
			//transform.position = Input.mousePosition;
		}
	}


	public void PointerExit()
	{
		inventoryManager.dropSlot = null;
		txtName.gameObject.SetActive(false);
	}

	public void PointerEnter()
	{
		if (inventoryManager.dragSlot != null)
		{
			inventoryManager.dropSlot = this;
		}

		if (item.amount > 0)
		{
			txtName.gameObject.SetActive(true);
		}
	}

	public void PointerDown()
	{
		if (item.amount > 0)
		{
			UIManager manager = FindObjectOfType<UIManager>();
			if (manager.state == UIManager.MenuState.Inventory)
			{
				slotHolder = Instantiate(inventoryManager.slotHolderPrefab, transform);
				slotHolder.transform.SetParent(inventoryManager.transform.parent);
				slotHolder.transform.SetAsLastSibling();

				if (item.amount > 0)
				{
					itemImage.raycastTarget = false;
					slotHolder.GetComponent<Image>().raycastTarget = false;
					slotHolder.GetComponent<Image>().sprite = item.itemData.icon;

					startPos = GetComponent<RectTransform>().position;
					inventoryManager.dragSlot = this;
					txtAmount.gameObject.SetActive(false);
					itemImage.gameObject.SetActive(false);
					isDraging = true;
				}
			}
			else if (manager.state == UIManager.MenuState.Chest)
			{
				if (Input.GetButtonDown("Fire1"))
				{
					Item it = new Item(item);
					it.amount = 1;
					manager.GetComponent<ChestManager>().AddItem(it);

					item.amount -= 1;
					RefreshSlot();
					manager.GetComponent<ChestManager>().RefreshChest();
				}

				if (Input.GetButtonDown("Fire2"))
				{
					Item it = new Item(item);
					manager.GetComponent<ChestManager>().AddItem(it);

					item.amount = 0;
					RefreshSlot();
					manager.GetComponent<ChestManager>().RefreshChest();
				}
			}
		}
	}

	public void PointerUp()
	{
		InventorySlot dropSlot = inventoryManager.dropSlot;
		if (inventoryManager.isDropping)
		{
			FindObjectOfType<HandManager>().DropItemByIndex(slotId, inventoryManager.inventoryDropTime);
			itemImage.raycastTarget = true;
			txtAmount.gameObject.SetActive(true);
			inventoryManager.dropSlot = null;
			inventoryManager.dragSlot = null;
			isDraging = false;
			Destroy(slotHolder);
			RefreshSlot();
		}
		else
		{
			if (dropSlot != null)
			{
				if (item.itemData == dropSlot.item.itemData)
				{
					for (int i = 0; i < item.amount; i++)
					{
						if (dropSlot.item.amount < dropSlot.item.itemData.GetStackLimit())
						{
							dropSlot.item.amount += 1;
							item.amount -= 1;
						}
					}
				}
				else
				{
					Swap(this, inventoryManager.dropSlot);

				}
			}

			itemImage.raycastTarget = true;
			txtAmount.gameObject.SetActive(true);
			inventoryManager.dropSlot = null;
			inventoryManager.dragSlot = null;
			isDraging = false;

			inventoryManager.RefreshInventory();
			Destroy(slotHolder);
		}
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

	public void Swap(InventorySlot slot1, InventorySlot slot2)
	{
		itemImage.gameObject.SetActive(true);

		inventoryManager.inventory.GetItemList()[slot1.slotId] = slot2.item;
		inventoryManager.inventory.GetItemList()[slot2.slotId] = slot1.item;

		inventoryManager.RefreshInventory();
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


	public void SetManager(InventoryManager manager)
	{
		inventoryManager = manager;
	}

	public void SetAsItemBarSlot()
	{
		RectTransform rect = itemImage.GetComponent<RectTransform>();
		GetComponent<RectTransform>().sizeDelta = new Vector2(40, 40);
		rect.sizeDelta = new Vector2(35, 35);
		txtName.fontSize = 15;
	}
}