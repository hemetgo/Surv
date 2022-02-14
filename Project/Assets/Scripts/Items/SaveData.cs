using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "SaveGame/SavaData", fileName = "New Save Data")]
public class SaveData : ScriptableObject
{
    public string id;
    public SaveObjectType type;
    public GameObject prefab;

    public enum SaveObjectType { Decoration, Mob, Nature }

 //   public GameObject GetPrefab()
	//{
	//	Debug.Log("a");
 //       return Resources.Load<SavableObject>("/Objects/" + type.ToString()).gameObject;
	//}

	//public void LoadPrefab()
	//{
	//	foreach (SavableObject sav in Resources.LoadAll<SavableObject>("Objects/" + type.ToString()))
	//	{
	//		//if (sav.gameObject.name == ) prefab = sav.gameObject;
	//	}
	//}

	//public void LoadAllPrefab()
	//{
	//	foreach (SavableObject sav in Resources.LoadAll<SavableObject>("Objects/Decoration"))
	//	{
	//		//if (sav.saveData == this) prefab = sav.gameObject;
	//	}

	//	foreach (SavableObject sav in Resources.LoadAll<SavableObject>("Objects/Mob"))
	//	{
	//		//if (sav.saveData == this) prefab = sav.gameObject;
	//	}
	//	foreach (SavableObject sav in Resources.LoadAll<SavableObject>("Objects/Nature"))
	//	{
	//		//if (sav.saveData == this) prefab = sav.gameObject;
	//	}
	//}
}

//#if UNITY_EDITOR
//[CustomEditor(typeof(SavableObject))]
//public class SaveDataInspector : Editor
//{
//	public override void OnInspectorGUI()
//	{
//		DrawDefaultInspector();

//		SaveData script = (SaveData)target;
//		if (GUILayout.Button("Load Prefab"))
//		{
//			script.LoadPrefab();
//		}
//		if (GUILayout.Button("Load all prefabs"))
//		{
//			script.LoadAllPrefab();
//		}
//	}
//}
//#endif