using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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

		bug = SpawnEntity(bugPrefab, 0, 0);

		targetDoor = SpawnEntity(targetDoorPrefab, mazeData.targetDoorPos % mazeData.width, mazeData.targetDoorPos / mazeData.width);
	}

	public GameObject SpawnEntity(GameObject prefab, int x, int y)
	{
		var clone = Instantiate(prefab);
		clone.transform.SetParent(this.transform);
		clone.transform.localPosition = new Vector3(x, -y, 0) + offset;

		return clone;
	}

	public Vector3 GetPositionCell(int n)
	{
		return new Vector3(n % mazeData.width, -n / mazeData.width, 0) + offset;
	}

	public void PrintPath()
	{
		if (FindPath(mazeData.targetDoorPos, 0)) {
			foundPath = true;
			int p = trace[0];
			while (p != -1) {
				p = trace[p];
			}
		}
	}

	public void AutoRun()
	{
		if (foundPath) {
			Sequence seq = DOTween.Sequence();
			int p = trace[0];
			while (p != -1) {
				seq.Append(bug.transform.DOMove(GetPositionCell(p), 0.25f).SetEase(Ease.Linear));
				p = trace[p];
			}
		}
	}

	// ====== Pathfinding ======
	readonly Queue<int> frontier = new Queue<int>(); // OPEN
	readonly List<int> visited = new List<int>(); // CLOSE
	[HideInInspector] public int[] trace = new int[1];
	protected bool foundPath = false;

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
