
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveObject
{
	public string id;
	public SaveTransform saveTransform;
	public List<SaveComponent> saveComponents = new List<SaveComponent>();

	public void Save(GameObject gameObject)
	{
		id = gameObject.GetComponent<SavableObject>().id;
		saveComponents.Clear();
		saveTransform = new SaveTransform(gameObject.transform);
		foreach (Component component in gameObject.GetComponents<Component>())
		{
			AddComponentToSaveObject(component);
		}
	}

	private void AddComponentToSaveObject(Component component)
	{
		if (component.GetType() == typeof(ChestObject))
		{
			saveComponents.Add(new SaveChestObject(component as ChestObject));
		}
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