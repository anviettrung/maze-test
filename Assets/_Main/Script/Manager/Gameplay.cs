using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameplay : MonoBehaviour
{
	public Maze m_Maze;
	bool initedMaze = false;

	private void Awake()
	{
		if (GameManager.IsExist) {
			GameManager.Instance.m_Gameplay = this;
			//GameManager.Instance.ReplayLevel();
		} else {
			SpawnMaze();
		}
	}

	public void SpawnMaze()
	{
		if (!initedMaze) {
			m_Maze.SpawnMaze();
			initedMaze = true;
		} else {
			m_Maze.ReloadMaze();
		}

	}

	public void BackToMainMenu()
	{
		if (GameManager.IsExist)
			GameManager.Instance.m_UIManager.gameObject.SetActive(true);
	}

	public void Retry()
	{
		SpawnMaze();
	}

	public void Continue()
	{
		if (GameManager.IsExist)
			GameManager.Instance.PlayNextLevel();
		else
			SpawnMaze();
	}
}
