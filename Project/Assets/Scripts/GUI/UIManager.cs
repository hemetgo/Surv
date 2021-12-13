using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
	public GameObject menusGUI;
	public GameObject inventoryGUI;
	public GameObject craftGUI;



	private void Start()
	{
		ResumeGame();
	}

	private void Update()
	{
		if (Input.GetButtonDown("OpenInventory"))
		{
			OpenMenus();
			OpenInventory();
		}

		if (Input.GetButtonDown("OpenCrafts"))
		{
			OpenMenus();
			OpenCrafts();
		}

		if (Input.GetKeyDown(KeyCode.Escape)) ResumeGame();
	}

	public void OpenMenus()
	{
		Cursor.lockState = CursorLockMode.None;
		menusGUI.SetActive(true);
	}
	public void ResumeGame()
	{
		Cursor.lockState = CursorLockMode.Locked;
		menusGUI.SetActive(false);
		inventoryGUI.SetActive(false);
		craftGUI.SetActive(false);
	}
	public void OpenInventory()
	{
		if (inventoryGUI.activeSelf)
		{
			ResumeGame();
			return;
		}

		inventoryGUI.SetActive(true);
		craftGUI.SetActive(false);
	}
	public void OpenCrafts()
	{
		if (craftGUI.activeSelf)
		{
			ResumeGame();
			return;
		}

		craftGUI.SetActive(true);
		inventoryGUI.SetActive(false);
		GetComponent<CraftManager>().currentType = "All";
		GetComponent<CraftManager>().RefreshCrafts(true);
	}

	public void Restart()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void QuitGame()
	{
		Application.Quit();
	}
}
