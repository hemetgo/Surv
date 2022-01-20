using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class SaveGasToolObject : SaveComponent
{
    public float currentGas;

    public SaveGasToolObject(GasToolObject gasTool)
	{
        currentGas = gasTool.currentGas;
	}

    public override void LoadComponent(GameObject gameObject)
    {
        GasToolObject component = gameObject.GetComponent<GasToolObject>();
        component.currentGas = currentGas;
    }
}
