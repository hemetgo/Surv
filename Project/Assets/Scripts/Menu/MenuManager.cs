using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
	public string gameScene;
	public SaveSlot saveSlotPrefab;
	public Transform saveSlotParent;
	public InputField newGameField;
	public DeletePopup deletePopup;

	private void Start()
	{
		GenerateSaveSlots();
	}

	public void NewGame()
	{
		PlayerPrefs.SetInt("LoadGame", 0);
		SetCurrentSave(newGameField.text);
		SceneManager.LoadScene(gameScene);
	}

	public void LoadGame()
	{
		PlayerPrefs.SetInt("LoadGame", 1);
		SceneManager.LoadScene(gameScene);
	}

	public void SetCurrentSave(string sav)
	{
		PlayerPrefs.SetString("CurrentSave", "/saves/" + sav + ".sav");
	}

	public void Quit()
	{
		Application.Quit();
	}

	public FileInfo[] GetSaves()
	{
		string path = Application.persistentDataPath + "/saves/";
		if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
		}

		DirectoryInfo info = new DirectoryInfo(path);
		FileInfo[] fileInfo = info.GetFiles();

		return fileInfo;
	}

	private void GenerateSaveSlots()
	{
		List<SaveSlot> slots = new List<SaveSlot>();
		foreach (FileInfo sav in GetSaves())
		{
			SaveSlot slot = Instantiate(saveSlotPrefab, saveSlotParent).GetComponent<SaveSlot>();
			slot.SetSave(sav);
			slots.Add(slot);
		}

		slots[0].Select();
	}

}
