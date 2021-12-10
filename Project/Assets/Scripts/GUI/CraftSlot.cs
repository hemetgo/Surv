using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftSlot : MonoBehaviour
{
    public ItemData itemData;
    public Image itemSprite;
    public bool isCraftable;
    public Color32 enabledColor;
    public Color32 disabledColor;
    [HideInInspector] public CraftManager craftManager;

    public void SetCraftEnabled(bool enabled)
	{
        itemSprite.sprite = itemData.icon;
        isCraftable = enabled;

        if (enabled)
		{
            GetComponent<Button>().targetGraphic.color = enabledColor;
            itemSprite.color = enabledColor;
        }
        else
		{
            GetComponent<Button>().targetGraphic.color = disabledColor;
            itemSprite.color = disabledColor;
        }
    }

    public void SelectCraftSlot()
    {
        craftManager.itemSprite.sprite = itemData.icon;
        craftManager.itemNameText.text = itemData.itemName.GetString();
        craftManager.itemDescriptionText.text = itemData.description.GetString();
        if (isCraftable) craftManager.craftButton.gameObject.SetActive(true);
        else craftManager.craftButton.gameObject.SetActive(false);

        foreach (Transform child in craftManager.recipeContainer.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (IngredientItem ingredient in itemData.recipe)
        {
            CraftIngredientSlot ingredientSlot = Instantiate(craftManager.ingredientPrefab, craftManager.recipeContainer).GetComponent<CraftIngredientSlot>();
            ingredientSlot.inventoryItemAmount = craftManager.GetComponent<InventoryManager>().inventory.GetInventoryAmount(ingredient.itemData);
            ingredientSlot.SetSlot(ingredient);
        }

        if (isCraftable)
        {
            Item craftItem = new Item();
            craftItem.itemData = itemData;
            craftItem.amount = 1;

            craftManager.craftItem = craftItem;
		}
		else
		{
            craftManager.craftItem = null;
		}
    }

}
