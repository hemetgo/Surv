using System.Collections.Generic;
using UnityEngine;

public class CaveHelper : MonoBehaviour
{
    public static CaveHelper instance;

    // cave settings

    public bool loadInventory;
    public int currentCaveLevel;
    public Inventory inventory;
    public int currentHP;
    public int currentItem;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(this);
    }

}