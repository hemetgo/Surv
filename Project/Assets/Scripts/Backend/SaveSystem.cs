using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
	private List<GameObject> prefabs = new List<GameObject>();
	private List<ItemData> itemDatas = new List<ItemData>();

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Backspace)) SaveGame();
		if (Input.GetKeyDown(KeyCode.Delete)) LoadGame();
	}

	public void SaveGame()
	{
		SavableObject[] savableObjectList = FindObjectsOfType<SavableObject>();
		
		BinaryFormatter formatter = new BinaryFormatter();
		string path = Application.persistentDataPath + "/SaveGame.fun";
		FileStream stream = File.Create(path);

		List<SaveObject> saveObjects = new List<SaveObject>();
		foreach(SavableObject save in savableObjectList)
		{
			saveObjects.Add(save.GetSaveObject());
		}

		formatter.Serialize(stream, saveObjects);
		stream.Close();
	}

	public void LoadGame()
	{
		string path = Application.persistentDataPath + "/SaveGame.fun";
		if (File.Exists(path))
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(path, FileMode.Open);

			List<SaveObject> saveObjects = formatter.Deserialize(stream) as List<SaveObject>;
			stream.Close();

			foreach(SaveObject saveObject in saveObjects)
			{
				GameObject prefab = saveObject.GetPrefab();
				SavableObject gameObject = Instantiate(prefab).GetComponent<SavableObject>();
				gameObject.LoadData(saveObject);
			}
			
		}
	}
}
