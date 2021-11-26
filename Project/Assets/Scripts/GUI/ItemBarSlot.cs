using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemBarSlot : MonoBehaviour
{
    public int slotId;
    [SerializeField]
    private Image itemSprite;
    [SerializeField]
    private TextMeshProUGUI amountText;
    [SerializeField]
    private Item itemData;

    public void StartSlot()
	{
        if (itemData != null) SetItem(itemData); 
	}

	public void SetItem(Item item)
	{
        itemData = item;
        ResetItemSlotGraphic();
    }

    private void ResetItemSlotGraphic()
	{
        if (itemData.amount <= 0)
        {
            itemSprite.gameObject.SetActive(false);
            amountText.gameObject.SetActive(false);
        }
        else if (itemData.amount > 1)
        {
            itemSprite.sprite = itemData.icon;
            amountText.text = "x" + itemData.amount;
            itemSprite.gameObject.SetActive(true);
            amountText.gameObject.SetActive(true);
        }
        else
        {
            itemSprite.sprite = itemData.icon;
            itemSprite.gameObject.SetActive(true);
            amountText.gameObject.SetActive(false);
        }
    }

    public Item GetItemData()
	{
        return itemData;
	}

    public void SetOutline(bool selected)
	{
        if (selected)
		{
            GetComponent<Outline>().enabled = true;
		}
		else 
        { 
            GetComponent<Outline>().enabled = false;
        }
    }
}
