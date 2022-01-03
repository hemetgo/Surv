using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Vector2 timeRange;
    public GameObject prefab;
    public Transform spawnPoint;

    private float spawnTimer;

    // Update is called once per frame
    void Update()
    {
        PlayerPrefs.Save();
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= Random.Range(timeRange.x, timeRange.y))
		{
            spawnTimer = 0;
            Instantiate(prefab, spawnPoint);
        }
    }
}
