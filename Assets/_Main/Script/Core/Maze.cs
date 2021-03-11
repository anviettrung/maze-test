using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{
	public MazeData mazeData;
	public CellsData cellsData;
	public GameObject bugPrefab;
	public GameObject targetDoorPrefab;

	protected Vector3 offset;

	protected List<SpriteRenderer> cellRend = new List<SpriteRenderer>();
	protected GameObject bug;
	protected GameObject targetDoor;

	public void ReloadMaze()
	{
		for (int y = 0; y < mazeData.height; y++) {
			for (int x = 0; x < mazeData.width; x++) {
				SpriteRenderer cell = cellRend[y * mazeData.width + x];
				cell.sprite = cellsData.Arts[mazeData.GetCell(x, y)];
			}
		}

		bug.transform.localPosition = offset;
		targetDoor.transform.localPosition =
			new Vector3(mazeData.targetDoorPos % mazeData.width,
						mazeData.targetDoorPos / mazeData.width,
						0) + offset;
	}

	public void SpawnMaze()
	{
		offset = new Vector3(-mazeData.width / 2.0f + 0.5f, mazeData.height / 2.0f - 0.5f, 0);

		for (int y = 0; y < mazeData.height; y++) {
			for (int x = 0; x < mazeData.width; x++) {
				SpriteRenderer cell = SpawnEntity(cellsData.Prefab.gameObject, x, y).GetComponent<SpriteRenderer>();
				cell.name = "Cell[" + x +  "," + y + "]";
				cell.sprite = cellsData.Arts[mazeData.GetCell(x, y)];

				cellRend.Add(cell);
			}
		}

		SpawnEntity(bugPrefab, 0, 0);

		SpawnEntity(targetDoorPrefab, mazeData.targetDoorPos % mazeData.width, mazeData.targetDoorPos / mazeData.width);
	}

	public GameObject SpawnEntity(GameObject prefab, int x, int y)
	{
		var clone = Instantiate(prefab);
		clone.transform.SetParent(this.transform);
		clone.transform.localPosition = new Vector3(x, -y, 0) + offset;

		return clone;
	}

	public void PrintPath()
	{
		if (FindPath(mazeData.targetDoorPos, 0)) {
			int p = trace[0];
			while (p != -1) {
				Debug.Log(p);
				p = trace[p];
			}
		}
	}

	// ====== Pathfinding ======
	readonly Queue<int> frontier = new Queue<int>(); // OPEN
	readonly List<int> visited = new List<int>(); // CLOSE
	[HideInInspector] public int[] trace = new int[1];

	protected bool FindPath(int startInd, int endInd)
	{
		if (trace.Length != mazeData.cells.Length)
			trace = new int[mazeData.cells.Length];

		for (int i = 0; i < trace.Length; i++)
			trace[i] = -1;

		visited.Clear();
		frontier.Clear();

		frontier.Enqueue(startInd);

		int p;
		while (frontier.Count > 0) {
			p = frontier.Dequeue();

			if (p == endInd)
				return true; // End

			visited.Add(p);
			foreach(int nb in mazeData.GetConnectNeighbor(p)) {
				if (!visited.Contains(nb) && !frontier.Contains(nb)) {
					frontier.Enqueue(nb);
				
					trace[nb] = p; // previous nb is p;
				}
			}
		}

		return false;
	}

	// ==============================
}
