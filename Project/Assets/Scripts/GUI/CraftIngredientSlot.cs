using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftIngredientSlot : MonoBehaviour
{
	public IngredientItem ingredient;
	public Image itemImage;
	public Text itemAmountText;
	public int inventoryItemAmount;

	public void SetSlot(IngredientItem data)
	{
		ingredient = data;
		itemImage.sprite = ingredient.itemData.icon;
		itemAmountText.text = inventoryItemAmount + "/" + ingredient.amount;
	}
}
