using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SavableObject : MonoBehaviour
{
	public string id;
	public SaveObject saveObject = new SaveObject();

	public SaveObject GetSaveObject()
	{
		saveObject.UpdateObject(gameObject);
		return saveObject;
	}

	public void LoadData(SaveObject dataObject)
	{
		saveObject = dataObject;
		LoadTransform(dataObject.saveTransform);
	}

	private void LoadTransform(SaveTransform transformData)
	{
		transform.position = transformData.GetPosition();
		transform.rotation = transformData.GetRotation();
	}
}
