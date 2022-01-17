
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveObject
{
	public string id;
	public SaveTransform saveTransform;
	public List<SaveComponent> saveComponents = new List<SaveComponent>();

	public void UpdateObject(GameObject gameObject)
	{
		id = gameObject.GetComponent<SavableObject>().id;
		saveComponents.Clear();
		saveTransform = new SaveTransform(gameObject.transform);
		foreach (Component component in gameObject.GetComponents<Component>())
		{
			AddComponentToList(component);
		}
	}

	private void AddComponentToList(Component component)
	{

	}

	public GameObject GetPrefab()
	{
		// Search in furnitures
		foreach(SavableObject prefab in Resources.LoadAll<SavableObject>("Objects/Furnitures/"))
		{
			if (prefab.id == id) return prefab.gameObject;
		}
		// Search in nature
		foreach (SavableObject prefab in Resources.LoadAll<SavableObject>("Objects/Nature/"))
		{
			if (prefab.id == id) return prefab.gameObject;
		}

		return null;
	}
}