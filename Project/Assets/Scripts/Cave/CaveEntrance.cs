using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CaveEntrance : MonoBehaviour
{
    public string caveName;
	public string entranceScene;
	public bool saveGame;
	public bool loadInventory;
	public EntranceType entranceType;
	// cave settings

	private SaveManager saveManager;

	public enum EntranceType { Enter, Down, Quit }

	private void Start()
	{
		saveManager = FindObjectOfType<SaveManager>(); 	
	}

	public void EnterTheCave()
	{
		if (entranceType == EntranceType.Enter)
		{
			if (saveGame) saveManager.SaveGame();
			CaveHelper.instance.currentCaveLevel = 0;
		} 
		else if (entranceType == EntranceType.Down)
		{
			CaveHelper.instance.currentCaveLevel += 1;
		}
		else if (entranceType == EntranceType.Quit)
		{
			CaveHelper.instance.currentCaveLevel = 0;
		}


		CaveHelper.instance.currentHP = FindObjectOfType<HealthController>().currentHp;
		CaveHelper.instance.currentItem = FindObjectOfType<ItemBarManager>().selectedSlot;
		CaveHelper.instance.loadInventory = loadInventory;
		CaveHelper.instance.inventory = FindObjectOfType<InventoryManager>().inventory;
		SceneManager.LoadScene(entranceScene);
	}

}