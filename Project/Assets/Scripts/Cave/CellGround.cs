using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellGround : MonoBehaviour
{
    private Cell cell;
    public Vector2Int position;
    public List<Cell.Direction> walls = new List<Cell.Direction>();
    public List<Cell.Direction> doors = new List<Cell.Direction>();

    private void Update()
    {
        if (cell != null)
        {
            position = cell.position;
            walls = cell.walls;
            doors = cell.doors;
        }
    }

	public void SetCell(Cell cell)
	{
        this.cell = cell;
	}

}


