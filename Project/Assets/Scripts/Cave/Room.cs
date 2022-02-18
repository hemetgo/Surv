using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Room
{
	public Vector2Int roomSize;
	public Vector2 startPosition;
	public Vector2 endPosition;
	public Cell startCell;
	public Cell endCell;
	public List<GameObject> grounds = new List<GameObject>();
	public List<GameObject> walls = new List<GameObject>();
	public List<Cell.Direction> doors = new List<Cell.Direction>();
	public List<Room> connections = new List<Room>();
	public List<Cell> cells = new List<Cell>();

	public Room() { }


}
