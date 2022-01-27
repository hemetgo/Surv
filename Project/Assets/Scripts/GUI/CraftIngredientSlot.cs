using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftIngredientSlot : MonoBehaviour
{
	public IngredientItem ingredient;
	public Image itemImage;
	public TextMeshProUGUI itemAmountText;
	public TextMeshProUGUI itemNameText;
	public int inventoryItemAmount;

	public void SetSlot(IngredientItem data)
	{
		ingredient = data;
		itemImage.sprite = ingredient.itemData.icon;
		itemAmountText.text = inventoryItemAmount + "/" + ingredient.amount;
		itemNameText.text = ingredient.itemData.GetItemName();
	}
}
