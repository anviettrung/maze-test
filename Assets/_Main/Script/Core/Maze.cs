using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{
	public MazeData mazeData;
	public CellsData cellsData;

	private void Start()
	{
		SpawnMaze();
	}

	public void SpawnMaze()
	{
		Vector3 offset = new Vector3(-mazeData.width / 2.0f + 0.5f, mazeData.height / 2.0f - 0.5f, 0);

		for (int y = 0; y < mazeData.height; y++) {
			for (int x = 0; x < mazeData.width; x++) {
				SpriteRenderer cell = Instantiate(cellsData.Prefab);
				cell.name = "Cell[" + x +  "," + y + "]";
				cell.transform.SetParent(this.transform);
				cell.transform.localPosition = new Vector3(x, -y, 0) + offset;

				cell.sprite = cellsData.Arts[mazeData.GetCell(x, y)];
			}
		}
	}
}
