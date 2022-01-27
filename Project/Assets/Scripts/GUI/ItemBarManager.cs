using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemBarManager : MonoBehaviour
{
    [SerializeField]
    private int selectedSlot = 0;
	public bool invertScroll;
    public List<InventorySlot> itemBarSlots;
	public List<GameObject> barBackgroundSlot;
	[HideInInspector] public HandManager handManager;

	private void Update()
	{
		//if (Cursor.lockState == CursorLockMode.Locked)
		//{
		if (Input.mouseScrollDelta.y < 0)
		{
			if (invertScroll)
			{
				if (selectedSlot >= 9)
				{
					SetCurrentSlot(0);
				}
				else
				{
					SetCurrentSlot(selectedSlot + 1);
				}
			}
			else
			{
				if (selectedSlot <= 0)
				{
					SetCurrentSlot(9);
				}
				else
				{
					SetCurrentSlot(selectedSlot - 1);
				}
			}
		}
		else if (Input.mouseScrollDelta.y > 0)
		{
			if (invertScroll)
			{
				if (selectedSlot <= 0)
				{
					SetCurrentSlot(9);
				}
				else
				{
					SetCurrentSlot(selectedSlot - 1);
				}
			} 
			else
			{
				if (selectedSlot >= 9)
				{
					SetCurrentSlot(0);
				}
				else
				{
					SetCurrentSlot(selectedSlot + 1);
				}
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
		//}
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
			barBackgroundSlot[i].transform.localScale = new Vector3(1, 1, 1);
		}
		barBackgroundSlot[selectedSlot].GetComponent<RectTransform>().localScale = new Vector3(1.3f, 1.3f, 1);

		handManager.HoldItem(itemBarSlots[selectedSlot].item);
    }
}
