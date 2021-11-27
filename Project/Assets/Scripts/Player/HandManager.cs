using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
	public delegate void DroppedItemHandler(Item item);
	public event DroppedItemHandler ItemDropped;

	public delegate void PlacedItemHandler(Item item);
	public event PlacedItemHandler ItemPlaced;

	public GameObject handItem;
	public Item handItemData;
	[Range(0.01f, 10)] public float dropForce;
	[Range(0.01f, 10)] public float dropCooldown;
	public Transform dropParent;


	private float dropTimer;

	private void Update()
	{
		dropTimer -= Time.deltaTime;
		if (dropTimer <= 0)
		{
			if (Input.GetButton("Drop"))
			{
				DropItem();
			}
		}
	}


	public void HoldItem(Item item)
	{
		if (handItem) handItem.SetActive(false);

		handItemData = item;
		if (!item.itemName.Equals(""))
		{
			if (handItemData.itemType == Item.ItemType.Furniture)
			{
				handItem = transform.Find("Furniture").gameObject;
			} else
            {
				handItem = transform.Find(item.itemName).gameObject;
			}
			handItem.SetActive(true);
		}
	}

	public void DropItem()
	{
		if (handItemData.amount > 0)
		{
			GameObject dropPrefab = null; 
			GameObject[] dropsList = Resources.LoadAll<GameObject>("DropsPrefabs");
			foreach (GameObject go in dropsList)
			{
				if (go.name.Equals(handItemData.itemName)) 
				{ 
					dropPrefab = go;
					break;
				}
			}

			GameObject drop = Instantiate(dropPrefab, transform.parent);
			drop.GetComponent<DropItem>().dropTimer = dropCooldown;
			drop.transform.position += transform.forward / 2;
			drop.transform.SetParent(dropParent);

			drop.transform.Rotate(UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360));
			drop.GetComponent<Rigidbody>().AddForce(transform.forward * dropForce, ForceMode.Impulse);
			drop.GetComponent<Rigidbody>().AddForce(Vector3.up * dropForce / 1.5f, ForceMode.Impulse);
			

			handItemData.amount -= 1;
			ItemDropped(handItemData);

			dropTimer = 0.2f;
		}
	}

	public void DropItem(Item item)
	{
		if (item.amount > 0)
		{
			GameObject dropPrefab = null;
			GameObject[] dropsList = Resources.LoadAll<GameObject>("DropsPrefabs");
			foreach (GameObject go in dropsList)
			{
				if (go.name.Equals(item.itemName))
				{
					dropPrefab = go;
					break;
				}
			}

			GameObject drop = Instantiate(dropPrefab, transform.parent);
			drop.GetComponent<DropItem>().dropTimer = dropCooldown;
			drop.transform.position += transform.forward / 2;
			drop.transform.SetParent(dropParent);

			drop.transform.Rotate(UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360));
			drop.GetComponent<Rigidbody>().AddForce(transform.forward * dropForce, ForceMode.Impulse);
			drop.GetComponent<Rigidbody>().AddForce(Vector3.up * dropForce / 1.5f, ForceMode.Impulse);


			item.amount -= 1;
			ItemDropped(item);

			dropTimer = 0.2f;
		}
	}

	public void PlaceItem()
    {
		handItemData.amount -= 1;
		ItemPlaced(handItemData);
	}
}
