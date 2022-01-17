using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
	public void NewGame(string scene)
	{
		SceneManager.LoadScene(scene);
	}

    public void LoadGame()
	{

	}

	public void Quit()
	{
		Application.Quit();
	}
}
