using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
	public MazeListData mazeList;
	public int curLevelID = 0;
	protected Gameplay gameplay;
	public Gameplay m_Gameplay {
		get => gameplay;
		set {
			gameplay = value;
		}
	}
	protected UIManager uiManager;
	public UIManager m_UIManager {
		get => uiManager;
		set {
			uiManager = value;
		}
	}

	private void Awake()
	{
		// Init
		if (GameManager.Instance != null) ;
	}

	private void Start()
	{
		curLevelID = AVT.Saver.saveFile.lastOpenedLevel;
	}

	public void PlayLevel(int x)
	{
		curLevelID = x % mazeList.mazes.Count;
		AVT.Saver.saveFile.lastOpenedLevel = curLevelID;

		m_Gameplay.m_Maze.mazeData = mazeList.mazes[curLevelID];
		m_Gameplay.SpawnMaze();

		if (uiManager != null)
			uiManager.gameObject.SetActive(false);
	}


	public void ReplayLevel()
	{
		PlayLevel(curLevelID);
	}

	public void PlayNextLevel()
	{
		PlayLevel(curLevelID + 1);
	}
}
