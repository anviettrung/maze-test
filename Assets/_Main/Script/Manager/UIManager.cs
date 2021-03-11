using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
	public LevelScroll levelScroll;

	private void Awake()
	{
		if (GameManager.IsExist)
			GameManager.Instance.m_UIManager = this;
	}

	private void Start()
	{
		levelScroll.unlocked = Random.Range(
			GameManager.Instance.curLevelID,
			GameManager.Instance.mazeList.mazes.Count);

		levelScroll.limitLevel = GameManager.Instance.mazeList.mazes.Count;

		levelScroll.Init();
	}
}
