using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawnObject : MonoBehaviour
{
    public bool randomYRot;
    public float maxScaleVariation;

    // Start is called before the first frame update
    void Start()
    {
        // rand
        transform.Rotate(0, Random.Range(0, 360), 0);
        transform.localScale = transform.localScale * (1 + Random.Range(-maxScaleVariation, maxScaleVariation));
    }
}
