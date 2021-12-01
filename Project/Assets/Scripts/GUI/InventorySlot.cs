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
    public TextMeshProUGUI txtName;
	public Sprite emptyImage;

	// aux
	private InventoryManager inventoryManager;
	private GameObject slotHolder;

	// Aux
    private bool isDraging;
	private Vector2 startPos;

	private void Start()
	{
		inventoryManager = FindObjectOfType<InventoryManager>();
	}

	private void Update()
	{
		if (isDraging)
		{
			slotHolder.transform.position = Input.mousePosition;
			//transform.position = Input.mousePosition;
		}
	}

	public void SetLabelActive(bool active)
	{
		if (!isDraging)
		{
			txtName.transform.parent.gameObject.SetActive(active);
		} else
		{
			txtName.transform.parent.gameObject.SetActive(false);
		}
	}

	public void PointerExit()
	{
		inventoryManager.dropSlot = null;
	}

	public void PointerEnter()
	{
		if (inventoryManager.dragSlot != null)
		{
			inventoryManager.dropSlot = this;
		}
	}

	public void PointerDown()
	{
		slotHolder = Instantiate(inventoryManager.slotHolderPrefab, transform);
		slotHolder.transform.SetParent(inventoryManager.transform.parent);
		slotHolder.transform.SetAsLastSibling();

		if (item.amount > 0)
		{
			itemImage.raycastTarget = false;
			slotHolder.GetComponent<Image>().raycastTarget = false;
			slotHolder.GetComponent<Image>().sprite = item.icon;

			startPos = GetComponent<RectTransform>().position;
			inventoryManager.dragSlot = this;
			txtAmount.gameObject.SetActive(false);
			itemImage.gameObject.SetActive(false);
			isDraging = true;
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
				if (item.itemName.Equals(dropSlot.item.itemName))
				{
					for (int i = 0; i < item.amount; i++)
					{
						if (dropSlot.item.amount < dropSlot.item.stackLimit)
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
			if (item.itemName.Equals("")) return true;
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
			txtName.gameObject.SetActive(true);

			itemImage.sprite = item.icon;
			txtAmount.text = "x" + item.amount;
			txtName.text = item.itemName;
		}
		else 
		{
			itemImage.sprite = emptyImage;
			txtAmount.gameObject.SetActive(false);
			txtAmount.text = "";
			txtName.gameObject.SetActive(false);
		}
	}


	public void SetManager(InventoryManager manager)
	{
		inventoryManager = manager;
	}
}