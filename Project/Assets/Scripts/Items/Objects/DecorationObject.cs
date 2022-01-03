using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorationObject : MonoBehaviour
{
	public GameObject dropPrefab;
	public bool isPlacing;
	public bool isOverlapping;
	
	private int damage;
	private float timer;
	//private Rigidbody rb;
	private QuickOutline outline;

	 public Material originalMaterial;
	 public Material redMaterial;

	private void Awake()
	{
		originalMaterial = GetComponent<Renderer>().material;
	}

	//public override void Interact()
	//{
	//	damage += 1;
	//	timer = 0;

	//	if (damage >= 3)
	//	{
	//		CatchObject();
	//	}
	//}

	//public override bool CanInteract(GameObject obj)
	//{
	//	return true;
	//}

	//public void CatchObject()
	//{
	//	GameObject drop =  Instantiate(dropPrefab, transform.position + new Vector3(0, 2, 0), new Quaternion());
	//	drop.transform.SetParent(GameObject.Find("Drops").transform);

	//	drop.transform.Rotate(UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360));
	//	drop.GetComponent<Rigidbody>().AddForce(transform.forward * 3, ForceMode.Impulse);
	//	drop.GetComponent<Rigidbody>().AddForce(Vector3.up * 3 / 1.5f, ForceMode.Impulse);
	//	drop.GetComponent<DropItem>().dropTimer = 0.5f;


	//	Destroy(gameObject);
	//}

    public void RotateLeft()
	{
		transform.Rotate(0, -90, 0);
	}

	public void RotateRight()
	{
		transform.Rotate(0, 90, 0);
	}

	//public override ObjectType GetObjectType()
 //   {
	//	return ObjectType.Decoration;
 //   }

	private void OnTriggerStay(Collider other)
	{
		if (other.GetComponent<DecorationObject>() ||
			other.GetComponent<AiAgent>() ||
			other.GetComponent<SmartObject>() ||
			other.GetComponent<DropItem>())
		{
			if (isPlacing) 
			{
				GetComponent<Renderer>().material = redMaterial;
				isOverlapping = true;
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.GetComponent<DecorationObject>() ||
			other.GetComponent<AiAgent>() ||
			other.GetComponent<SmartObject>() ||
			other.GetComponent<DropItem>())
		{
			if (isPlacing) {
				GetComponent<Renderer>().material = originalMaterial;
				isOverlapping = false;
			} }
	}

	//public override string GetInteractButton()
	//{
	//	return "Fire1";
	//}
}


