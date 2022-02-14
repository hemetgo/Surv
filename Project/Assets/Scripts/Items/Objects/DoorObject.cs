using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorObject : SmartObject
{
    public GameObject[] doors;
	public float[] openAngles;
	public bool isOpen;
	private bool canInteract;
	private List<Vector3> startRots = new List<Vector3>();
	private List<Vector3> openRots = new List<Vector3>();

	private void Start()
	{
		canInteract = true;
		for (int i = 0; i < doors.Length; i++)
		{
			startRots.Add(doors[i].transform.localEulerAngles);
			openRots.Add(doors[i].transform.localEulerAngles + new Vector3(0, openAngles[i], 0));
		}
	}

	public override void Interact()
	{
		canInteract = false;
		isOpen = !isOpen;
		
		if (isOpen)
		{
			for (int i = 0; i < doors.Length; i++)
			{
				doors[i].transform.localEulerAngles = openRots[i];
			}
		}
		else
		{
			for (int i = 0; i < doors.Length; i++)
			{
				doors[i].transform.localEulerAngles = startRots[i];
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
