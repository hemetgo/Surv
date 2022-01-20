
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveObject
{
	public string id;
	public bool useSceneObject;
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
		else if (component.GetType() == typeof(ChopObject))
		{
			saveComponents.Add(new SaveChopObject(component as ChopObject));
		}
		else if (component.GetType() == typeof(GasToolObject))
		{
			saveComponents.Add(new SaveGasToolObject(component as GasToolObject));
		}
		else if (component.GetType() == typeof(Sapling))
		{
			saveComponents.Add(new SaveSapling(component as Sapling));
		}
		else if (component.GetType() == typeof(InventoryManager))
		{
			useSceneObject = true;
			saveComponents.Add(new SaveInventory(component as InventoryManager));
		}
		else if (component.GetType() == typeof(HealthController))
		{
			useSceneObject = true;
			saveComponents.Add(new SaveHealthController(component as HealthController));
		}
		else if (component.GetType() == typeof(BattleMobController))
		{
			saveComponents.Add(new SaveBattleMobController(component as BattleMobController));
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
		// Search in mobs
		foreach (SavableObject prefab in Resources.LoadAll<SavableObject>("Objects/Mobs/"))
		{
			if (prefab.id == id) return prefab.gameObject;
		}

		return null;
	}
}