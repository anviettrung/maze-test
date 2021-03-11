#if GoogleAdmob
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GoogleMobileAds.Api;

public class Admob : Singleton<Admob>
{
	public bool enableAdmob;
	public bool removedAds = false;
	public bool showInterstitalOnFocus = true;

	[Header("Interstitial ID")]
	public string inter_IOS_ID;
	public string inter_ADR_ID;

	[Header("Reward ID")]
	public string reward_IOS_ID;
	public string reward_ADR_ID;

	[Header("Banner ID")]
	public string banner_IOS_ID;
	public string banner_ANR_ID;
	public AdSize bannerSizeType = AdSize.Banner;

	InterstitialAd interstitial;
	RewardedAd rewardedAd;
	BannerView bannerView;

	Action OnEarnRewardAction = null;

	// Init
	void Start()
	{
		if (PlayerPrefs.GetInt("removed_ads") == 1)
		{
			removedAds = true;
		}
		else
		{
			removedAds = false;

			MobileAds.Initialize(initStatus => { });
			MobileAds.SetiOSAppPauseOnBackground(true);

			RequestBanner();
			RequestInterstitial();
			RequestRewardAd();
		}
	}

	bool focusedFirstTime = false;
	private void OnApplicationFocus(bool focus)
	{
		if (removedAds || !enableAdmob || !showInterstitalOnFocus) return;
		if (focus) {
			if (focusedFirstTime)
				Admob.Instance.ShowInterstitial();
			else
				focusedFirstTime = true;
		}
	}

	#region Interstitial

	public void ShowInterstitial()
	{
		if (!enableAdmob || removedAds) return;

		if (interstitial.IsLoaded())
		{
			interstitial.Show();
		}
	}


	private void RequestInterstitial()
	{
		string adUnitID;

#if UNITY_ANDROID
		adUnitID = inter_ADR_ID;
#elif UNITY_IPHONE
		adUnitID = inter_IOS_ID;
#endif

		if (interstitial != null) interstitial.Destroy();

		interstitial = new InterstitialAd(adUnitID);

		// Called when an ad request has successfully loaded.
		this.interstitial.OnAdLoaded += HandleOnAdLoaded;
		// Called when an ad request failed to load.
		this.interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
		// Called when an ad is shown.
		this.interstitial.OnAdOpening += HandleOnAdOpened;
		// Called when the ad is closed.
		this.interstitial.OnAdClosed += HandleOnAdClosed;
		// Called when the ad click caused the user to leave the application.
		this.interstitial.OnAdLeavingApplication += HandleOnAdLeavingApplication;

		AdRequest request = new AdRequest.Builder().Build();
		interstitial.LoadAd(request);
	}

	public void HandleOnAdLoaded(object sender, EventArgs args)
	{
		//MonoBehaviour.print("HandleAdLoaded event received");
	}

	public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
	{
		//MonoBehaviour.print("HandleFailedToReceiveAd event received with message: "
		//					+ args.Message);
	}

	public void HandleOnAdOpened(object sender, EventArgs args)
	{
		//MonoBehaviour.print("HandleAdOpened event received");
	}

	public void HandleOnAdClosed(object sender, EventArgs args)
	{
		//MonoBehaviour.print("HandleAdClosed event received");
		RequestInterstitial();
	}

	public void HandleOnAdLeavingApplication(object sender, EventArgs args)
	{
		//MonoBehaviour.print("HandleAdLeavingApplication event received");
	}

	#endregion

	#region Reward Ads

	public void ShowRewardAds(Action actionOnEarnedSuccess)
	{
		if (!enableAdmob) return;

#if !UNITY_EDITOR
		if (rewardedAd.IsLoaded())
		{
			OnEarnRewardAction = actionOnEarnedSuccess;
			rewardedAd.Show();
		}
#else
		actionOnEarnedSuccess.Invoke(); // Skip admob
#endif
	}


	public void RequestRewardAd()
	{
		string adUnitID;

#if UNITY_ANDROID
		adUnitID = reward_ADR_ID;
#elif UNITY_IPHONE
		adUnitID = reward_IOS_ID;
#endif

		rewardedAd = new RewardedAd(adUnitID);

		// Called when an ad request has successfully loaded.
		rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
		// Called when an ad request failed to load.
		rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
		// Called when an ad is shown.
		rewardedAd.OnAdOpening += HandleRewardedAdOpening;
		// Called when an ad request failed to show.
		rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
		// Called when the user should be rewarded for interacting with the ad.
		rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
		// Called when the ad is closed.
		rewardedAd.OnAdClosed += HandleRewardedAdClosed;

		AdRequest request = new AdRequest.Builder().Build();
		rewardedAd.LoadAd(request);
	}

	public void HandleRewardedAdLoaded(object sender, EventArgs args)
	{
		//MonoBehaviour.print("HandleRewardedAdLoaded event received");
	}

	public void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
	{
		//MonoBehaviour.print(
		//	"HandleRewardedAdFailedToLoad event received with message: "
		//					 + args.Message);
	}

	public void HandleRewardedAdOpening(object sender, EventArgs args)
	{
		//MonoBehaviour.print("HandleRewardedAdOpening event received");
	}

	public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
	{
		//MonoBehaviour.print(
		//	"HandleRewardedAdFailedToShow event received with message: "
		//					 + args.Message);
	}

	public void HandleRewardedAdClosed(object sender, EventArgs args)
	{
		RequestRewardAd();
	}

	public void HandleUserEarnedReward(object sender, Reward args)
	{
		string type = args.Type;
		double amount = args.Amount;

		//MonoBehaviour.print(
		//	"HandleRewardedAdRewarded event received for "
		//				+ amount.ToString() + " " + type);

		if (amount > 0 && OnEarnRewardAction != null)
			OnEarnRewardAction.Invoke();
	}

	#endregion

	#region Banner
#if UNITY_EDITOR
	[SerializeField] protected GameObject bannerAdsCanvasOnEditor;
#endif

	protected bool bannerShowed = false;
	protected List<RectTransform> canvasResizerBaseOnBannerAds = new List<RectTransform>();

	public void RequestBanner()
	{
		string adUnitID;

#if UNITY_ANDROID
		adUnitID = banner_ADR_ID;
#elif UNITY_IPHONE
		adUnitID = banner_IOS_ID;
#endif

		if (bannerView != null) bannerView.Destroy();

		bannerView = new BannerView(adUnitID, bannerSizeType, AdPosition.Bottom);

		// Called when an ad request has successfully loaded.
		this.bannerView.OnAdLoaded += this.HandleOnBannerAdLoaded;
		// Called when an ad request failed to load.
		this.bannerView.OnAdFailedToLoad += this.HandleOnBannerAdFailedToLoad;
		// Called when an ad is clicked.
		this.bannerView.OnAdOpening += this.HandleOnBannerAdOpened;
		// Called when the user returned from the app after an ad click.
		this.bannerView.OnAdClosed += this.HandleOnBannerAdClosed;
		// Called when the ad click caused the user to leave the application.
		this.bannerView.OnAdLeavingApplication += this.HandleOnBannerAdLeavingApplication;


		AdRequest request = new AdRequest.Builder().Build();
		bannerView.LoadAd(request);
	}

	public void HandleOnBannerAdLoaded(object sender, EventArgs args)
	{
		//MonoBehaviour.print("HandleAdLoaded event received");
		bannerShowed = true;
		ResizeCanvasBaseOnBannerAds();
		bannerView.Show();
	}

	public void HandleOnBannerAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
	{
		//MonoBehaviour.print("HandleFailedToReceiveAd event received with message: "
		//					+ args.Message);
	}

	public void HandleOnBannerAdOpened(object sender, EventArgs args)
	{
		//MonoBehaviour.print("HandleAdOpened event received");
	}

	public void HandleOnBannerAdClosed(object sender, EventArgs args)
	{
		//MonoBehaviour.print("HandleAdClosed event received");
	}

	public void HandleOnBannerAdLeavingApplication(object sender, EventArgs args)
	{
		//MonoBehaviour.print("HandleAdLeavingApplication event received");
	}

	public void ResizeCanvasBaseOnBannerAds()
	{
		for (int i = 0; i < canvasResizerBaseOnBannerAds.Count; i++)
		{
			if (canvasResizerBaseOnBannerAds[i] != null)
				canvasResizerBaseOnBannerAds[i].offsetMax = new Vector2(
					canvasResizerBaseOnBannerAds[i].offsetMax.x,
					-bannerSizeType.Height * 3);
		}
	}

	public void UnresizeCanvasBaseOnBannerAds()
	{
		for (int i = 0; i < canvasResizerBaseOnBannerAds.Count; i++)
		{
			canvasResizerBaseOnBannerAds[i].offsetMax = new Vector2(
				canvasResizerBaseOnBannerAds[i].offsetMax.x,
				0);
		}
	}

	public void AddCanvasResizerBaseOnBannerAds(RectTransform target)
	{
		canvasResizerBaseOnBannerAds.Add(target);
		if (bannerShowed) ResizeCanvasBaseOnBannerAds();
		else UnresizeCanvasBaseOnBannerAds();
	}

	public void RemoveCanvasResizerBaseOnBannerAds(RectTransform target)
	{
		canvasResizerBaseOnBannerAds.Remove(target);
	}

#if UNITY_EDITOR
	public void ShowBannerAdsInEditor()
	{
		bannerShowed = true;
		bannerAdsCanvasOnEditor.gameObject.SetActive(true);
		ResizeCanvasBaseOnBannerAds();
	}
#endif

	#endregion
}

#if UNITY_EDITOR
[CustomEditor(typeof(Admob))]
public class AdmobEditor : Editor
{
	Admob mTarget;

	private void Awake()
	{
		mTarget = (Admob)target;
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (GUILayout.Button("Show Test Banner Ads"))
			mTarget.ShowBannerAdsInEditor();
	}
}

#endif

#endif