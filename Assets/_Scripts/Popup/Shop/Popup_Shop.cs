using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Popup_Shop : Popup
{
    [SerializeField] private bool _isRewardAdActive = false;
    [SerializeField] private GameObject _productRewardAd = null;
    //[SerializeField] private Button _buttonRewardAd = null;
    [SerializeField] private GameObject _adLoaded = null;
    [SerializeField] private GameObject _adLoading = null;
    [SerializeField] private ContentSizeFitter _parentContentSizeFilter = null;
    [SerializeField] private GameObject _lastBottomLine = null;
    [SerializeField] private Button _buttonClose2 = null;

    [SerializeField] private Text _errorText = null;

    public override void OnStart()
    {
        base.OnStart();

        if (_isRewardAdActive)
        {
            _productRewardAd.SetActive(true);
            _lastBottomLine.SetActive(true);

            //_buttonRewardAd.onClick.RemoveAllListeners();
            //_buttonRewardAd.onClick.AddListener(ShowAd);
        }
        else
        {
            _productRewardAd.SetActive(false);
            _lastBottomLine.SetActive(false);
        }

        _errorText.text = "";
        IAPManager.Instance.callbackAfterPurchase = AfterPurchase;
    }

    public override void OnOpen()
    {
        //_buttonRewardAd.interactable = true;

        if (_isRewardAdActive)
            _buttonClose2.onClick.AddListener(ShowWatchVideo);

        StartCoroutine(UpdateLayout());

        _errorText.gameObject.SetActive(false);
        if (IAPManager.Instance.error != "")
        {
            _errorText.text = IAPManager.Instance.error;
            _errorText.gameObject.SetActive(true);
        }
    }

    public void AfterPurchase(string productID)
    {
        var product = IAPManager.Instance.GetProductByID(productID);
        if (product != null)
        {
            string msg;
            if (product.productType == UnityEngine.Purchasing.ProductType.Consumable)
            {
                Config.UpdateCoins?.Invoke();

                msg = "Your purchase has been successful and you have got " + product.quantity + " coins!";
            }
            else
                msg = "Your purchase has been successful! Banner and forced ads removed!";

            Popup purchaseSuccess = PopupManager.Instance.GetPopup(PopupID.PurchaseSuccess);
            if (purchaseSuccess != null)
            {
                ((Popup_PurchaseSuccess)purchaseSuccess).textDescription.text = msg;
                StartCoroutine(ShowPurchaseSuccessDelay());
            }
        }

        PopupManager.Instance.PopupClose(this);
    }

    private IEnumerator UpdateLayout()
    {
        _parentContentSizeFilter.enabled = false;
        yield return new WaitForEndOfFrame();
        _parentContentSizeFilter.enabled = true;
    }

    private void ShowWatchVideo()
    {
        StartCoroutine(ShowWatchVideoDelay());
    }

    private IEnumerator ShowWatchVideoDelay()
    {
        yield return new WaitForSeconds(0.1f);
        PopupManager.Instance.PopupOpen(PopupID.WatchVideo);
    }

    private IEnumerator ShowPurchaseSuccessDelay()
    {
        yield return new WaitForSeconds(0.1f);
        PopupManager.Instance.PopupOpen(PopupID.PurchaseSuccess);
    }

    public void ShowAd()
    {
        if (AdsManager.Instance.IsRewardLoaded())
        {
            //_buttonRewardAd.interactable = false;

            PopupManager.Instance.PopupClose(this);

            AdsManager.Instance.ShowReward();
        }
        else if (AdsManager.Instance.rewardAdFailedReason != AdsManager.AdFailedType.None)
        {
            var buttons = new Dictionary<string, Alert.ButtonDelegate>() {
                {"Retry", MyRewardManager.Instance.RequestRewardVideo },
                {"Cancel", null }
            };

            if (AdsManager.Instance.rewardAdFailedReason == AdsManager.AdFailedType.NoInternet)
            {
                new Alert("No Internet", "Please check your internet connection!", 1, null, buttons);
            }
            else
            {
                new Alert("Ooops!", "Unable to load Ad please try again after sometime!", 1, null, buttons);
            }
        }
    }


    public void FreeHint()
    {
        AdsManager.Instance.ShowReward(() =>
        {
            DConsole.Log("Board RewardEarned");
            PlayerPrefs.SetInt("hintNum", PlayerPrefs.GetInt("hintNum", 150) + 150);
            GameManager.Instance.UseHint.interactable = true;
            GameManager.Instance.HintNumTxt.text = PlayerPrefs.GetInt("hintNum", 150).ToString();
        });
    }


    public void AdLoading()
    {
        _adLoaded.SetActive(false);
        _adLoading.SetActive(true);
        //_buttonRewardAd.interactable = false;
    }

    public void AdLoaded()
    {
        _adLoaded.SetActive(true);
        _adLoading.SetActive(false);
        //_buttonRewardAd.interactable = true;
    }
}
