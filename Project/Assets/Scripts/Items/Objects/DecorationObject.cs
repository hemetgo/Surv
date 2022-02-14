using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorationObject : MonoBehaviour
{
	public BoxCollider placingCollider;
	public Collider triggerCollider;
	public Renderer rend;
	[HideInInspector] public bool isHorizontal;
	public SnapType snap;
	public enum SnapType { None, Wall, Ground, WallZ }

	[HideInInspector] public bool isPlacing;
	public bool isOverlapping;
	
	//private Rigidbody rb;
	private QuickOutline outline;

	[HideInInspector] public List<Material[]> originalMaterials = new List<Material[]>();
	[HideInInspector] public Material redMaterial;

	//List<Material[]> originalMaterials = new List<Material[]>();

	private void Awake()
	{
		if (GetComponent<Renderer>())
		{
			originalMaterials.Add(GetComponent<Renderer>().materials);
			foreach (Renderer r in GetComponentsInChildren<Renderer>())
			{
				originalMaterials.Add(r.materials);
			}
		}
		else
		{
			foreach(Renderer r in GetComponentsInChildren<Renderer>())
			{
				originalMaterials.Add(r.materials);
			}
		}

		if (rend == null) rend = GetComponent<Renderer>();
		//if (placingCollider == null) placingCollider = GetComponent<BoxCollider>();
		//placingCollider.enabled = true;

		PlaceObject();
	}

	public void RotateLeft()
	{
		transform.Rotate(0, -90, 0);
	}

	public void RotateRight()
	{
		transform.Rotate(0, 90, 0);
	}
	public void SetMaterial(Material mat)
	{
		if (GetComponent<Renderer>())
		{
			GetComponent<Renderer>().materials = new Material[] { mat };
			Renderer[] rends = GetComponentsInChildren<Renderer>();
			for (int i = 0; i < rends.Length; i++)
			{
				Material[] matList = new Material[rends[i].materials.Length];
				for (int m = 0; m < rends[i].materials.Length; m++)
				{
					matList[m] = mat;
				}

				rends[i].materials = matList;
			}
		}
		else
		{
			Renderer[] rends = GetComponentsInChildren<Renderer>();
			for (int i = 0; i < rends.Length; i++)
			{
				Material[] matList = new Material[rends[i].materials.Length];
				for (int m = 0; m < rends[i].materials.Length; m++)
				{
					matList[m] = mat;
				}

				rends[i].materials = matList;
			}
		}
	}

	public void SetRed()
	{
		if (GetComponent<Renderer>())
		{
			GetComponent<Renderer>().materials = new Material[] { redMaterial };
			Renderer[] rends = GetComponentsInChildren<Renderer>();
			for (int i = 0; i < rends.Length; i++)
			{
				Material[] redList = new Material[rends[i].materials.Length];
				for (int m = 0; m < rends[i].materials.Length; m++)
				{
					redList[m] = redMaterial;
				}

				rends[i].materials = redList;
			}
		}
		else
		{
			Renderer[] rends = GetComponentsInChildren<Renderer>();
			for (int i = 0; i < rends.Length; i++)
			{
				Material[] redList = new Material[rends[i].materials.Length];
				for(int m = 0; m < rends[i].materials.Length; m++)
				{
					redList[m] = redMaterial;
				}

				rends[i].materials = redList;
			}
		}
	}

	public void SetOriginalMaterials()
	{
		if (GetComponent<Renderer>())
		{
			GetComponent<Renderer>().materials = originalMaterials[0];
			Renderer[] rends = GetComponentsInChildren<Renderer>();
			for (int i = 1; i < rends.Length; i++)
			{
				rends[i].materials = originalMaterials[i];
			}
		}
		else
		{
			Renderer[] rends = GetComponentsInChildren<Renderer>();
			for (int i = 0; i < rends.Length; i++)
			{
				rends[i].materials = originalMaterials[i];
			}
		}
	}

	public void PlaceObject()
	{
		isPlacing = false;
		isOverlapping = false;
		gameObject.layer = 0;
		foreach (Transform go in GetComponentsInChildren<Transform>()) go.gameObject.layer = 0;
		SetOriginalMaterials();
		EnableSnappingPoints(true);
		EnableTrigger(false);
		EnableRigidbody(false);
		Destroy(placingCollider);
	}

	public void EnableSnappingPoints(bool enable)
	{
		foreach (SnapPoint snap in GetComponentsInChildren<SnapPoint>())
		{
			if (enable)
				snap.gameObject.layer = 0;
			else
				snap.gameObject.layer = 2;

			snap.enabled = enable;
		}

		foreach (Collider col in GetComponents<Collider>())
		{
			col.enabled = enable;
		}

		foreach (Collider col in GetComponentsInChildren<Collider>())
		{
			col.enabled = enable;
		}
	}
	public void EnableTrigger(bool enable)
	{
		foreach (BoxCollider box in GetComponents<BoxCollider>())
		{
			if (box != triggerCollider)
				box.isTrigger = enable;
		}
	}
	public void EnableRigidbody(bool enable)
	{
		if (enable)
		{
			Rigidbody rig = gameObject.AddComponent<Rigidbody>();
			if (rig)
			{
				rig.useGravity = false;
				rig.isKinematic = true;
			}
		}
		else
		{
			Destroy(GetComponent<Rigidbody>());
		}
	}
	

	private void OnTriggerEnter(Collider other)
	{
		if (other.GetComponent<DecorationObject>() ||
			other.GetComponent<AiAgent>() ||
			other.GetComponent<SmartObject>() ||
			other.GetComponent<DropItem>())
		{
			if (isPlacing) 
			{
				isOverlapping = true;
				//GetComponent<Renderer>().material = redMaterial;
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
			if (isPlacing) 
			{
				
				isOverlapping = false;
				if (GetComponent<Renderer>())
				{
					GetComponent<Renderer>().materials = originalMaterials[0];
				}
				else
				{
					Renderer[] rends = GetComponentsInChildren<Renderer>();
					for (int i = 0; i < rends.Length; i++)
					{
						rends[i].materials = originalMaterials[i];
					}
				}
			} 
		}
	}

}


