using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
	public int width;
	public int height;

	public MazeData maze;

	protected bool[] marks;
	protected List<int> frontier = new List<int>();

	protected int[] direct = { 1, 2, 4, 8 };

	private void Start()
	{
		GenerateMazeByPrim(Random.Range(0, width * height - 1));
	}

	public void Init()
	{
		maze.width = width;
		maze.height = height;
		maze.cells = new int[width * height];
		marks = new bool[width * height];
		for (int i = 0; i < maze.cells.Length; i++) {
			maze.cells[i] = 0;
			marks[i] = false;
		}

		frontier.Clear();
	}

	public void GenerateMazeByPrim(int startCell)
	{
		Init();
		Visit(startCell);

		while (frontier.Count > 0) {
			int randomCell = Random.Range(0, frontier.Count);
			int cell = frontier[randomCell];
			frontier.RemoveAt(randomCell);

			List<int> nb = new List<int>();
			foreach (int nbID in maze.GetNeighbor(cell))
				if (marks[nbID] == true) {
					int nbID_cp = nbID;
					nb.Add(nbID_cp);
				}

			if (nb.Count > 0) {
				int randID = Random.Range(0, nb.Count);
				maze.Connect(cell, nb[randID]);
			}

			Visit(cell);
		}
	}

	protected void Visit(int cellID)
	{
		if (!maze.IsValidID(cellID)) return;
		if (marks[cellID]) return;

		marks[cellID] = true;

		List<int> nb = maze.GetNeighbor(cellID);

		foreach (int nbID in nb)
			AddFrontier(nbID);
	}

	protected void AddFrontier(int id)
	{
		if (!maze.IsValidID(id)) return;
		if (marks[id]) return;

		if (frontier.Contains(id) == false)
			frontier.Add(id);
	}
}
