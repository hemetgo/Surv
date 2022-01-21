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
	public GameObject sideBarGUI;
	public GameObject inventoryGUI;
	public GameObject craftGUI;
	public GameObject chestGUI;

	public enum MenuState { None, Inventory, Craft, Chest }

	private void Start()
	{
		CloseMenus();
	}

	private void Update()
	{
		if (Input.GetButtonDown("OpenInventory"))
		{
			OpenInventory();
		}

		if (Input.GetButtonDown("OpenCrafts"))
		{
			OpenCrafts(ItemData.CraftTool.None, 0, null);
		}

		if (Input.GetKeyDown(KeyCode.Escape)) CloseMenus();
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
		if(inventoryGUI.activeSelf)
		{
			CloseMenus();
			return;
		}

		sideBarGUI.SetActive(true);
		inventoryGUI.SetActive(true);
		craftGUI.SetActive(false);
		chestGUI.SetActive(false);
	}

	public void OpenCrafts(ItemData.CraftTool tool, int toolLevel, CraftToolObject craftTool)
	{
		CraftManager craft = GetComponent<CraftManager>();
		craft.currentCraftTool = tool;
		craft.toolLevel = toolLevel;
		craft.craftTool = craftTool;

		state = MenuState.Craft;
		OpenMenus();
		
		if(craftGUI.activeSelf)
		{
			CloseMenus();
			return;
		}

		sideBarGUI.SetActive(true);
		craftGUI.SetActive(true);
		inventoryGUI.SetActive(false);
		chestGUI.SetActive(false);
		craft.currentType = "All";
		craft.RefreshCrafts(true);
	}

	public void OpenCraftUI()
	{
		OpenCrafts(ItemData.CraftTool.None, 0, null);
	}

	public void OpenChest(ChestObject chest)
	{
		state = MenuState.Chest;
		OpenMenus();
		sideBarGUI.SetActive(false);
		inventoryGUI.SetActive(true);
		chestGUI.SetActive(true);
		craftGUI.SetActive(false);
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

	public void QuitGame()
	{
		Application.Quit();
	}
}
