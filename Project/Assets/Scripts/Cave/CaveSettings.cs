using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Cave Settings", fileName = "New Cave Settings")]
public class CaveSettings : ScriptableObject
{
	[Header("Cave Settings")]
	public float tileSize;
	public Vector2Int roomCountRange;
	public Vector2Int roomSizeRange;
	public int gridSize;
	
	[Header("Ground Prefabs")]
	public GameObject[] groundTilePrefab;
	public GameObject[] groundStairPrefab;
	
	[Header("Roof Prefabs")]
	public GameObject[] stairRoofPrefab;
	public GameObject[] caveRoofTile;

	[Header("Wall Prefabs")]
	public GameObject[] wallTilePrefab;
	public GameObject[] doorTilePrefab;

	[Header("Prefabs")]
	public GameObject[] stairPrefab;
	public GameObject[] mobsPrefabs;
	public CaveLoot[] lootsPrefabs;

	public GameObject GetRandomLoot()
	{
		GameObject loot = null;

		float random = Random.Range(0, 101);

		List<CaveLoot> loots = new List<CaveLoot>();
		foreach(CaveLoot l in lootsPrefabs)
		{
			if (CaveHelper.instance.currentCaveLevel >= l.firstLevel)
			{
				loots.Add(l);
			}
		}

		loots = loots.OrderBy(r => r.rarity).ToList();

		foreach (CaveLoot l in loots)
		{
			if (random  <= l.rarity)
			{
				loot = l.prefab;
				break;
			}
		}

		return loot;
	}
}
