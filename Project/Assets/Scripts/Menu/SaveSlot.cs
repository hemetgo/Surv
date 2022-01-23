using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    public string save;
    public Text saveNameText;
    public Text dateText;
    public Color selectedColor;
    public Color unselectedColor;

	public void SetSave(FileInfo sav)
	{
        save = sav.Name.Replace(".sav", "");
        saveNameText.text = save;
        dateText.text = sav.LastAccessTime.ToString("dd/MM/yyyy HH:mm");
    }

    public void Select()
	{
        PlayerPrefs.SetInt("LoadGame", 1);
        SetCurrentSave(save);

        foreach (SaveSlot slot in FindObjectsOfType<SaveSlot>())
		{
            slot.Unselect();
		}

        GetComponent<Image>().color = selectedColor;
        saveNameText.color = unselectedColor;
        dateText.color = unselectedColor;
    }

    public void Unselect()
	{
        GetComponent<Image>().color = unselectedColor;
        saveNameText.color = selectedColor;
        dateText.color = selectedColor;
    }

    public void SetCurrentSave(string sav)
    {
        PlayerPrefs.SetString("CurrentSave", "/saves/" + sav + ".sav");
    }
}
