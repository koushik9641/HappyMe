using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MyRewardManager : Singleton<MyRewardManager>
{

    private void OnEnable()
    {
        DelayRewardLoad(0.2f);
    }

    // Start is called before the first frame update
    public void RequestRewardVideo()
    {
       // _rewardButton.interactable = false;

        DConsole.Log("BoardManager Requesting Reward");

        AdsManager.Instance.RewardAddListener(new AdsManager.RewardCallback()
        {
            rewardLoaded = RewardVideoLoaded,
            rewardEarned = RewardVideoRewarded,             // It will be replaced on multiple reward
            rewardFailedToLoad = RewardVideoFailedToLoad,
            rewardClosed = RewardVideoClosed
        });

        AdsManager.Instance.LoadReward();
    }

    public void DelayRewardLoad(float time)
    {
        StartCoroutine(DoDelayRewardLoad(time));
    }

    IEnumerator DoDelayRewardLoad(float time)
    {
        yield return new WaitForSeconds(time);

        RequestRewardVideo();
    }
    #region RewardVideo callback handlers

    public void RewardVideoLoaded()
    {
        DConsole.Log("Board RewardLoaded");

        //_rewardButton.interactable = true;
    }

    public void RewardVideoRewarded()
    {
        DConsole.Log("Board RewardEarned");

        UnityMainThread.wkr.AddJob(() =>
        {
            AdsManager.Instance.IsRewarded = true;

            //_rewardButton.interactable = false;

            AdsManager.Instance.AddRewardAmount();
        });
    }

    public void RewardVideoFailedToLoad()
    {
        DConsole.Log("Board RewardFailed");
        UnityMainThread.wkr.AddJob(() =>
        {
            DelayRewardLoad(0.1f);
        });
        // _rewardButton.interactable = true;
    }

    public void RewardVideoClosed()
    {
        DConsole.Log("Board RewardClosed");

        UnityMainThread.wkr.AddJob(() =>
        {
            DelayRewardLoad(0.1f);
        });
    }

    #endregion
}
