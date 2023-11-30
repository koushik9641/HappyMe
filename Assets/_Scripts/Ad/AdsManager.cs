using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Events;

//18/03/2022 Changed To AdMod and Added UnityAds
//21/03/2022 Fallback Network
//30/06/2022 Restricted to One Ad Load for Reward & Interstitial
//04/07/2022 Remove null condition from RewardCallback

public class AdsManager : SingletonDontDestroy<AdsManager>
{
    public enum AdNetwork { None, AdMob, UnityAds }
    public enum AdFailedType { None, NoInternet, Other }
    public enum AdPosition { Bottom, Top }

    public class RewardCallback
    {
        public Action rewardLoaded;
        public Action rewardFailedToLoad;
        public Action rewardEarned;
        public Action rewardClosed;
    }

    [SerializeField] private AdMobManager _adMobManager = null;
    [SerializeField] private UnityAdsManager _unityAdsManager = null;
    [SerializeField] private UnityAdsRewardManager _unityAdsRewardManager = null;

    [SerializeField] private AdNetwork _firstAdNetwork = AdNetwork.AdMob;
    [SerializeField] private AdNetwork _fallbackNetwork = AdNetwork.None;
    [SerializeField] private AdPosition _bannerPosition = AdPosition.Bottom;

    [SerializeField] private GameObject _adBannerBackground = null;

    private bool _isRewarded = false;
    private int _rewardAmount = 0;
    private int _rewardCollected = 0;

    private AdNetwork _currentBannerNetwork;
    private AdNetwork _currentInterstitialNetwork;
    private AdNetwork _currentRewardNetwork;

    private RewardCallback _rewardCallback = null;
    protected AdFailedType _rewardAdFailedReason = AdFailedType.None;

    protected bool _isAlreadyInsterstitialLoaded = false;
    protected bool _isAlreadyRewardLoaded = false;

    public UnityAdsManager unityAdsManager => _unityAdsManager;
    public UnityAdsRewardManager unityAdsRewardManager => _unityAdsRewardManager;

    public AdPosition bannerPostion => _bannerPosition;

    public bool IsRewarded { get { return _isRewarded; } set { _isRewarded = value; } }
    public int RewardCollected => _rewardCollected;

    public AdFailedType rewardAdFailedReason {
        get { return _rewardAdFailedReason; }
        set { _rewardAdFailedReason = value; }
    }

    //public static AdsManager Instance;

    protected  override void Awake()
    {
        base.Awake();

        //Instance = this;

        if (_adMobManager == null)
            _adMobManager = GetComponent<AdMobManager>();

        if (_unityAdsManager)
            _unityAdsManager = GetComponent<UnityAdsManager>();

        if (_unityAdsRewardManager)
            _unityAdsRewardManager = GetComponent<UnityAdsRewardManager>();

        _adBannerBackground.SetActive(false);

        InitRewardAmount();

        if (_fallbackNetwork == _firstAdNetwork)
            _fallbackNetwork = AdNetwork.None;
    }

    #region RewardSetup

    public void InitRewardAmount()
    {

        /*if (!PlayerPrefs.HasKey(KeyName.rewardAmount))
            PlayerPrefs.SetInt(KeyName.rewardAmount, Config.initialRewardAmount);

        _rewardAmount = PlayerPrefs.GetInt(KeyName.rewardAmount, Config.initialRewardAmount);*/
    }

    public int GetRewardAmount()
    {
        return _rewardAmount;
    }

    public void DeductRewardAmount()
    {
        _rewardAmount -= Config.deductRewardAmount;
        //PlayerPrefs.SetInt(KeyName.rewardAmount, _rewardAmount);
    }

    public void AddRewardAmount(int reward = 0)
    {
        if (reward == 0)
        {
            if (Config.randomReward)
                reward = UnityEngine.Random.Range(Config.randomRewardMinTimes, Config.randomRewardMaxTimes + 1) * Config.deductRewardAmount;
            else
                reward = Config.addRewardAmount;
        }

        _rewardCollected = reward;

        _rewardAmount += reward;
        //PlayerPrefs.SetInt(KeyName.rewardAmount, _rewardAmount);
    }

    #endregion

    #region Banner

    public void LoadBanner(AdNetwork adNetwork = AdNetwork.None)
    {
        if (adNetwork == AdNetwork.None)
            adNetwork = _firstAdNetwork;

        _adMobManager.DestroyBannerAd();
        //_unityAdsManager.HideBannerAd();

        DConsole.Log("AdsManager Banner Loading From: " + adNetwork);

        if (adNetwork == AdNetwork.AdMob)
        {
            _currentBannerNetwork = AdNetwork.AdMob;
            _adMobManager.RequestBannerAd();
        }
        else
        {
            _currentBannerNetwork = AdNetwork.UnityAds;
            //_unityAdsManager.LoadBanner();
        }
    }

    public void DestroyBanner()
    {
        DConsole.Log("AdsManager Banner Destroy");

        _adMobManager.DestroyBannerAd();
        //_unityAdsManager.HideBannerAd();

        _adBannerBackground.SetActive(false);
    }

    public void BannerAdLoaded()
    {
        DConsole.Log("AdsManager Banner Loaded");
        _adBannerBackground.SetActive(true);
    }

    public void BannerAdFailed()
    {
        _adBannerBackground.SetActive(false);

        DConsole.Log("AdsManager Banner Failed: " + _currentBannerNetwork);

        if (_fallbackNetwork != AdNetwork.None && _currentBannerNetwork != _fallbackNetwork)
        {
            LoadBanner(_fallbackNetwork);
        }
    }

    #endregion

    #region Interstitial

    public void LoadInterstitial(AdNetwork adNetwork = AdNetwork.None)
    {
        if (_isAlreadyInsterstitialLoaded)
        {
            InterstitialLoaded();
            return;
        }

        if (adNetwork == AdNetwork.None)
            adNetwork = _firstAdNetwork;

        DConsole.Log("AdsManager Interstitial Loading From: " + adNetwork);

        if (adNetwork == AdNetwork.AdMob)
        {
            _currentInterstitialNetwork = AdNetwork.AdMob;
            _adMobManager.RequestInterstitialAd();
        }
        else
        {
            _currentInterstitialNetwork = AdNetwork.UnityAds;
            //_unityAdsManager.LoadInterstitialAd();
        }
    }

    public void ShowInterstitial()
    {
        DConsole.Log("AdsManager Interstitial Show");

        if (_currentInterstitialNetwork == AdNetwork.AdMob)
            _adMobManager.ShowInterstitialAd();
        else
            ;// _unityAdsManager.ShowInterstitialAd();
    }

    public void InterstitialLoaded()
    {
        DConsole.Log("AdsManager Interstitial Loaded");

        _isAlreadyInsterstitialLoaded = true;
    }

    public void InterstitialFailedToLoad()
    {
        DConsole.Log("AdsManager Interstitial Load Failed");

        if (_fallbackNetwork != AdNetwork.None && _currentInterstitialNetwork != _fallbackNetwork)
        {
            LoadInterstitial(_fallbackNetwork);
        }
    }

    public void InterstitialOpened()
    {
        _isAlreadyInsterstitialLoaded = false;
    }

    #endregion

    #region Reward

    public void RewardAddListener(RewardCallback rewardCallback)
    {
        DConsole.Log("AdsManager Reward Add Listener");

        _rewardCallback = rewardCallback;
    }

    public void LoadReward(AdNetwork adNetwork = AdNetwork.None)
    {
        if (_isAlreadyRewardLoaded)
        {
            RewardLoaded();
            return;
        }

        if (adNetwork == AdNetwork.None)
            adNetwork = _firstAdNetwork;

        DConsole.Log("AdsManager Reward Loading From: " + adNetwork);

        _rewardAdFailedReason = AdFailedType.None;
        if (adNetwork == AdNetwork.AdMob)
        {
            _currentRewardNetwork = AdNetwork.AdMob;
            _adMobManager.RequestRewardAd();
        }
        else
        {
            _currentRewardNetwork = AdNetwork.UnityAds;
            // _unityAdsManager.LoadRewardAd();
        }
    }

    public void ShowReward(Action rewardEarned = null)
    {
        DConsole.Log("AdsManager Reward Show: " + _currentRewardNetwork);

        // For multiple reward
        if (_rewardCallback != null && rewardEarned != null)
        {
            _rewardCallback.rewardEarned = rewardEarned;
        }

        if (_currentRewardNetwork == AdNetwork.AdMob)
            _adMobManager.ShowRewardedAd();
        else
            ;// _unityAdsManager.ShowRewardAd();
    }

    public bool IsRewardLoaded()
    {
        bool val = false;
        if (_currentRewardNetwork == AdNetwork.AdMob)
            val = _adMobManager.RewardedAd.IsLoaded();
        else
            ;// val = _unityAdsManager.IsRewardAdLoaded;

        DConsole.Log("AdsManager Reward Ready: " + _currentRewardNetwork + ", Val: " + val);

        return val;
    }

    // Callbacks
    public void RewardLoaded()
    {
        DConsole.Log("AdsManager Reward Loaded: " + _currentRewardNetwork);

        _isAlreadyRewardLoaded = true;

        _rewardAdFailedReason = AdFailedType.None;
        _rewardCallback?.rewardLoaded?.Invoke();
    }

    public void RewardEarned()
    {
        DConsole.Log("AdsManager Reward Earned: " + _currentRewardNetwork);

        _rewardCallback?.rewardEarned?.Invoke();
    }

    public void RewardFailedToLoad()
    {
        DConsole.Log("AdsManager Reward Failed: " + _currentRewardNetwork);

        _rewardCallback?.rewardFailedToLoad?.Invoke();

        if (_fallbackNetwork != AdNetwork.None && _currentRewardNetwork != _fallbackNetwork)
        {
            LoadReward(_fallbackNetwork);
        }
    }

    public void RewardClosed()
    {
        DConsole.Log("AdsManager Reward Closed: " + _currentRewardNetwork);

        _rewardCallback?.rewardClosed?.Invoke();
    }

    public void RewardOpened()
    {
        _isAlreadyRewardLoaded = false;
    }

    #endregion
}
