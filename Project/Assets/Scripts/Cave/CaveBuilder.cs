using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CaveBuilder : MonoBehaviour
{
	[Header("Settings")]
	public float tileSize;
	public Vector2Int roomCountRange;
	public Vector2Int roomSizeRange;
	public int gridSize;

	[Header("Prefabs")]
	public GameObject groundTilePrefab;
	public GameObject groundStairPrefab;
	public GameObject stairPrefab;
	public GameObject stairRoofPrefab;
	public GameObject[] caveRoofTile;
    public GameObject wallTilePrefab;
    public GameObject doorTilePrefab;
	public GameObject[] mobsPrefabs;
	public CaveLoot[] lootsPrefabs;
	
	[Header("Rooms")]
	public List<Room> rooms = new List<Room>();

	// Privates
	private Cell[,] grid;

	private void Start()
	{
		GenerateGrid();
		AlocateRooms();
		AlocateDoors();
		BuildGrounds();
		BuildWalls();
		PlaceStairs();
		PlaceLoots();
		PlaceMobs();

		FindObjectOfType<FirstPersonController>().transform.position =
			rooms[0].cells[Random.Range(0, rooms[0].cells.Count)].groundTile.transform.position;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	private void GenerateGrid()
	{
		grid = new Cell[gridSize, gridSize];
		for (int x = 0; x < gridSize; x++)
		{
			for (int y = 0; y < gridSize; y++)
			{
				grid[x, y] = new Cell(x, y);
			}
		}
	}

	private void AlocateRooms()
	{
		int roomCounts = Random.Range(roomCountRange.x, roomCountRange.y + 1);

		for(int i = 0; i < roomCounts; i++)
		{
			// Create room data
			Room room = new Room();
			room.roomSize = new Vector2Int(
				Random.Range(roomSizeRange.x, roomSizeRange.y + 1),
				Random.Range(roomSizeRange.x, roomSizeRange.y + 1));


			// Alocate the first room
			if (rooms.Count == 0)
			{
				room.roomSize = new Vector2Int(4, 4);
				Vector2Int startFirst = new Vector2Int(
					gridSize / 2 - room.roomSize.x,
					gridSize / 2 - room.roomSize.y);
				Vector2Int endFirst = new Vector2Int(
					gridSize / 2,
					gridSize / 2);

				room.startPosition = startFirst;
				room.endPosition = endFirst;

				room.startCell = grid[startFirst.x, startFirst.y];
				room.endCell = grid[endFirst.x, endFirst.y];

				for (int x = startFirst.x; x < endFirst.x; x++)
				{
					for (int y = startFirst.y; y < endFirst.y; y++)
					{
						grid[x, y].isFree = false;
						grid[x, y].room = room;
						room.cells.Add(grid[x, y]);
					}
				}

				rooms.Add(room);
				continue;
			}

			// Alocate others rooms
			// Find free start cells
			List<Cell> freeStartCells = new List<Cell>();
			foreach(Cell cell in grid)
			{
				if (GridFree(cell.position, cell.position + room.roomSize))
				{
					if (IsSideOfRoom(cell.position, cell.position + room.roomSize))
					{
						freeStartCells.Add(cell);
					}
				}
			}

			Cell startCell = freeStartCells[Random.Range(0, freeStartCells.Count)];

			Vector2Int start = startCell.position;
			Vector2Int end = new Vector2Int(startCell.position.x + room.roomSize.x, startCell.position.y + room.roomSize.y);

			room.startPosition = start;
			room.endPosition = end;
			
			room.startCell = startCell;
			room.endCell = grid[end.x, end.y];

			for (int x = start.x; x < end.x; x++)
			{
				for (int y = start.y; y < end.y; y++)
				{
					grid[x, y].isFree = false;
					grid[x, y].room = room;
					room.cells.Add(grid[x, y]);
				}
			}

			rooms.Add(room);
		}
	}

	private void AlocateDoors()
	{
		foreach(Room room in rooms)
		{
			var shuffledCells = room.cells.OrderBy(x => System.Guid.NewGuid()).ToList();
			
			foreach (Cell cell in shuffledCells)
			{
				foreach(Cell side in GetSideCells(cell))
				{
					if (!side.isFree)
					{
						if (side.room != cell.room)
						{
							if (side.room.doors.Count == 0)
							{
								if (GetCellDirection(cell, side) == Cell.Direction.Top)
								{
									cell.doors.Add(Cell.Direction.Top);
									room.doors.Add(Cell.Direction.Top);
									side.doors.Add(Cell.Direction.Bottom);
									side.room.doors.Add(Cell.Direction.Bottom);
								}
								else if (GetCellDirection(cell, side) == Cell.Direction.Right)
								{
									cell.doors.Add(Cell.Direction.Right);
									room.doors.Add(Cell.Direction.Right);
									side.doors.Add(Cell.Direction.Left);
									side.room.doors.Add(Cell.Direction.Left);
								}
								else if (GetCellDirection(cell, side) == Cell.Direction.Bottom)
								{
									cell.doors.Add(Cell.Direction.Bottom);
									room.doors.Add(Cell.Direction.Bottom);
									side.doors.Add(Cell.Direction.Top);
									side.room.doors.Add(Cell.Direction.Top);
								}
								else if (GetCellDirection(cell, side) == Cell.Direction.Left)
								{
									cell.doors.Add(Cell.Direction.Left);
									room.doors.Add(Cell.Direction.Left);
									side.doors.Add(Cell.Direction.Right);
									side.room.doors.Add(Cell.Direction.Right);
								}
							}
						}
					}
				}
			}
		}
	}

	private void BuildGrounds()
	{
		Transform groundParent = GameObject.Find("Ground").transform;
		Transform rooftParent = GameObject.Find("Roof").transform;
		foreach (Cell cell in grid)
		{
			if (!cell.isFree)
			{
				Vector3 spawnPos = new Vector3(
					transform.position.x + cell.position.x * tileSize, 
					transform.position.y, 
					transform.position.z + cell.position.y * tileSize);

				GameObject ground = Instantiate(groundTilePrefab, spawnPos, new Quaternion());
				ground.transform.parent = transform;
				cell.groundTile = ground;
				ground.AddComponent<CellGround>().SetCell(cell);
				ground.transform.parent = groundParent;
				cell.room.grounds.Add(ground);

				GameObject roof = Instantiate(caveRoofTile[Random.Range(0, caveRoofTile.Length)], spawnPos, new Quaternion());
				roof.transform.parent = transform;
				float height = wallTilePrefab.GetComponent<Renderer>().bounds.size.y;
				roof.transform.Translate(0, height, 0);
				roof.transform.parent = rooftParent;
				cell.roofTile = roof;
			}
		}
	}

	private void BuildWalls()
	{
		Transform wallParent = GameObject.Find("Wall").transform;
		foreach (Cell cell in grid)
		{
			Cell topCell = null;
			Cell rightCell = null;
			Cell bottomCell = null;
			Cell leftCell = null;

			if (cell.position.y + 1 < gridSize) topCell = grid[cell.position.x, cell.position.y + 1];
			if (cell.position.x + 1 < gridSize) rightCell = grid[cell.position.x + 1, cell.position.y];
			if (cell.position.y - 1 >= 0) bottomCell = grid[cell.position.x, cell.position.y - 1];
			if (cell.position.x - 1 >= 0) leftCell = grid[cell.position.x - 1, cell.position.y];

			if (topCell != null)
			{
				if (!topCell.isFree)
				{
					if (topCell.room != cell.room)
					{
						if (!topCell.walls.Contains(Cell.Direction.Bottom))
						{
							if ((!topCell.doors.Contains(Cell.Direction.Bottom)
								&& !cell.doors.Contains(Cell.Direction.Top)
								|| cell.position.y + 1 >= gridSize))
							{
								Vector3 spawnPos = new Vector3(
									transform.position.x + cell.position.x * tileSize,
									transform.position.y,
									transform.position.z + cell.position.y * tileSize + tileSize / 2);
								GameObject wall = Instantiate(wallTilePrefab, spawnPos, new Quaternion());
								wall.transform.parent = wallParent;
							}
							else if ((topCell.doors.Contains(Cell.Direction.Bottom)
								&& cell.doors.Contains(Cell.Direction.Top)))
							{
								Vector3 spawnPos = new Vector3(
									transform.position.x + cell.position.x * tileSize,
									transform.position.y,
									transform.position.z + cell.position.y * tileSize + tileSize / 2);
								GameObject wall = Instantiate(doorTilePrefab, spawnPos, new Quaternion());
								wall.transform.parent = wallParent;
							}
						}
					}
				}
			}

			if (rightCell != null)
			{
				if (!rightCell.isFree)
				{
					if (rightCell.room != cell.room)
					{
						if (!rightCell.walls.Contains(Cell.Direction.Left))
						{
							if ((!rightCell.doors.Contains(Cell.Direction.Left)
								&& !cell.doors.Contains(Cell.Direction.Right)
								|| cell.position.x + 1 >= gridSize))
							{
								Vector3 spawnPos = new Vector3(
									transform.position.x + cell.position.x * tileSize + tileSize / 2,
									transform.position.y,
									transform.position.z + cell.position.y * tileSize);
								GameObject wall = Instantiate(wallTilePrefab, spawnPos, new Quaternion());
								wall.transform.Rotate(0, 90, 0);
								wall.transform.parent = wallParent;
							}
							else if (rightCell.doors.Contains(Cell.Direction.Left)
								&& cell.doors.Contains(Cell.Direction.Right))
							{
								Vector3 spawnPos = new Vector3(
									transform.position.x + cell.position.x * tileSize + tileSize / 2,
									transform.position.y,
									transform.position.z + cell.position.y * tileSize);
								GameObject wall = Instantiate(doorTilePrefab, spawnPos, new Quaternion());
								wall.transform.Rotate(0, 90, 0);
								wall.transform.parent = wallParent;
							}
						}
					}
				}
			}

			if (bottomCell != null)
			{
				if (!bottomCell.isFree)
				{
					if (bottomCell.room != cell.room)
					{
						if (!bottomCell.walls.Contains(Cell.Direction.Top))
						{
							if ((!bottomCell.doors.Contains(Cell.Direction.Top)
								&& !cell.doors.Contains(Cell.Direction.Bottom))
								|| cell.position.y - 1 < 0)
							{
								Vector3 spawnPos = new Vector3(
									transform.position.x + cell.position.x * tileSize,
									transform.position.y,
									transform.position.z + cell.position.y * tileSize - tileSize / 2);
								GameObject wall = Instantiate(wallTilePrefab, spawnPos, new Quaternion());
								wall.transform.parent = wallParent;
							}
							else if ((bottomCell.doors.Contains(Cell.Direction.Top)
								&& cell.doors.Contains(Cell.Direction.Bottom)))
							{
								Vector3 spawnPos = new Vector3(
									transform.position.x + cell.position.x * tileSize,
									transform.position.y,
									transform.position.z + cell.position.y * tileSize - tileSize / 2);
								GameObject wall = Instantiate(doorTilePrefab, spawnPos, new Quaternion());
								wall.transform.parent = wallParent;
							}
						}
					}
				}
			}

			if (leftCell != null)
			{
				if (!leftCell.isFree)
				{
					if (leftCell.room != cell.room)
					{
						if (!leftCell.walls.Contains(Cell.Direction.Right))
						{
							if ((!leftCell.doors.Contains(Cell.Direction.Right)
								&& !cell.doors.Contains(Cell.Direction.Left))
								|| cell.position.x - 1 < 0)
							{
								Vector3 spawnPos = new Vector3(
									transform.position.x + cell.position.x * tileSize - tileSize / 2,
									transform.position.y,
									transform.position.z + cell.position.y * tileSize);
								GameObject wall = Instantiate(wallTilePrefab, spawnPos, new Quaternion());
								wall.transform.Rotate(0, 90, 0);
								wall.transform.parent = wallParent;
							}
							else if ((!leftCell.doors.Contains(Cell.Direction.Right)
								&& !cell.doors.Contains(Cell.Direction.Left)))
							{
								Vector3 spawnPos = new Vector3(
									transform.position.x + cell.position.x * tileSize - tileSize / 2,
									transform.position.y,
									transform.position.z + cell.position.y * tileSize);
								GameObject wall = Instantiate(doorTilePrefab, spawnPos, new Quaternion());
								wall.transform.Rotate(0, 90, 0);
								wall.transform.parent = wallParent;
							}
						}
					}
				}
			}
		}
	}

	private void PlaceMobs()
	{
		Transform enemyParent = GameObject.Find("Enemy").transform;

		for (int i = 1; i < rooms.Count; i++)
		{
			Room room = rooms[i];
			var shuffledCells = room.cells.OrderBy(x => System.Guid.NewGuid()).ToList();
			Queue cellQueue = new Queue();
			foreach (Cell cell in shuffledCells) 
			{ 
				if (cell.objectFree)
				{
					cellQueue.Enqueue(cell);
				} 
			}
			for (int mobs = 0; mobs < 1; mobs++)
			{
				Cell spawnCell = cellQueue.Dequeue() as Cell;
				GameObject mob = Instantiate(mobsPrefabs[Random.Range(0, mobsPrefabs.Length)], 
					spawnCell.groundTile.transform.position, new Quaternion());
				mob.transform.Translate(0, 1, 0);
				mob.transform.parent = enemyParent;
				spawnCell.objectFree = false;
			}
		}
	}

	private void PlaceLoots()
	{
		Transform lootParent = GameObject.Find("Loot").transform;
		for (int i = 0; i < rooms.Count; i++)
		{
			Room room = rooms[i];
			var shuffledCells = room.cells.OrderBy(x => System.Guid.NewGuid()).ToList();
			Queue cellQueue = new Queue();
			foreach (Cell cell in shuffledCells)
			{
				if (cell.objectFree)
				{
					cellQueue.Enqueue(cell);
				}
			}
			for (int loots = 0; loots < 2; loots++)
			{
				Cell spawnCell = cellQueue.Dequeue() as Cell;
				GameObject loot = Instantiate(lootsPrefabs[Random.Range(0, lootsPrefabs.Length)].prefab,
					spawnCell.groundTile.transform.position, new Quaternion());
				loot.transform.Translate(0, -.5f, 0);
				loot.transform.Rotate(0, Random.Range(0, 360), 0);
				loot.transform.parent = lootParent;
				spawnCell.objectFree = false;
			}
		}
	}

	private void PlaceStairs()
	{
		// Spawn Top Stair
		Room room = rooms[0];
		var shuffledCells = room.cells.OrderBy(x => System.Guid.NewGuid()).ToList();
		Queue cellQueue = new Queue();
		foreach (Cell cell in shuffledCells)
		{
			if (cell.objectFree && cell.doors.Count == 0 &&
				cell.position.x > cell.room.startPosition.x &&
				cell.position.x < cell.room.endPosition.x &&
				cell.position.y > cell.room.startPosition.y &&
				cell.position.y < cell.room.endPosition.y)
			{
				if (cell.position.x > room.startCell.position.x &&
					cell.position.x < room.endCell.position.x &&
					cell.position.y > room.startCell.position.y &&
					cell.position.y < room.endCell.position.y)
				{
					cellQueue.Enqueue(cell);
				}
			}
		}

		Cell spawnCell = cellQueue.Dequeue() as Cell;
		GameObject stair = Instantiate(stairPrefab,
					spawnCell.groundTile.transform.position, new Quaternion());
		float yRot = 0;
		switch (Random.Range(0, 4))
		{
			case 0: yRot = 0; break;
			case 1: yRot = 90; break;
			case 2: yRot = 180; break;
			case 3: yRot = 270; break;
		}
		stair.transform.Rotate(0, yRot, 0);
		stair.transform.parent = transform;
		spawnCell.objectFree = false;
		spawnCell.groundTile = stair;

		GameObject roof = Instantiate(stairRoofPrefab, spawnCell.roofTile.transform);
		roof.transform.parent = transform;
		float height = wallTilePrefab.GetComponent<Renderer>().bounds.size.y;
		roof.transform.parent = spawnCell.roofTile.transform.parent;
		Destroy(spawnCell.roofTile);



		// Spawn Down Stair
		room = rooms[rooms.Count - 1];
		shuffledCells = room.cells.OrderBy(x => System.Guid.NewGuid()).ToList();
		cellQueue = new Queue();
		foreach (Cell cell in shuffledCells)
		{
			if (cell.objectFree && cell.doors.Count == 0)
			{
				cellQueue.Enqueue(cell);
			}
		}


		spawnCell = cellQueue.Dequeue() as Cell;
		GameObject groundStair = Instantiate(groundStairPrefab,
					spawnCell.groundTile.transform.position, new Quaternion());
		yRot = 0;
		switch (Random.Range(0, 4))
		{
			case 0: yRot = 0; break;
			case 1: yRot = 90; break;
			case 2: yRot = 180; break;
			case 3: yRot = 270; break;
		}
		groundStair.transform.Rotate(0, yRot, 0);
		groundStair.transform.parent = transform;
		spawnCell.objectFree = false;
		Destroy(spawnCell.groundTile);
		spawnCell.groundTile = groundStair;
	}

	#region Tools

	private bool GridFree(Vector2Int start, Vector2Int end)
	{
		for (int x = start.x; x <= end.x; x++)
		{
			for (int y = start.y; y <= end.y; y++)
			{
				if (x >= 0 && x < gridSize && y >= 0 && y < gridSize)
				{
					if (!grid[x, y].isFree) return false;
				}
				else
				{
					return false;
				}
			}
		}

		return true;
	}

	private bool IsSideOfRoom(Vector2Int start, Vector2Int end)
	{
		for (int x = start.x; x < end.x; x++)
		{
			for (int y = start.y; y < end.y; y++)
			{
				if (x >= 0 && x < gridSize && y >= 0 && y < gridSize)
				{
					if (y + 1 < gridSize) 
						if (!grid[x, y + 1].isFree) return true;
					if (x + 1 < gridSize) 
						if (!grid[x + 1, y].isFree) return true;
					if (y - 1 > 0)
						if (!grid[x, y - 1].isFree) return true;
					if (x - 1 > 0)
						if (!grid[x - 1, y].isFree) return true;
				}
				else
				{
					return false;
				}
			}
		}

		return false;
	}

	private Cell.Direction GetCellDirection(Cell origin, Cell side)
	{
		if (side.position.y > origin.position.y) return Cell.Direction.Top;
		else if (side.position.x > origin.position.x) return Cell.Direction.Right;
		else if (side.position.y < origin.position.y) return Cell.Direction.Bottom;
		else if (side.position.x < origin.position.x) return Cell.Direction.Left;

		return Cell.Direction.None;
	}

	private List<Cell> GetSideCells(Cell cell)
	{
		List<Cell> cells = new List<Cell>();

		if (cell.position.y + 1 < gridSize)
		{
			cells.Add(grid[cell.position.x, cell.position.y + 1]);
		}
		if (cell.position.x + 1 < gridSize)
		{
			cells.Add(grid[cell.position.x + 1, cell.position.y]);
		}
		if (cell.position.y - 1 >= 0)
		{
			cells.Add(grid [cell.position.x, cell.position.y - 1]);
		}
		if (cell.position.x - 1 >= 0)
		{
			cells.Add(grid [cell.position.x - 1, cell.position.y]);
		}

		return cells;
	}

	#endregion
}
