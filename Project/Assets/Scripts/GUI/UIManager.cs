using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
	public MenuState state;
	public GameObject menusGUI;
	public GameObject inventoryGUI;
	public GameObject craftGUI;
	public GameObject chestGUI;
	public GameObject superMenu;

	public enum MenuState { None, Inventory, Craft, Chest, Super }

	private void Start()
	{
		CloseMenus();
	}

	private void Update()
	{
		if (Input.GetButtonDown("OpenInventory"))
		{
			OpenInventory();
			ResetCrafts(ItemData.CraftTool.None, 0, null);
		}

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (superMenu.activeSelf)
			{
				CloseMenus();
				superMenu.SetActive(false);
			}
			else
			{
				OpenMenus();
				superMenu.SetActive(true);
			}
		}
	}

	public void OpenMenus()
	{
		PlayerPrefs.SetString("GameState", "Menu");
		Singleton.Instance.OpenMenu();
		Cursor.lockState = CursorLockMode.None;

		menusGUI.SetActive(true);
	}

	public void CloseMenus()
	{
		PlayerPrefs.SetString("GameState", "Resume");
		state = MenuState.None;
		Singleton.Instance.CloseMenu();
		Cursor.lockState = CursorLockMode.Locked;

		menusGUI.SetActive(false);
		inventoryGUI.SetActive(false);
		craftGUI.SetActive(false);
		chestGUI.SetActive(false);
	}

	public void OpenInventory()
	{
		state = MenuState.Inventory;
		OpenMenus();
		if (inventoryGUI.activeSelf)
		{
			CloseMenus();
			return;
		}

		inventoryGUI.SetActive(true);
		chestGUI.SetActive(false);
	}

	public void ResetCrafts(ItemData.CraftTool tool, int toolLevel, CraftToolObject craftTool)
	{
		craftGUI.SetActive(true);

		CraftManager craft = GetComponent<CraftManager>();
		craft.currentCraftTool = tool;
		craft.toolLevel = toolLevel;
		craft.craftTool = craftTool;
		//state = MenuState.Craft;
		
		craft.currentType = "All";
		craft.RefreshCrafts(true);
	}

	public void StartCraft(ItemData.CraftTool tool, int toolLevel, CraftToolObject craftTool)
	{
		OpenInventory();
		ResetCrafts(tool, toolLevel, craftTool);
	}

	public void OpenChest(ChestObject chest)
	{
		state = MenuState.Chest;
		OpenMenus();
		craftGUI.SetActive(false);
		inventoryGUI.SetActive(true);
		chestGUI.SetActive(true);
		GetComponent<ChestManager>().chest = chest;
		GetComponent<ChestManager>().RefreshChest();
	}

	public void Restart()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void SwitchScene(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}

	public void ResumeGame()
	{
		CloseMenus();
		superMenu.SetActive(false);
	}

	public void SaveGame()
	{
		GetComponent<SaveManager>().SaveGame();
	}

	public void QuitGame()
	{
		Application.Quit();
	}
}
