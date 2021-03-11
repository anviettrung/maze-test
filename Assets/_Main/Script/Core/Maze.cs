using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{
	public MazeData mazeData;
	public CellsData cellsData;
	public GameObject bugPrefab;
	public GameObject targetDoor;

	protected Vector3 offset;

	private void Start()
	{
		SpawnMaze();
	}

	public void SpawnMaze()
	{
		offset = new Vector3(-mazeData.width / 2.0f + 0.5f, mazeData.height / 2.0f - 0.5f, 0);

		for (int y = 0; y < mazeData.height; y++) {
			for (int x = 0; x < mazeData.width; x++) {
				SpriteRenderer cell = SpawnEntity(cellsData.Prefab.gameObject, x, y).GetComponent<SpriteRenderer>();
				cell.name = "Cell[" + x +  "," + y + "]";
				cell.sprite = cellsData.Arts[mazeData.GetCell(x, y)];
			}
		}

		SpawnEntity(bugPrefab, 0, 0);

		SpawnEntity(targetDoor, mazeData.targetDoorPos % mazeData.width, mazeData.targetDoorPos / mazeData.width);
	}

	public GameObject SpawnEntity(GameObject prefab, int x, int y)
	{
		var clone = Instantiate(prefab);
		clone.transform.SetParent(this.transform);
		clone.transform.position = new Vector3(x, -y, 0) + offset;

		return clone;
	}
}
