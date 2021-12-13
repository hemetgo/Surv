using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemBarManager : MonoBehaviour
{
    [SerializeField]
    private int selectedSlot = 0;
    public List<InventorySlot> itemBarSlots;
	public List<GameObject> barBackgroundSlot;
	[HideInInspector] public HandManager handManager;


	private void Update()
	{
		if (Cursor.lockState == CursorLockMode.Locked)
		{
			if (Input.mouseScrollDelta.y > 0)
			{
				if (selectedSlot >= itemBarSlots.Count - 1)
				{
					SetCurrentSlot(0);
				}
				else
				{
					SetCurrentSlot(selectedSlot + 1);
				}
			}
			else if (Input.mouseScrollDelta.y < 0)
			{
				if (selectedSlot <= 0)
				{
					SetCurrentSlot(itemBarSlots.Count - 1);
				}
				else
				{
					SetCurrentSlot(selectedSlot - 1);
				}
			}


			for (int i = 0; i < 10; i++)
			{
				if (Input.GetKeyDown("" + i))
				{
					if (i == 0) SetCurrentSlot(9);
					else SetCurrentSlot(i - 1);
				}
			}
		}
	}

	public void RefreshItemBar()
	{
		SetCurrentSlot(selectedSlot);
	}

	public void SetCurrentSlot(int selectedSlot)
	{
        this.selectedSlot = selectedSlot;
        for (int i = 0; i < 10; i++)
		{
			barBackgroundSlot[i].GetComponent<Outline>().enabled = false;
		}
		barBackgroundSlot[selectedSlot].GetComponent<Outline>().enabled = true;
		handManager.HoldItem(itemBarSlots[selectedSlot].item);
    }

}
