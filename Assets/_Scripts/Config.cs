using System;
using UnityEngine;

[DefaultExecutionOrder(-25)]
public class Config : Singleton<Config>
{
    public static bool isDebug = false;// Make this false before publish...

    public static string appID = "1";
    public static string appPrivacyLink = "https://www.gameosophy.net/page/nexsa-studio-privacy-policy.html";
    public static string developerLink = "https://play.google.com/store/apps/developer?id=Nexsa+Studio";

    public static string api = "https://www.lazygamerstudio.com/";
    public static string supportEmail = "shout.lazygamer@gmail.com";

    public static string socialFacebook = "https://www.facebook.com/";
    public static string socialInstagram = "https://www.instagram.com/";



		public static readonly string adMobAppID = "ca-app-pub-3940256099942544~3347511713";
        public static readonly string adMobBannerID = "ca-app-pub-3940256099942544/6300978111";
        public static readonly string adMobInterstitialID = "ca-app-pub-3940256099942544/1033173712";
        public static readonly string adMobRewardID = "ca-app-pub-3940256099942544/5224354917";

    // Live UnityAds Keys
        public static readonly string adUnityAppID = "4790371";
        public static readonly string adUnityBannerID = "Banner_Android";
        public static readonly string adUnityInterstitialID = "Interstitial_Android";
        public static readonly string adUnityRewardID = "Rewarded_Android";

        public static int initialRewardAmount = 500;
        public static int addRewardAmount = 150;
        public static int deductRewardAmount = 50;

        public static bool randomReward = false;
        public static int randomRewardMinTimes = 2;
        public static int randomRewardMaxTimes = 6;

    // Rate Panel Settings
    // No. of game or levels should be played to show Rate Panel.
        public static int gameNoToShowRatePanel = 20;
        public static string rateAlertTitle = "Rate Us";
        public static string rateNowButtonLabel = "Rate Now";
        public static string rateLaterButtonLabel = "Later";
        public static string rateDescription = "Please support us by Rating 5 Star!";

        // Share Settings
        public static readonly string shareSubject = "Play this Game - " + appName;
        public static readonly string shareBody = "I just played " + appName + "! Play this game I am sure you will like it: " + appLink;
        public static readonly string shareImageName = "share.png";  //type png is recommended. Keep the image in StreamingAssets folder


        /******************************************  DO NOT EDIT BELOW THIS LINE *********************************/

    public static string appName;
    public static string appLink;
    public static bool isFirstTime = false;
    public static bool isAdRemoved = false;
    public static bool takeTutorial = false;

    public static Action UpdateCoins;

    #if UNITY_EDITOR
    [RuntimeInitializeOnLoadMethod]
    static void Init()
    {
        isFirstTime = false;
        isAdRemoved = false;
        takeTutorial = false;
    }
    #endif

    protected override void Awake()
    {
        base.Awake();

        appName = Application.productName;
        appLink = "https://play.google.com/store/apps/details?id=" + Application.identifier;

        CheckFirstTime();

        DConsole.Log("Config Initialized!");

        PauseGame();
    }

    private void CheckFirstTime()
    {
        if (!PlayerPrefs.HasKey(KeyName.isFirstOpen))
        {
            isFirstTime = true;
            PlayerPrefs.SetInt(KeyName.isFirstOpen, 1);
        }
        else
            isFirstTime = false;
    }

    public static void PauseGame(bool _isPause = true)
    {
        //@ Write Code to Pause Game
        //throw new NotImplementedException("Write code to Puase Game!");
    }

    public static void UpdateCoinAddListener(Action action)
    {
        UpdateCoins += action;
    }

    public static void UpdateCoinRemoveListener(Action action)
    {
        UpdateCoins -= action;
    }
}
