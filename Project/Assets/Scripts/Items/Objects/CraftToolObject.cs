using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftToolObject : SmartObject
{
    [Header("Craft")]
    public ItemData.CraftTool craftTool;
    public int toolLevel;

	public override void Interact()
    {
        FindObjectOfType<UIManager>().StartCraft(craftTool, toolLevel, this);
  //      FindObjectOfType<CraftManager>().currentCraftTool = craftTool;
		//FindObjectOfType<CraftManager>().toolLevel = toolLevel;
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

	private void Reset()
	{
		if (!GetComponent<DecorationObject>()) gameObject.AddComponent<DecorationObject>();
	}
}
