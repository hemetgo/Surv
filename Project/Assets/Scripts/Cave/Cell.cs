using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
	public Room room;
	public Vector2Int position;
	public GameObject groundTile;
	public bool isFree;
	public List<Direction> walls = new List<Direction>();
	public List<Direction> doors = new List<Direction>();

	public enum Direction { None, Top, Right, Bottom, Left }

	public Cell(int x, int y)
	{
		isFree = true;
		position = new Vector2Int(x, y);
	}

}
