using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorObject : SmartObject
{
    public GameObject[] doors;
	public float[] openAngles;
	public bool isOpen;
	private bool canInteract;

	private void Start()
	{
		canInteract = true;
	}

	public override void Interact()
	{
		canInteract = false;
		isOpen = !isOpen;
		
		if (isOpen)
		{
			for (int i = 0; i < doors.Length; i++)
			{
				doors[i].transform.Rotate(0, openAngles[i], 0);
			}
		}
		else
		{
			for (int i = 0; i < doors.Length; i++)
			{
				doors[i].transform.eulerAngles = new Vector3(0, 0, 0);
			}
		}

		canInteract = true;
	}

	public override bool CanInteract(GameObject obj)
	{
		return canInteract;
	}

	public override ObjectType GetObjectType()
	{
		return ObjectType.Door;
	}

	public override string GetInteractButton()
	{
		return "Interact";
	}
}
