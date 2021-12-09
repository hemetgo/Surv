using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
	}

}
