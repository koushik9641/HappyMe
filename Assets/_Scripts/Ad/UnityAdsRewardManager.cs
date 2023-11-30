using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class UnityAdsRewardManager : MonoBehaviour /*, IUnityAdsLoadListener, IUnityAdsShowListener*/
{
    /*private UnityAdsManager _unityAdsManager = null;

    private void Start()
    {
        _unityAdsManager = AdsManager.Instance.unityAdsManager;
    }

    // Start is called before the first frame update
    public void LoadRewardAd()
    {
        _unityAdsManager.IsRewardAdLoaded = false;

        if (!_unityAdsManager.isInitialized) return;

        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        DConsole.Log("UnityAds Reward Loading: " + _unityAdsManager.rewardAdUnitId);
        Advertisement.Load(_unityAdsManager.rewardAdUnitId, this);
    }

    // Implement a method to execute when the user clicks the button:
    public void ShowRewardAd()
    {
        //if (!_unityAdsManager.IsRewardAdLoaded) return;

        DConsole.Log("UnityAds Reward Showing");

        // Then show the ad:
        Advertisement.Show(_unityAdsManager.rewardAdUnitId, this);
    }

    // If the ad successfully loads, add a listener to the button and enable it:
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        DConsole.Log("UnityAds Reward Loaded: " + adUnitId);

        if (adUnitId.Equals(_unityAdsManager.rewardAdUnitId))
        {
            _unityAdsManager.IsRewardAdLoaded = true;
            _unityAdsManager.RewardAdLoaded();
        }
    }

    // Implement the Show Listener's OnUnityAdsShowComplete callback method to determine if the user gets a reward:
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(_unityAdsManager.rewardAdUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            DConsole.Log("UnityAds Reward Earned");

            _unityAdsManager.IsRewardAdLoaded = false;
            _unityAdsManager.RewardAdEarned();

            // Grant a reward.

            // Load another ad:
            //LoadRewardAd();
        }
    }

    // Implement Load and Show Listener error callbacks:
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        _unityAdsManager.IsRewardAdLoaded = false;
        DConsole.Log($"UnityAds Reward Failed Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Use the error details to determine whether to try to load another ad.

        _unityAdsManager.RewardAdFailedToLoad();
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        DConsole.Log($"Error unity showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Use the error details to determine whether to try to load another ad.
    }

    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }*/

}
