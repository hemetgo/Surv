using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftIngredientSlot : MonoBehaviour
{
	public IngredientItem ingredient;
	public Image itemImage;
	public Text itemAmountText;

	public void SetSlot(IngredientItem data)
	{
		ingredient = data;
		itemImage.sprite = ingredient.item.icon;
		itemAmountText.text = ingredient.amount.ToString();
	}
}
