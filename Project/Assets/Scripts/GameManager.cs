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

        if (!dungeon)
        {
            if (PlayerPrefs.GetInt("LoadGame") == 1)
                saveManager.LoadGame();
            else
                NewGame();
        }
    }

    public void NewGame()
	{
        Debug.Log("NEW GAME");
        RandomTerrain terrain = Instantiate(terrainPrefab).GetComponent<RandomTerrain>();
        player = FindObjectOfType<FirstPersonController>();
        terrain.StarTerrain();
        gui.SetActive(true);

        saveManager.SaveGame();
        player.transform.Find("StartItems").gameObject.SetActive(true);
    }
}


