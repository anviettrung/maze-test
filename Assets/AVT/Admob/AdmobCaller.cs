#if GoogleAdmob
using UnityEngine;

public class AdmobCaller : MonoBehaviour
{
	public void ShowInterstitial()
	{
		Admob.Instance.ShowInterstitial();
	}

	public void ShowInterstitialIfOldPlayer()
	{
		if (PlayerPrefs.HasKey("play_game_second_time"))
		{
			Debug.Log("UwU");
			Admob.Instance.ShowInterstitial();
		}
		else
		{
			PlayerPrefs.SetInt("play_game_second_time", 1);
		}
	}
}

#endif