using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftToolObject : SmartObject
{
    [Header("Craft")]
    public ItemData.CraftTool craftTool;
    

    public override void Interact()
    {
        FindObjectOfType<UIManager>().OpenCrafts(craftTool);
        FindObjectOfType<CraftManager>().currentCraftTool = craftTool;
    }

    public override bool CanInteract(GameObject obj)
	{
        return true;
	}

    public override ObjectType GetObjectType()
    {
        return ObjectType.CraftTool;
    }

	public override string GetInteractButton()
	{
        return "Interact";
	}

}
