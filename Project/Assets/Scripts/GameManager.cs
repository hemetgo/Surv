using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public SaveManager saveManager;
    public GameObject terrainPrefab;
    public GameObject gui;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("LoadGame") == 1)
        {
            saveManager.LoadGame();
        }
        else
        {
            NewGame();
        }
    }

    public void NewGame()
	{
        RandomTerrain terrain = Instantiate(terrainPrefab).GetComponent<RandomTerrain>();
        terrain.StarTerrain();
        gui.SetActive(true);
    }
}
