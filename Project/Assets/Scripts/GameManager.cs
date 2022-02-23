using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public SaveManager saveManager;
    public InventoryManager inventoryManager;
    public GameObject terrainPrefab;
    public GameObject gui;
    public FirstPersonController player;

    public bool dungeon;

    public List<Item> startItems = new List<Item>();

    // Start is called before the first frame update
    void Start()
    {
        // it starts here to prevent eventually bugs
        inventoryManager.StartInventory();
        player = FindObjectOfType<FirstPersonController>();

        if (!dungeon)
        {
            if (PlayerPrefs.GetInt("LoadGame") == 1)
            {
                Destroy(player.transform.Find("StartItems").gameObject);
                saveManager.LoadGame();
            }
            else
            {
                NewGame();
            }
        }


        StartCaveHelper();
    }

    public void NewGame()
	{
        RandomTerrain terrain = Instantiate(terrainPrefab).GetComponent<RandomTerrain>();
        
        terrain.StarTerrain();
        gui.SetActive(true);

        saveManager.SaveGame();
        player.transform.Find("StartItems").gameObject.SetActive(true);

        PlayerPrefs.SetInt("LoadGame", 1);
    }

    private void StartCaveHelper()
	{
        if (CaveHelper.instance.loadInventory)
        {
            InventoryManager inv = FindObjectOfType<InventoryManager>();
            inv.inventory.itemList = CaveHelper.instance.inventory.itemList;
            inv.RefreshInventory();
            FindObjectOfType<HealthController>().currentHp = CaveHelper.instance.currentHP;
            FindObjectOfType<ItemBarManager>().SetCurrentSlot(CaveHelper.instance.currentItem);
        }
    }

}


