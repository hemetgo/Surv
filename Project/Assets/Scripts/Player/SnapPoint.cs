using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapPoint : MonoBehaviour
{
	private DecorationObject decorParent;
	public enum Position { None, Top, Right, Bottom, Left }

	private void Start()
	{
		gameObject.layer = 8;
		decorParent = GetComponentInParent<DecorationObject>();
	}

	public Position GetPosition()
	{
		if (decorParent.snap == DecorationObject.SnapType.Wall)
		{
			if (transform.position.y > transform.parent.position.y)
				return Position.Top;
			else if (transform.position.y < transform.parent.position.y)
				return Position.Bottom;

			if (decorParent.isHorizontal)
			{
				if (transform.position.z < transform.parent.position.x)
					return Position.Left;
				else if (transform.position.x > transform.parent.position.x)
					return Position.Right;
			}
			else
			{
				if (transform.position.z > transform.parent.position.z)
					return Position.Left;
				else if (transform.position.z < transform.parent.position.z)
					return Position.Right;
			}
		}
		else if (decorParent.snap == DecorationObject.SnapType.WallZ)
		{
			if (transform.position.y > transform.parent.position.y)
				return Position.Top;
			else if (transform.position.y < transform.parent.position.y)
				return Position.Bottom;

			if (decorParent.isHorizontal)
			{
				if (transform.position.x < transform.parent.position.x)
					return Position.Left;
				else if (transform.position.x > transform.parent.position.x)
					return Position.Right;
			}
			else
			{
				if (transform.position.z > transform.parent.position.z)
					return Position.Left;
				else if (transform.position.z < transform.parent.position.z)
					return Position.Right;
			}
		}
		else if (decorParent.snap == DecorationObject.SnapType.Ground)
		{
			if (transform.position.z > transform.parent.position.z)
			{
				return Position.Top;
			}
			else if (transform.position.z < transform.parent.position.z)
			{
				return Position.Bottom;
			}
			else if (transform.position.x > transform.parent.position.x)
			{
				return Position.Right;
			}
			else if (transform.position.x < transform.parent.position.x)
			{
				return Position.Left;
			}
		}

		return Position.None;
	}

}
