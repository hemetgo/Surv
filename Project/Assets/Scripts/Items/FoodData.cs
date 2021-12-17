using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Food", fileName = "New Food")]
public class FoodData : ItemData
{
    [Header("Food")]
    public FoodType foodType;
    public ItemEffect effect;

    public enum FoodType { Fruit, Meat }

    public void UseItem(GameObject player)
	{
        effect.UseEffect(player);
	}
}
