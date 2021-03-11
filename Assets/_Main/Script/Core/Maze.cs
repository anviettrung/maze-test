using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{
	public MazeData mazeData;


	protected int[] direct = { 1, 2, 4, 8 };
	public void GenerateMazeByPrim(MazeData maze)
	{
		maze.cells = new int[maze.width * maze.height];

		bool[] marks = new bool[maze.width * maze.height];
		Queue<int> frontier = new Queue<int>();

		int firstCellID = Random.Range(0, maze.cells.Length);
		marks[firstCellID] = true;
	}

	protected int MarkCell(int id, int value, ref bool[] marker, ref Queue<int> _frontier)
	{
		marker[id] = true;



		return value | direct[Random.Range(0, 3)];
	}
}
