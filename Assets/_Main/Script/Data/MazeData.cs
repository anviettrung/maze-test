﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MazeData : ScriptableObject
{
	public int width = 10;
	public int height = 13;
	public int[] cells = new int[130];

	protected readonly static int[] direct = { 1, 2, 4, 8 }; // N E S W

	public int GetCell(int x, int y)
	{
		int id = y * width + x;

		return IsValidID(id) ? cells[id] : - 1;
	}

	public bool IsValidID(int id) => (id >= 0) && (id < cells.Length);

	public void RemoveEdge(int cellID, int direct)
	{

	}

	public List<int> GetNeighbor(int cellID)
	{
		List<int> results = new List<int>();

		if (IsValidID(cellID - width)) results.Add(cellID - width);
		if (cellID % width != 0) results.Add(cellID - 1);
		if (IsValidID(cellID + width)) results.Add(cellID + width);
		if (cellID % width != width - 1) results.Add(cellID + 1);

		return results;
	}

	public void Connect(int cellA, int cellB)
	{
		if (cellA + width == cellB) { // B is bottom of A
			cells[cellA] |= direct[2];
			cells[cellB] |= direct[0];
		} else if (cellA - width == cellB) {
			cells[cellA] |= direct[0];
			cells[cellB] |= direct[2];
		} else if (cellA + 1 == cellB) {
			cells[cellA] |= direct[1];
			cells[cellB] |= direct[3];
		} else if (cellA - 1 == cellB) {
			cells[cellA] |= direct[3];
			cells[cellB] |= direct[1];
		}
	}
}