using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CaveLoot
{
    [Range(0, 100)]public float rarity;
    public GameObject prefab;
}
