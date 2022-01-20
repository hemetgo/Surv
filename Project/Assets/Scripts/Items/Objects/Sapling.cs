using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sapling : MonoBehaviour
{
    public float growTimer;
    public GameObject saplingPrefab;
    public GameObject grewPrefab;

    // Start is called before the first frame update
    void Start()
    {
        saplingPrefab.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        growTimer -= Time.deltaTime;

        if (growTimer <= 0)
		{
            saplingPrefab.GetComponent<MeshRenderer>().enabled = false;
            GameObject grew = Instantiate(grewPrefab);
            grew.transform.position = transform.position;
            Destroy(gameObject);
        }
    }
}
