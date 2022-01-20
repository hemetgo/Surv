using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class SaveBattleMobController : SaveComponent
{
    public int currentHp;

    public SaveBattleMobController(BattleMobController battleMobController)
	{
        currentHp = battleMobController.currentHp;
	}

    public override void LoadComponent(GameObject gameObject)
    {
        BattleMobController component = gameObject.GetComponent<BattleMobController>();
        component.currentHp = currentHp;
    }
}
