using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftManager : MonoBehaviour
{
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

    // Start is called before the first frame update
    void Start()
    {
        itemSprite.sprite = null;
        itemNameText.text = "";
        itemDescriptionText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("OpenCrafts")) RefreshCrafts(ItemData.ItemType.Furniture);   
    }

    public void RefreshCrafts(ItemData.ItemType type)
	{
        // Limpar slots
        foreach (CraftSlot slot in craftSlots)
		{
            Destroy(slot.gameObject);
		}
        craftSlots = new List<CraftSlot>();

        // Carrega os itens
        ItemData[] itemDatas = Resources.LoadAll<ItemData>("ItemData");
        List<ItemData> crafts = new List<ItemData>();

        foreach (ItemData item in itemDatas)
        {
            if (item.itemType == type)
            {
                crafts.Add(item);
                CraftSlot slot = Instantiate(craftSlotPrefab, craftSlotsContainer).GetComponent<CraftSlot>();
                slot.itemData = item;
                slot.craftManager = this;
                craftSlots.Add(slot);
            }
        }

        // Verifica se pode craftar
        List<Item> inventory = GetComponent<InventoryManager>().inventory.itemList;
        for (int i = 0; i < crafts.Count; i++)
		{
            ItemData iData = crafts[i];
            int yes = 0; // corrects ingredients counter
            foreach (Item item in inventory)
            {
                foreach(IngredientItem ingredient in iData.recipe)
				{
                    if (item.itemData == ingredient.item && item.amount >= ingredient.amount)
					{
                        yes += 1;
					}
				}
            }

            if (yes >= iData.recipe.Count) craftSlots[i].SetCraftEnabled(true);
            else craftSlots[i].SetCraftEnabled(false);
        }
        craftSlots[0].SelectCraftSlot();
	}
}
