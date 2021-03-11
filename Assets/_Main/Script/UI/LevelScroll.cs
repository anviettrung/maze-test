using Mopsicus.InfiniteScroll;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelScroll : MonoBehaviour
{

	[SerializeField]
	private InfiniteScroll Scroll;

	private int Count = 100;
	public int height = 400;

	public int unlocked = 15;
	public int limitLevel = 50;

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

		if (index % 2 == 0) {
			for (int i = 0; i < row.levelItems.Count; i++) {
				int realInd = index * 4 + i;
				row.levelItems[i].SetLevelNum(realInd, limitLevel);
				// Set Star
				row.levelItems[i].SetUnlockUI(realInd < unlocked);
			}
		} else {
			for (int i = 0; i < row.levelItems.Count; i++) {
				int realInd = index * 4 + (3 - i);
				row.levelItems[i].SetLevelNum(realInd, limitLevel);
				// Set Star
				row.levelItems[i].SetUnlockUI(realInd < unlocked);
			}
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