using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapon", fileName = "New Weapon")]
public class WeaponData : ItemData
{
    [Header("Weapon")]
    public int durability;

    public void UseItem(int cost)
	{
		durability -= cost;

        if (durability <= 0)
		{
            BreakWeapon();
		}
	}

    private void BreakWeapon()
	{
        Debug.Log("Broken");
	}

}
