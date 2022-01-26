using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTerrain : MonoBehaviour
{
    public Vector2Int spawnTrees;
    public GameObject treePrefab;


    public void StarTerrain()
    {
        FindObjectOfType<FirstPersonController>().transform.position = GetComponent<Collider>().bounds.center + Vector3.up * 2;
        PlantTress();
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
            GameObject tree = Instantiate(treePrefab, spawnPos, new Quaternion());
            tree.transform.Rotate(0, Random.Range(0, 360), 0);
            tree.transform.SetParent(transform);
		}
	}
}
