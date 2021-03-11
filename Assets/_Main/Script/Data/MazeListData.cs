using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MazeListData : ScriptableObject
{
	public List<MazeData> mazes = new List<MazeData>();
}
