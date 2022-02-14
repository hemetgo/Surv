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
    public ItemData.CraftTool currentCraftTool;
    public int toolLevel;
    [HideInInspector] public CraftToolObject craftTool;

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
    public int lastSelectedIndex;

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
        RefreshCrafts(true);
    }

    public void RefreshCrafts(bool reselect)
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
                    foreach (ItemData item in array) 
                        if (item.enable) 
                            itemDatas.Add(item);
            }
        }
        else
        {
            itemDatas = Resources.LoadAll<ItemData>("ItemData/" + currentType.ToString()).ToList();
        }

        List<ItemData> crafts = new List<ItemData>();

        int indexCount = 0;
        for (int i = 0; i < itemDatas.Count; i++)
        {
            ItemData item = itemDatas[i];
            if (item.recipe.Count > 0 && toolLevel >= item.toolLevel)
            {
                if (item.recipeTool == currentCraftTool)
                {
                    crafts.Add(item);
                    CraftSlot slot = Instantiate(craftSlotPrefab, craftSlotsContainer).GetComponent<CraftSlot>();
                    slot.itemData = item;
                    slot.craftManager = this;
                    craftSlots.Add(slot);

                    slot.index = indexCount;
                    indexCount += 1;
                }
            }
        }

        if (craftSlots.Count > 0)
        {
            Inventory inventory = GetComponent<InventoryManager>().inventory;
            // Verifica se pode craftar
            for (int i = 0; i < crafts.Count; i++)
            {
                ItemData item = crafts[i];
                int yes = 0;

                foreach (IngredientItem ingredient in item.recipe)
                {
                    if (inventory.GetInventoryAmount(ingredient.itemData) >= ingredient.amount)
                    {
                        yes += 1;
                    }
                }

                bool canCraft = true;
                if (currentCraftTool == ItemData.CraftTool.Forge)
                {
                    GasToolObject gasTool = craftTool as GasToolObject;
                    if (!gasTool.HaveGas(item.gasCost))
                    {
                        canCraft = false;
                    }
                }

                if (yes >= item.recipe.Count && canCraft) craftSlots[i].SetCraftEnabled(true);
                else craftSlots[i].SetCraftEnabled(false);
            }
        }
        if (craftSlots.Count > 0)
        {
            if (reselect) craftSlots[0]?.SelectCraftSlot();
            else craftSlots[lastSelectedIndex]?.SelectCraftSlot();
        }
    }

    public void Craft()
	{
        foreach (IngredientItem ingredient in craftItem.itemData.recipe)
		{
            Item removeItem = new Item(ingredient.itemData);
            removeItem.amount = ingredient.amount;

            GetComponent<InventoryManager>().inventory.RemoveItem(removeItem);
        }
        GetComponent<InventoryManager>().inventory.AddItem(craftItem);

        if (currentCraftTool == ItemData.CraftTool.Forge)
        {
            GasToolObject gasTool = craftTool as GasToolObject;
            gasTool.RemoveGas(craftItem.itemData.gasCost);
        }

        RefreshCrafts(false);
    }
}
