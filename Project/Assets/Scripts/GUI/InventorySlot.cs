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
	public RectTransform inventoryBackground;

	// aux
	private InventoryManager inventoryManager;

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
			transform.position = Input.mousePosition;
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
		startPos = GetComponent<RectTransform>().position;
		inventoryManager.dragSlot = this;
		txtAmount.gameObject.SetActive(false);
		isDraging = true;
	}

	public void PointerUp()
	{
		if (inventoryManager.dropSlot == null)
		{
			GetComponent<RectTransform>().position = startPos;
		}
		else
		{
			Swap(this, inventoryManager.dropSlot);
		}
		
		txtAmount.gameObject.SetActive(true);
		inventoryManager.dropSlot = null;
		inventoryManager.dragSlot = null;
		isDraging = false;
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
		inventoryManager.inventory.GetItemList()[slot1.slotId] = slot2.item;
		inventoryManager.inventory.GetItemList()[slot2.slotId] = slot1.item;

		Vector3 pos1 = slot1.GetComponent<RectTransform>().position;
		Vector3 pos2 = slot2.GetComponent<RectTransform>().position;

		slot1.GetComponent<RectTransform>().position = pos2;
		slot2.GetComponent<RectTransform>().position = pos1;

		inventoryManager.RefreshInventory();
	}

	public void RefreshSlot()
	{
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
			itemImage.gameObject.SetActive(false);
			txtAmount.gameObject.SetActive(false);
			txtName.gameObject.SetActive(false);
		}
	}


	public void SetManager(InventoryManager manager)
	{
		inventoryManager = manager;
	}
}