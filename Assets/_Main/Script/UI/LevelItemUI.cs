using UnityEngine;
using TMPro;

public class LevelItemUI : MonoBehaviour
{
	public GameObject lineH;
	public GameObject lineV;

	public TextMeshProUGUI levelNum;
	public GameObject lockUI;

	public GameObject[] stars;

	public GameObject tutorial;

	public void SetStar(int n)
	{
		for (int i = 0; i < stars.Length; i++)
			stars[i].SetActive(i < n);
	}

	public void SetUnlockUI(bool value)
	{
		lockUI.SetActive(!value);
	}

	public void SetLevelNum(int n, int limit = 9999)
	{
		if (n >= limit) {
			gameObject.SetActive(false);
			return;
		} else {
			gameObject.SetActive(true);
		}

		if (n > 0) {
			levelNum.text = (n + 1).ToString();
			levelNum.gameObject.SetActive(true);
			tutorial.gameObject.SetActive(false);
		} else {
			levelNum.gameObject.SetActive(false);
			tutorial.gameObject.SetActive(true);
		}

		lineH.SetActive(n % 8 != 3 && n % 8 != 4);
		lineV.SetActive(n % 8 == 3 || n % 8 == 7);

		if (n == limit - 1) {
			lineH.SetActive(false);
			lineV.SetActive(false);
		}
	}
}
