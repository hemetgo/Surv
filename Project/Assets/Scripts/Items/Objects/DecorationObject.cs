using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorationObject : MonoBehaviour
{
	public bool snap;

	[HideInInspector] public bool isPlacing;
	[HideInInspector] public bool isOverlapping;
	
	//private Rigidbody rb;
	private QuickOutline outline;

	[HideInInspector] public List<Material[]> originalMaterials = new List<Material[]>();
	[HideInInspector] public Material redMaterial;

	//List<Material[]> originalMaterials = new List<Material[]>();

	private void Awake()
	{
		EnableSnappingPoints(false);

		if (GetComponent<Renderer>())
		{
			originalMaterials.Add(GetComponent<Renderer>().materials);
		}
		else
		{
			foreach(Renderer r in GetComponentsInChildren<Renderer>())
			{
				originalMaterials.Add(r.materials);
			}
		}
	}

    public void RotateLeft()
	{
		transform.Rotate(0, -90, 0);
	}

	public void RotateRight()
	{
		transform.Rotate(0, 90, 0);
	}

	public void SetRed()
	{
		if (GetComponent<Renderer>())
		{
			GetComponent<Renderer>().materials = new Material[] { redMaterial };
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

	public void SetOriginal()
	{
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


