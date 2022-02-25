using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTerrain : MonoBehaviour
{
    public Vector2Int spawnTrees;
    public GameObject[] treePrefabs;
    public GameObject[] cavePrefabs;


    public void StarTerrain()
    {
        FindObjectOfType<FirstPersonController>().transform.position = GetComponent<Collider>().bounds.center + Vector3.up * 2;
        PlantTress();
        BuildCaves();
    }

    private void PlantTress()
	{
        for (int i = 0; i < Random.Range(spawnTrees.x, spawnTrees.y); i++)
		{
            Vector3 startTerrain = transform.position - GetComponent<Collider>().bounds.size / 2;
            Vector3 endTerrain = transform.position + GetComponent<Collider>().bounds.size / 2;
            Vector3 spawnPos = new Vector3(
                Random.Range(startTerrain.x, endTerrain.x),
                transform.position.y,
                Random.Range(startTerrain.z, endTerrain.z));
            GameObject tree = Instantiate(treePrefabs[Random.Range(0, treePrefabs.Length)], spawnPos, new Quaternion());
            tree.transform.Rotate(0, Random.Range(0, 360), 0);
            tree.transform.SetParent(transform.parent);
		}
	}

    private void BuildCaves()
	{
        for (int i = 0; i < Random.Range(5, 10); i++)
        {
            Vector3 startTerrain = transform.position - GetComponent<Collider>().bounds.size / 2;
            Vector3 endTerrain = transform.position + GetComponent<Collider>().bounds.size / 2;
            Vector3 spawnPos = new Vector3(
                Random.Range(startTerrain.x, endTerrain.x),
                transform.position.y,
                Random.Range(startTerrain.z, endTerrain.z));
            GameObject tree = Instantiate(cavePrefabs[Random.Range(0, cavePrefabs.Length)], spawnPos, new Quaternion());
            tree.transform.Rotate(0, Random.Range(0, 360), 0);
            tree.transform.SetParent(transform.parent);
        }
    }
}
