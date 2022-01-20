using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class SavableObject : MonoBehaviour
{
	public string id;
	public SaveObject saveObject = new SaveObject();

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

	private void Reset()
	{
		id = gameObject.name;
	}

	public void ReloadComponent()
	{
		id = gameObject.name;
	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(SavableObject))]
public class SavableObjectInspector : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		SavableObject script = (SavableObject)target;
		if (GUILayout.Button("Reload"))
		{
			script.ReloadComponent();
		}
	}
}
#endif