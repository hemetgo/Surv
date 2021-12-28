using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sapling : MonoBehaviour
{
    public float growTimer;
    public GameObject saplingPrefab;
    public GameObject grewPrefab;

    private bool grew;

    // Start is called before the first frame update
    void Start()
    {
        saplingPrefab.SetActive(true);
        grewPrefab.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        growTimer -= Time.deltaTime;

        if (growTimer <= 0 && !grew)
		{
            grew = true;
            saplingPrefab.SetActive(false);
            grewPrefab.SetActive(true);
        }
    }
}
