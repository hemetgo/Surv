using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBarManager : MonoBehaviour
{
    [SerializeField]
    private int selectedSlot = 0;
    public ItemBarSlot[] itemBarSlots;
	public HandManager handManager;

	private void Start()
	{
		foreach(ItemBarSlot slot in itemBarSlots)
		{
			slot.StartSlot();
		}

		SetCurrentSlot(selectedSlot);
	}

	private void Update()
	{
		if (Time.timeScale > 0)
		{
			if (Input.mouseScrollDelta.y > 0)
			{
				if (selectedSlot >= itemBarSlots.Length - 1)
				{
					SetCurrentSlot(0);
				} else
				{
					SetCurrentSlot(selectedSlot + 1);
				}
			} else if (Input.mouseScrollDelta.y < 0)
			{
				if (selectedSlot <= 0)
				{
					SetCurrentSlot(itemBarSlots.Length - 1);
				}
				else
				{
					SetCurrentSlot(selectedSlot - 1);
				}
			}
		}

		for (int i = 0; i < 10; i++)
		{
			if (Input.GetKeyDown("" + i))
			{
				if (i == 0) SetCurrentSlot(9);
				else SetCurrentSlot(i-1);
			}
		}
	}

	public void RefreshItemBar()
	{
		foreach (ItemBarSlot slot in itemBarSlots)
		{
			slot.StartSlot();
		}

		SetCurrentSlot(selectedSlot);
	}

	public void SetCurrentSlot(int selectedSlot)
	{
        this.selectedSlot = selectedSlot;
        foreach(ItemBarSlot slot in itemBarSlots)
		{
            slot.SetOutline(false);
		}
        itemBarSlots[selectedSlot].SetOutline(true);
		handManager.HoldItem(itemBarSlots[selectedSlot].GetItemData());
    }

}
