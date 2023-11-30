using System;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using System.Collections.Generic;

//10/05/2020
//01/12/2020
//22/02/2021
//18/10/2021
//18/12/2021
//30/12/2021 Variables and Random Reward
//18/03/2022 Changed To AdMod and Added UnityAds

public class AdMobManager : MonoBehaviour
{
    #region Variables

    private const string _testAppID             = "ca-app-pub-3940256099942544~3347511713";
    private const string _testBannerID          = "ca-app-pub-3940256099942544/6300978111";
    private const string _testInterstitialID    = "ca-app-pub-3940256099942544/1033173712";
    private const string _testRewardID          = "ca-app-pub-3940256099942544/5224354917";

    private BannerView      _bannerView;
    private InterstitialAd  _interstitialAd;
    private RewardedAd      _rewardedAd;

    private string  _appID;
    private string  _bannerID;
    private string  _interstitialID;
    private string  _rewardID;

    private int     _adLoadFailedType   = 0;

    public BannerView BannerView            => _bannerView;
    public InterstitialAd InterstitialAd    => _interstitialAd;
    public RewardedAd RewardedAd            => _rewardedAd;

    public int AdLoadFailedType     { get { return _adLoadFailedType; } set { _adLoadFailedType = value; } }

    #endregion

    #region AdSetup

    protected void Awake()
    {
        //base.Awake();

        if (Config.isDebug)
        {
            DConsole.Log("Test Ad at: " + gameObject.name);
            _appID = _testAppID;
            _bannerID = _testBannerID;
            _interstitialID = _testInterstitialID;
            _rewardID = _testRewardID;
        }
        else
        {
            _appID = Config.adMobAppID;
            _bannerID = Config.adMobBannerID;
            _interstitialID = Config.adMobInterstitialID;
            _rewardID = Config.adMobRewardID;
        }
    }

    public void Start()
    {
        MobileAds.SetiOSAppPauseOnBackground(true);

        List<string> deviceIds = new List<string>() { AdRequest.TestDeviceSimulator };

        // Configure TagForChildDirectedTreatment and test device IDs.
        RequestConfiguration requestConfiguration =
            new RequestConfiguration.Builder()
            .SetTagForChildDirectedTreatment(TagForChildDirectedTreatment.False)
            .SetTestDeviceIds(deviceIds).build();
        MobileAds.SetRequestConfiguration(requestConfiguration);

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(HandleInitCompleteAction);
    }

    private void HandleInitCompleteAction(InitializationStatus initstatus)
    {
        // Callbacks from GoogleMobileAds are not guaranteed to be called on
        // main thread.
        // In this example we use MobileAdsEventExecutor to schedule these calls on
        // the next Update() loop.
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            //RequestBannerAd();
        });
    }

    private AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder().Build();
    }

    #endregion

    #region BANNER ADS

    public void RequestBannerAd()
    {
        // These ad units are configured to always serve test ads.
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = _bannerID;
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/2934735716";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Clean up banner before reusing
        if (_bannerView != null)
        {
            _bannerView.Destroy();
        }

        var adPostion = AdPosition.Bottom;
        if (AdsManager.Instance.bannerPostion == AdsManager.AdPosition.Top)
            adPostion = AdPosition.Top;

        // Create a 320x50 banner at top of the screen
        _bannerView = new BannerView(adUnitId, AdSize.Banner, adPostion);

        // Add Event Handlers
        _bannerView.OnAdLoaded += HandleBannerAdLoaded;
        _bannerView.OnAdFailedToLoad += HandleBannerAdFailedToLoad;
        _bannerView.OnAdOpening += HandleBannerAdOpened;
        _bannerView.OnAdClosed += HandleBannerAdClosed;

        // Load a banner ad
        _bannerView.LoadAd(CreateAdRequest());
    }

    public void DestroyBannerAd()
    {
        if (_bannerView != null)
        {
            _bannerView.Destroy();
        }
    }

    #endregion

    #region INTERSTITIAL ADS

    public void RequestInterstitialAd()
    {
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = _interstitialID;
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Clean up interstitial before using it
        if (_interstitialAd != null)
        {
            _interstitialAd.Destroy();
        }
        _interstitialAd = new InterstitialAd(adUnitId);

        // Add Event Handlers
        _interstitialAd.OnAdLoaded += HandleInterstitialLoaded;
        _interstitialAd.OnAdFailedToLoad += HandleInterstitialFailedToLoad;
        _interstitialAd.OnAdOpening += HandleInterstitialOpened;
        _interstitialAd.OnAdClosed += HandleInterstitialClosed;

        // Load an interstitial ad
        _interstitialAd.LoadAd(CreateAdRequest());
    }

    public void ShowInterstitialAd()
    {
        if (_interstitialAd.IsLoaded())
        {
            _interstitialAd.Show();
        }
    }

    public void DestroyInterstitialAd()
    {
        if (_interstitialAd != null)
        {
            _interstitialAd.Destroy();
        }
    }

    #endregion

    #region REWARDED ADS

    public void RequestRewardAd()
    {
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = _rewardID;
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
        string adUnitId = "unexpected_platform";
#endif

        DConsole.Log("AdMob Reward Loading");

        // create new rewarded ad instance
        _rewardedAd = new RewardedAd(adUnitId);

        // Add Event Handlers
        _rewardedAd.OnAdLoaded += HandleRewardAdLoaded;
        _rewardedAd.OnAdFailedToLoad += HandleRewardAdFailedToLoad;
        _rewardedAd.OnAdOpening += HandleRewardAdOpening;
        _rewardedAd.OnAdFailedToShow += HandleRewardAdFailedToShow;
        _rewardedAd.OnAdClosed += HandleRewardAdClosed;
        _rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;

        // Create empty ad request
        _rewardedAd.LoadAd(CreateAdRequest());
    }

    public void ShowRewardedAd()
    {
        if (_rewardedAd != null && _rewardedAd.IsLoaded())
        {
            _rewardedAd.Show();
        }
        else
        {
            DConsole.Log("Rewarded ad is not ready yet");
        }
    }

    #endregion

    #region Banner callback handlers

    public void HandleBannerAdLoaded(object sender, EventArgs args)
    {
        DConsole.Log("AdMob Banner Loaded");

        UnityMainThread.wkr.AddJob(() =>
        {
            AdsManager.Instance.BannerAdLoaded();
        });
    }

    public void HandleBannerAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        DConsole.Log("AdMob Banner Failed message: " + args.LoadAdError.GetMessage());

        UnityMainThread.wkr.AddJob(() =>
        {
            AdsManager.Instance.BannerAdFailed();
        });
    }

    public void HandleBannerAdOpened(object sender, EventArgs args)
    {
        DConsole.Log("AdMob Banner Opened");
    }

    public void HandleBannerAdClosed(object sender, EventArgs args)
    {
        DConsole.Log("AdMob Banner Closed");
    }

    #endregion

    #region Interstitial callback handlers

    public void HandleInterstitialLoaded(object sender, EventArgs args)
    {
        DConsole.Log("AdMob Interstitial Loaded");

        AdsManager.Instance.InterstitialLoaded();
    }

    public void HandleInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        DConsole.Log("AdMob Interstitial Failed message: " + args.LoadAdError.GetMessage());

        AdsManager.Instance.InterstitialFailedToLoad();
    }

    public void HandleInterstitialOpened(object sender, EventArgs args)
    {
        DConsole.Log("AdMob Interstitial Opened");

        AdsManager.Instance.InterstitialOpened();
    }

    public void HandleInterstitialClosed(object sender, EventArgs args)
    {
        DConsole.Log("AdMob Interstitial Closed");
    }

    #endregion

    #region RewardedAd callback handlers

    public void HandleRewardAdLoaded(object sender, EventArgs args)
    {
        DConsole.Log("AdMob Reward Loaded");
        AdsManager.Instance.RewardLoaded();
    }

    public void HandleRewardAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        if (args.LoadAdError.GetMessage() == "Network Error")
        {
            AdsManager.Instance.rewardAdFailedReason = AdsManager.AdFailedType.NoInternet;
        }
        else
        {
            AdsManager.Instance.rewardAdFailedReason = AdsManager.AdFailedType.Other;
        }

        DConsole.Log("AdMob Reward Failed Error: " + args.LoadAdError.GetMessage());

        AdsManager.Instance.RewardFailedToLoad();
    }

    public void HandleRewardAdOpening(object sender, EventArgs args)
    {
        DConsole.Log("AdMob Reward Opening");

        AdsManager.Instance.RewardOpened();
    }

    public void HandleRewardAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        DConsole.Log("AdMob Reward Failed To Show: " + args.AdError.GetMessage());
    }

    public void HandleRewardAdClosed(object sender, EventArgs args)
    {
        DConsole.Log("AdMob Reward Closed");
        AdsManager.Instance.RewardClosed();
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        DConsole.Log("AdMob Reward Earned");
        AdsManager.Instance.RewardEarned();

        //DConsole.Log("HandleRewardedAdRewarded event received for " + amount.ToString() + " " + type);
    }

    #endregion
}
