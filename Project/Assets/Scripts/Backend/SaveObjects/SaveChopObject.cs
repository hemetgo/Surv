using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class SaveChopObject : SaveComponent
{
    public int currentDamage;

    public SaveChopObject(ChopObject chop)
	{
        currentDamage = chop.currentDamage;
	}

    public override void LoadComponent(GameObject gameObject)
    {
        ChopObject component = gameObject.GetComponent<ChopObject>();
        component.currentDamage = currentDamage;
    }
}
