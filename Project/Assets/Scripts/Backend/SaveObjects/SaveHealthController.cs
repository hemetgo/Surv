using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class SaveHealthController : SaveComponent
{
    public int currentHp;

    public SaveHealthController(HealthController healthController)
	{
        currentHp = healthController.currentHp;
	}

    public override void LoadComponent(GameObject gameObject)
    {
        HealthController component = gameObject.GetComponent<HealthController>();
        component.currentHp = currentHp;
    }
}
