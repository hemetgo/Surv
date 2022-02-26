using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    public Item item;

    private Transform player;
    private bool followPlayer;
    private float dropForce = 3;

    [HideInInspector]
    public float dropTimer;

    private InventoryManager inventoryManager;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.layer = 6;
        inventoryManager = FindObjectOfType<InventoryManager>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        foreach(Collider col in GetComponents<Collider>())
		{
            Physics.IgnoreCollision(col, player.gameObject.GetComponent<Collider>());
        }

        // Add durability
        if (item.itemData.UseDurability())
		{
            item.durability = item.itemData.GetDurability();
		}
    }

    // Update is called once per frame
    void Update()
    {
        Collect();
    }

    private void Collect()
    {
        dropTimer -= Time.deltaTime;
        if (dropTimer <= 0 && !inventoryManager.inventory.IsFull())
        {
            float distance = Vector3.Distance(transform.position, player.position);
            if (distance < 3.5f)
            {
                followPlayer = true;
                Destroy(GetComponent<Rigidbody>());
            }

            if (followPlayer)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 15 * Time.deltaTime);
                if (distance < 0.5f)
                {
                    InventoryManager inv = FindObjectOfType<InventoryManager>();
                    inv.inventory.AddItem(item);

                    Destroy(gameObject);
                }
            }
        }
    }

	private void Reset()
	{
        if (!GetComponent<Rigidbody>()) gameObject.AddComponent<Rigidbody>();
        item = new Item();
        item.amount = 1;

        ItemData itemData;
        itemData = Resources.Load<ItemData>("ItemData/Decoration/" + gameObject.name);
        itemData = Resources.Load<ItemData>("ItemData/Battle/" + gameObject.name);
        itemData = Resources.Load<ItemData>("ItemData/Tool/" + gameObject.name);
        itemData = Resources.Load<ItemData>("ItemData/Decoration/" + gameObject.name);
        itemData = Resources.Load<ItemData>("ItemData/Material/" + gameObject.name);
        itemData = Resources.Load<ItemData>("ItemData/Consumable/" + gameObject.name);
        itemData = Resources.Load<ItemData>("ItemData/Miscellaneous/" + gameObject.name);

        item.itemData = itemData;
    }
}
