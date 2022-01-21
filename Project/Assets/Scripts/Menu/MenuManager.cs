using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
	public string currentSave;
	public string gameScene;

	public void NewGame()
	{
		PlayerPrefs.SetInt("LoadGame", 0);
		SetCurrentSave(currentSave);
		SceneManager.LoadScene(gameScene);
	}

	public void LoadGame()
	{
		PlayerPrefs.SetInt("LoadGame", 1);
		SetCurrentSave(currentSave);
		SceneManager.LoadScene(gameScene);
	}

	public void SetCurrentSave(string sav)
	{
		PlayerPrefs.SetString("CurrentSave", "/" + sav + ".fun");

	}

	public void Quit()
	{
		Application.Quit();
	}
}
