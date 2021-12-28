using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestObject : SmartObject
{
    [Header("Chest")]
    public int itemSlots;
    public List<Item> itemList;
    

    public override void Interact()
    {
        FindObjectOfType<UIManager>().OpenChest(this);
    }

    public override bool CanInteract(GameObject obj)
	{
        return true;
	}

    public bool CanDestroy()
	{
        foreach(Item item in itemList)
		{
            if (item.amount > 0) return false;
		}

        return true;
	}

    public override ObjectType GetObjectType()
    {
        return ObjectType.Chest;
    }

	public override string GetInteractButton()
	{
        return "InteractButton";
	}

}
