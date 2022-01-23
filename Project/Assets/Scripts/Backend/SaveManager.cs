using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
	public GameObject savingGUI;
	public Text saveProgressText;
	private List<GameObject> prefabs = new List<GameObject>();
	private List<ItemData> itemDatas = new List<ItemData>();
	private SavableObject[] savableObjects;

	private void Start()
	{
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Backspace)) SaveGame();
		if (Input.GetKeyDown(KeyCode.Delete)) LoadGame();
	}

	public void SaveGame()
	{
		savingGUI.SetActive(true);

		SavableObject[] savableObjectList = FindObjectsOfType<SavableObject>();
		
		BinaryFormatter formatter = new BinaryFormatter();
		string path = Application.persistentDataPath + PlayerPrefs.GetString("CurrentSave");
		FileStream stream = File.Create(path);

		List<SaveObject> saveObjects = new List<SaveObject>();
		for(int i = 0; i < savableObjectList.Length; i++)
		{
			SavableObject save = savableObjectList[i];
			saveObjects.Add(save.GetSaveObject());

			saveProgressText.text = i / savableObjectList.Length * 100 + "%";
		}

		formatter.Serialize(stream, saveObjects);
		stream.Close();

		savingGUI.SetActive(false);
	}

	public void LoadGame()
	{
		savableObjects = FindObjectsOfType<SavableObject>();
		
		string path = Application.persistentDataPath + PlayerPrefs.GetString("CurrentSave");
		if (File.Exists(path))
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(path, FileMode.Open);

			List<SaveObject> saveObjects = formatter.Deserialize(stream) as List<SaveObject>;
			stream.Close();

			foreach(SaveObject saveObject in saveObjects)
			{
				if (saveObject.useSceneObject)
				{
					foreach(SavableObject sav in savableObjects)
					{
						
						if (sav.id == saveObject.id)
						{
							sav.LoadData(saveObject);
						}
					}
				}
				else
				{
					GameObject prefab = saveObject.GetPrefab();
					SavableObject gameObject = Instantiate(prefab).GetComponent<SavableObject>();
					gameObject.LoadData(saveObject);
				}
			}
		}
	}
}
