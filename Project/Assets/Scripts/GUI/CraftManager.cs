using System.Collections;
using System.Linq;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CraftManager : MonoBehaviour
{
    public string currentType;

    [Header("Objects")]
    public Transform craftSlotsContainer;
    public List<CraftSlot> craftSlots;

    [Header("Prefabs")]
    public GameObject craftSlotPrefab;
    public GameObject ingredientPrefab;

    [Header("Item Sheet")]
    public Image itemSprite;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescriptionText;
    public Transform recipeContainer;
    public Button craftButton;

    [HideInInspector] public Item craftItem;

    // Start is called before the first frame update
    void Start()
    {
        currentType = "All";
        itemSprite.sprite = null;
        itemNameText.text = "";
        itemDescriptionText.text = "";
    }

    public void SelectCraftType(string type)
	{
        currentType = type;
        RefreshCrafts();
	}

    public void RefreshCrafts()
	{
        // Limpar slots
        foreach (CraftSlot slot in craftSlots)
		{
            Destroy(slot.gameObject);
		}
        craftSlots = new List<CraftSlot>();

        // Carrega os itens
        List<ItemData> itemDatas = new List<ItemData>();

        if (currentType.Equals("All"))
        {
            int itemTypesCount = Enum.GetNames(typeof(ItemData.ItemType)).Length;

            for (int i = 0; i < itemTypesCount; i++) 
            {
                List<ItemData> array = Resources.LoadAll<ItemData>("ItemData/" + (ItemData.ItemType)i).ToList();
                if (array.Count > 0)
                    foreach (ItemData item in array) itemDatas.Add(item);
            }
        }
        else
        {
            itemDatas = Resources.LoadAll<ItemData>("ItemData/" + currentType.ToString()).ToList();
        }

        List<ItemData> crafts = new List<ItemData>();

        foreach (ItemData item in itemDatas)
        {
            if (item.recipe.Count > 0)
            {
                crafts.Add(item);
                CraftSlot slot = Instantiate(craftSlotPrefab, craftSlotsContainer).GetComponent<CraftSlot>();
                slot.itemData = item;
                slot.craftManager = this;
                craftSlots.Add(slot);
            }
        }

        if (craftSlots.Count > 0)
        {
            // Verifica se pode craftar
            List<Item> inventory = GetComponent<InventoryManager>().inventory.itemList;
            for (int i = 0; i < crafts.Count; i++)
            {
                ItemData iData = crafts[i];
                int yes = 0; // corrects ingredients counter
                foreach (Item item in inventory)
                {
                    foreach (IngredientItem ingredient in iData.recipe)
                    {
                        if (item.itemData == ingredient.itemData && item.amount >= ingredient.amount)
                        {
                            yes += 1;
                        }
                    }
                }

                if (yes >= iData.recipe.Count) craftSlots[i].SetCraftEnabled(true);
                else craftSlots[i].SetCraftEnabled(false);
            }
        }
        craftSlots[0].SelectCraftSlot();
    }

    public void Craft()
	{
        foreach (IngredientItem ingredient in craftItem.itemData.recipe)
		{
            Item removeItem = new Item();
            removeItem.itemData = ingredient.itemData;
            removeItem.amount = ingredient.amount;

            GetComponent<InventoryManager>().inventory.RemoveItem(removeItem);
        }
        GetComponent<InventoryManager>().inventory.AddItem(craftItem);

        RefreshCrafts();
    }
}
