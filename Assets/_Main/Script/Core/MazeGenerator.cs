using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MazeGenerator : MonoBehaviour
{
	[Header("Basic Settings")]
	public int width;
	public int height;

	[Header("Generate Settings")]
	public int startIndexMaze;
	public int genCount;
	public string prefixName;
	public string savingPath;

	protected MazeData maze;
	protected bool[] marks;
	protected List<int> frontier = new List<int>();

	private void Start()
	{
		LazyGenerateHugeMaze();
	}

	public void LazyGenerateHugeMaze()
	{
		for (int i = 0; i < genCount; i++) {
			MazeData maze_asset = ScriptableObject.CreateInstance<MazeData>();

			maze = maze_asset;
			GenerateMazeByPrim(Random.Range(0, width * height - 1));
			maze.targetDoorPos = Random.Range(1, width * height);

			maze.name = prefixName + (i + startIndexMaze).ToString();

			AssetDatabase.CreateAsset(maze_asset, savingPath + maze.name + ".asset");
			AssetDatabase.SaveAssets();
		}
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
