using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureObject : SmartObject
{
	public GameObject dropPrefab;

	private int damage;
	private float timer;


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

	public void ToggleKinematic()
    {
		GetComponent<Rigidbody>().isKinematic = !GetComponent<Rigidbody>().isKinematic;
	}
}
