using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemEffect
{
    public EffectType effectType;
	public int healPower;

    public enum EffectType { Heal }
   
    public void UseEffect(GameObject player)
	{
		switch (effectType)
		{
			case EffectType.Heal:
				player.GetComponent<HealthController>().Heal(healPower);
				break;
		}
	}

}
