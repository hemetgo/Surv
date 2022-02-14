using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class SavableObject : MonoBehaviour
{
	//public SaveData saveData;
	//public SaveData.SaveObjectType type;
	//public string id;
	public SaveObject saveObject = new SaveObject();

	private void Start()
	{
		gameObject.name = gameObject.name.Replace("(Clone)", "");
	}

	public SaveObject GetSaveObject()
	{
		saveObject.Save(gameObject);
		return saveObject;
	}

	public void LoadData(SaveObject dataObject)
	{
		saveObject = dataObject;
		LoadTransform(dataObject.saveTransform);
		foreach(SaveComponent component in saveObject.saveComponents)
		{
			component.LoadComponent(gameObject);
		}
	}

	private void LoadTransform(SaveTransform transformData)
	{
		transform.position = transformData.GetPosition();
		transform.rotation = transformData.GetRotation();
	}

	//public void Reload()
	//{
	//	if (Resources.Load<SaveData>("SaveData/Decoration/") != null)
	//	{
	//		saveData = Resources.Load<SaveData>("SaveData/Decoration/");
	//		return;
	//	}
	//	if (Resources.Load<SaveData>("SaveData/Mob/") != null)
	//	{
	//		saveData = Resources.Load<SaveData>("SaveData/Mob/");
	//		return;
	//	}
	//	if (Resources.Load<SaveData>("SaveData/Nature/") != null)
	//	{
	//		saveData = Resources.Load<SaveData>("SaveData/Nature/");
	//		return;
	//	}
	//}


	//public void GenerateSaveData()
	//{
	//	SaveData asset = ScriptableObject.CreateInstance<SaveData>();

	//	AssetDatabase.CreateAsset(asset, "Assets/Resources/SaveData/"+ type.ToString() + "/" + gameObject.name +".asset");
	//	asset.id = gameObject.name;
	//	asset.prefab = gameObject;
	//	AssetDatabase.SaveAssets();

	//	//EditorUtility.FocusProjectWindow();

	//	//Selection.activeObject = asset;
	//}

	//public void GenerateAllSaveData()
	//{
	//	foreach(SavableObject sav in Resources.LoadAll<SavableObject>("Objects/Decoration/"))
	//	{
	//		SaveData asset = ScriptableObject.CreateInstance<SaveData>();

	//		AssetDatabase.CreateAsset(asset, "Assets/Resources/SaveData/" + sav.type.ToString() + "/" + sav.gameObject.name + ".asset");
	//		asset.id = gameObject.name;
	//		asset.prefab = gameObject;
	//		sav.saveData = asset;
	//		AssetDatabase.SaveAssets();
	//	}

	//	foreach (SavableObject sav in Resources.LoadAll<SavableObject>("Objects/Mob/"))
	//	{
	//		SaveData asset = ScriptableObject.CreateInstance<SaveData>();

	//		AssetDatabase.CreateAsset(asset, "Assets/Resources/SaveData/" + sav.type.ToString() + "/" + sav.gameObject.name + ".asset");
	//		asset.id = gameObject.name;
	//		asset.prefab = gameObject;
	//		sav.saveData = asset;
	//		AssetDatabase.SaveAssets();
	//	}
	//	foreach (SavableObject sav in Resources.LoadAll<SavableObject>("Objects/Nature/"))
	//	{
	//		SaveData asset = ScriptableObject.CreateInstance<SaveData>();

	//		AssetDatabase.CreateAsset(asset, "Assets/Resources/SaveData/" + sav.type.ToString() + "/" + sav.gameObject.name + ".asset");
	//		asset.id = gameObject.name;
	//		asset.prefab = gameObject;
	//		sav.saveData = asset;
	//		AssetDatabase.SaveAssets();
	//	}
	//}
}

#if UNITY_EDITOR
[CustomEditor(typeof(SavableObject))]
public class SavableObjectInspector : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		//SavableObject script = (SavableObject)target;
		//if (GUILayout.Button("Reload"))
		//{
		//	script.Reload();
		//}
		//if (GUILayout.Button("Generate Save Data"))
		//{
		//	script.GenerateSaveData();
		//}
		//if (GUILayout.Button("Generate All Save Data"))
		//{
		//	script.GenerateAllSaveData();
		//}
	}
}
#endif