using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
	public delegate void DroppedItemHandler(Item item);
	public event DroppedItemHandler ItemDropped;

	public delegate void DeletedItemHandler(Item item);
	public event DeletedItemHandler ItemDeleted;

	public delegate void PlacedItemHandler(Item item);
	public event PlacedItemHandler ItemPlaced;

	public GameObject handItemObject = null;
	public GameObject secondHandItemObject = null;
	public Item handItem;
	public Item secondHandItem;
	[Range(0.01f, 10)] public float dropForce;
	[Range(0.01f, 10)] public float dropCooldown;
	public Transform dropParent;

	public Transform secondHand;

	private float dropTimer;
	[HideInInspector] public Animator animator;

	private void Start()
	{

		animator = GetComponent<Animator>();
	}

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
		if (handItem == null)
		{
			handItem = new Item(Resources.Load<ItemData>("ItemData/_Empty"));
		}

		if (handItemObject) handItemObject.SetActive(false);

		handItem = item;
		if (item.amount > 0)
		{
			if (!item.itemData.itemName.english.Equals(""))
			{
				if (transform.Find(item.itemData.itemType.ToString()).Find(item.itemData.itemName.english))
				{
					if (handItem.itemData.itemType == ItemData.ItemType.Decoration)
					{
						handItemObject = transform.Find("Decoration").gameObject;
					}
					else
					{
						handItemObject = transform.Find(item.itemData.itemType.ToString()).Find(item.itemData.itemName.english).gameObject;
					}
					handItemObject.SetActive(true);
				}
			}
		}
	}

	public void HoldItemSecondHand(Item item)
	{
		if (secondHandItem == null)
		{
			secondHandItem = new Item(Resources.Load<ItemData>("ItemData/_Empty"));
		}

		if (secondHandItemObject) secondHandItemObject.SetActive(false);

		secondHandItem = item;
		if (item.amount > 0)
		{
			if (!item.itemData.itemName.english.Equals(""))
			{
				if (secondHand.Find(item.itemData.itemType.ToString()).Find(item.itemData.itemName.english))
				{
					if (secondHandItem.itemData.itemType == ItemData.ItemType.Decoration)
					{
						secondHandItemObject = secondHand.transform.Find("Furniture").gameObject;
					}
					else
					{
						secondHandItemObject = secondHand.transform.Find(item.itemData.itemType.ToString()).Find(item.itemData.itemName.english).gameObject;
					}
					secondHandItemObject.SetActive(true);
				}
			}
		}
	}

	public void DropItem()
	{
		if (handItem.amount > 0)
		{
			GameObject drop = Instantiate(handItem.itemData.drop, transform.parent);
			drop.GetComponent<DropItem>().dropTimer = dropCooldown;
			drop.transform.position += transform.forward / 2;
			drop.transform.SetParent(dropParent);

			drop.transform.Rotate(UnityEngine.Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
			drop.GetComponent<Rigidbody>().AddForce(transform.forward * dropForce, ForceMode.Impulse);
			drop.GetComponent<Rigidbody>().AddForce(Vector3.up * dropForce / 1.5f, ForceMode.Impulse);
			

			handItem.amount -= 1;
			ItemDropped(handItem);

			dropTimer = 0.2f;
		}
	}

	public void DropItemByIndex(int index, float dropTime)
	{
		Item item = FindObjectOfType<InventoryManager>().inventory.itemList[index];

		if (item.amount > 0)
		{
			GameObject drop = Instantiate(item.itemData.drop, transform.parent);
			drop.GetComponent<DropItem>().dropTimer = dropTime;
			drop.GetComponent<DropItem>().item.amount = item.amount;
			drop.transform.position += transform.forward / 2;
			drop.transform.SetParent(dropParent);

			drop.transform.Rotate(UnityEngine.Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
			drop.GetComponent<Rigidbody>().AddForce(transform.forward * dropForce, ForceMode.Impulse);
			drop.GetComponent<Rigidbody>().AddForce(Vector3.up * dropForce / 1.5f, ForceMode.Impulse);


			item.amount = 0;
			ItemDropped(item);

			dropTimer = 0.2f;
		}
	}

	public void DropItem(Item item)
	{
		if (item.amount > 0)
		{
			GameObject drop = Instantiate(handItem.itemData.drop, transform.parent);
			drop.GetComponent<DropItem>().dropTimer = dropCooldown;
			drop.transform.position += transform.forward / 2;
			drop.transform.SetParent(dropParent);

			drop.transform.Rotate(UnityEngine.Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
			drop.GetComponent<Rigidbody>().AddForce(transform.forward * dropForce, ForceMode.Impulse);
			drop.GetComponent<Rigidbody>().AddForce(Vector3.up * dropForce / 1.5f, ForceMode.Impulse);


			item.amount -= 1;
			ItemDropped(item);

			dropTimer = 0.2f;
		}
	}

	public void RemoveItem(Item item)
	{
		Debug.Log(item.itemData);
		if (item.amount > 0)
		{
			item.amount -= 1;
			ItemDeleted(item);
		}
	}

	public void PlaceItem()
    {
		handItem.amount -= 1;
		ItemPlaced(handItem);
	}
}
