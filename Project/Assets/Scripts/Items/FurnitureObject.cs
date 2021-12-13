using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureObject : SmartObject
{
	public GameObject dropPrefab;

	private int damage;
	private float timer;
	private Rigidbody rb;
	private QuickOutline outline;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
		outline = gameObject.AddComponent<QuickOutline>();
		outline.enabled = false;
		outline.OutlineColor = new Color32(0, 255, 0, 255);
		outline.OutlineWidth = 10;
		outline.OutlineMode = QuickOutline.Mode.OutlineVisible;

		rb.isKinematic = true;
	}

	public override void Interact()
	{
		/*damage += 1;
		timer = 0;

		if (damage >= 3)
		{
			CatchObject();
		}*/
		
		CatchObject();
	}

	private void Update()
	{
		timer += Time.deltaTime;
		if (timer > 3)
		{
			damage = 0;
		}
	}

	private void FixedUpdate()
	{
		outline.enabled = false;
	}

	public override bool CanInteract(GameObject obj)
	{
		return true;
	}

	public void CatchObject()
	{
		GameObject drop =  Instantiate(dropPrefab, transform.position + new Vector3(0, 2, 0), new Quaternion());
		drop.transform.SetParent(GameObject.Find("Drops").transform);

		drop.transform.Rotate(UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360));
		drop.GetComponent<Rigidbody>().AddForce(transform.forward * 3, ForceMode.Impulse);
		drop.GetComponent<Rigidbody>().AddForce(Vector3.up * 3 / 1.5f, ForceMode.Impulse);
		drop.GetComponent<DropItem>().dropTimer = 0.5f;


		Destroy(gameObject);
	}

    public void RotateLeft()
	{
		transform.Rotate(0, -90, 0);
	}

	public void RotateRight()
	{
		transform.Rotate(0, 90, 0);
	}

	public override ObjectType GetObjectType()
    {
		return ObjectType.Furniture;
    }

	public void ToggleLocked()
    {
		rb.isKinematic = !rb.isKinematic;

		if (rb.isKinematic) outline.OutlineColor = new Color32(255, 0, 0, 255);
		else outline.OutlineColor = new Color32(0, 255, 0, 255);
	}

	public void SetOutlineEnabled(bool enabled)
	{
		outline.enabled = enabled;
	}
}
