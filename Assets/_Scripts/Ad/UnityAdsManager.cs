using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class UnityAdsManager : MonoBehaviour/*, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener*/
{
    /*[SerializeField] private UnityAdsRewardManager _unityAdsRewardManager = null;

    private bool _testMode = true;

    private string _androidGameId;
    private string _iOSGameId;

    //private BannerPosition _bannerPosition = BannerPosition.BOTTOM_CENTER;
    private string _androidBannerAdUnitId = "Banner_Android";
    //private string _iOSBannerAdUnitId = "Banner_iOS";
    private string _bannerAdUnitId = null;

    private string _androidInterstitialAdUnitId = "Interstitial_Android";
    //private string _iOSInterstitialAdUnitId = "Interstitial_iOS";
    private string _interstitialAdUnitId;

    private string _androidRewardAdUnitId = "Rewarded_Android";
    //private string _iOSRewardAdUnitId = "Rewarded_iOS";
    private string _rewardAdUnitId = null;

    private string _gameId;
    private bool _isInitialized = false;

    public string rewardAdUnitId => _rewardAdUnitId;
    public bool isInitialized => _isInitialized;
    [HideInInspector] public bool IsRewardAdLoaded = false;

    protected void Awake()
    {
        //base.Awake();

        _androidGameId = Config.adUnityAppID;

        _testMode = true;
        _androidBannerAdUnitId = "Banner_Android";
        _androidInterstitialAdUnitId = "Interstitial_Android";
        _androidRewardAdUnitId = "Rewarded_Android";

        if (!Config.isDebug)
        {
            _testMode = false;

            _androidBannerAdUnitId = Config.adUnityBannerID;
            _androidInterstitialAdUnitId = Config.adUnityInterstitialID;
            _androidRewardAdUnitId = Config.adUnityRewardID;
        }
    }

    private void Start()
    {
        _bannerPosition = BannerPosition.BOTTOM_CENTER;
        if (AdsManager.Instance.bannerPostion == AdsManager.AdPosition.Top)
            _bannerPosition = BannerPosition.TOP_CENTER;
        
        _isInitialized = false;
        InitializeAds();
    }

    #region Initialize

    public void InitializeAds()
    {
        _gameId = (Application.platform == RuntimePlatform.IPhonePlayer) ? _iOSGameId : _androidGameId;

        _interstitialAdUnitId = _androidInterstitialAdUnitId;
        _rewardAdUnitId = _androidRewardAdUnitId;
        _bannerAdUnitId = _androidBannerAdUnitId;

        //_showAdButton.interactable = false;

        Advertisement.Initialize(_gameId, _testMode, this);
        
        Advertisement.Banner.SetPosition(_bannerPosition);
    }

    public void OnInitializationComplete()
    {
        DConsole.Log("Unity Ads initialization complete.");

        _isInitialized = true;
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        DConsole.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");

        _isInitialized = false;
    }

    #endregion

    #region Banner

    // Implement a method to call when the Load Banner button is clicked:
    public void LoadBanner()
    {
        if (!_isInitialized) return;

        DConsole.Log("UnityAds Banner Loading");

        // Set up options to notify the SDK of load events:
        BannerLoadOptions options = new BannerLoadOptions
        {
            loadCallback = OnBannerLoaded,
            errorCallback = OnBannerError
        };

        // Load the Ad Unit with banner content:
        Advertisement.Banner.Load(_bannerAdUnitId, options);
    }

    // Implement code to execute when the loadCallback event triggers:
    void OnBannerLoaded()
    {
        DConsole.Log("UnityAds Banner loaded");

        AdsManager.Instance.BannerAdLoaded();

        ShowBannerAd();
    }

    // Implement code to execute when the load errorCallback event triggers:
    void OnBannerError(string message)
    {
        DConsole.Log($"UnityAds Banner Failed: {message}");
        // Optionally execute additional code, such as attempting to load another ad.

        AdsManager.Instance.BannerAdFailed();
    }

    // Implement a method to call when the Show Banner button is clicked:
    void ShowBannerAd()
    {
        DConsole.Log("UnityAds Banner Shown");

        // Set up options to notify the SDK of show events:
        BannerOptions options = new BannerOptions
        {
            clickCallback = OnBannerClicked,
            hideCallback = OnBannerHidden,
            showCallback = OnBannerShown
        };

        // Show the loaded Banner Ad Unit:
        Advertisement.Banner.Show(_bannerAdUnitId, options);
    }

    // Implement a method to call when the Hide Banner button is clicked:
    public void HideBannerAd()
    {
        DConsole.Log("UnityAds Banner Hide");

        // Hide the banner:
        //Advertisement.Banner.Hide();
    }

    void OnBannerClicked() { }
    void OnBannerShown() { }
    void OnBannerHidden() { }

    #endregion

    #region Interstitial

    // Load content to the Ad Unit:
    public void LoadInterstitialAd()
    {
        if (!_isInitialized) return;

        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        DConsole.Log("UnityAd Load Interstitial: " + _interstitialAdUnitId);
        //Advertisement.Load(_interstitialAdUnitId, this);
    }

    // Show the loaded content in the Ad Unit:
    public void ShowInterstitialAd()
    {
        // Note that if the ad content wasn't previously loaded, this method will fail
        DConsole.Log("UnityAd Showing Interstitial: " + _interstitialAdUnitId);
        //Advertisement.Show(_interstitialAdUnitId, this);
    }

    // Implement Load Listener and Show Listener interface methods: 
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        DConsole.Log("UnityAd Interstitial Loaded");
        AdsManager.Instance.InterstitialLoaded();

        // Optionally execute code if the Ad Unit successfully loads content.
        //ShowInterstitialAd();
    }

    /*public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        DConsole.Log($"UnityAd Interstitial Failed Ad Unit: {adUnitId} - {error.ToString()} - {message}");
        // Optionally execute code if the Ad Unit fails to load, such as attempting to try again.

        AdsManager.Instance.InterstitialFailedToLoad();
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        DConsole.Log($"Error showing Interstitial Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Optionally execute code if the Ad Unit fails to show, such as loading another ad.
    }

    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState) { }
    
    #endregion

    #region Reward

    public void LoadRewardAd()
    {
        AdsManager.Instance.unityAdsRewardManager.LoadRewardAd();
    }

    public void ShowRewardAd()
    {
        AdsManager.Instance.unityAdsRewardManager.ShowRewardAd();
    }

    public void RewardAdLoaded()
    {
        AdsManager.Instance.RewardLoaded();
    }

    public void RewardAdFailedToLoad()
    {
        AdsManager.Instance.rewardAdFailedReason = AdsManager.AdFailedType.NoInternet;
        AdsManager.Instance.RewardFailedToLoad();
    }

    public void RewardAdEarned()
    {
        AdsManager.Instance.RewardEarned();
        AdsManager.Instance.RewardClosed();
    }

    #endregion
    */
}
