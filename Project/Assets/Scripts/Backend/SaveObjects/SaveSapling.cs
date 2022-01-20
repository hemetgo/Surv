using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class SaveSapling : SaveComponent
{
    public float growTimer;

    public SaveSapling(Sapling sapling)
	{
        growTimer = sapling.growTimer;
	}

    public override void LoadComponent(GameObject gameObject)
    {
        Sapling component = gameObject.GetComponent<Sapling>();
        component.growTimer = growTimer;
    }
}
