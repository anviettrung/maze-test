﻿using Mopsicus.InfiniteScroll;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelScroll : MonoBehaviour
{
	[SerializeField]
	private InfiniteScroll Scroll;

	private int Count = 100;
	public int height = 400;

	protected int unlocked = 15;
	public int limitLevel = 50;

	private void Awake()
	{
		unlocked = Random.Range(5, limitLevel - 1);
	}

	void Start()
	{
		Scroll.OnFill += OnFillItem;
		Scroll.OnHeight += OnHeightItem;

		Count = Mathf.CeilToInt(limitLevel / 4.0f);
		Scroll.InitData(Count);
	}

	void OnFillItem(int index, GameObject item)
	{
		LevelRowUI row = item.GetComponent<LevelRowUI>();

		for (int i = 0; i < row.levelItems.Count; i++) {
			int realInd = index * 4;
			realInd += index % 2 == 0 ? i : (3 - i);
			row.levelItems[i].SetLevelNum(realInd, limitLevel);

			// In real publish game. We will get star count from database
			row.levelItems[i].SetStar(Random.Range(1, 3));
			row.levelItems[i].SetUnlockUI(realInd < unlocked);
		}
	}

	int OnHeightItem(int index)
	{
		return height;
	}

	public void SceneLoad(int index)
	{
		SceneManager.LoadScene(index);
	}

}