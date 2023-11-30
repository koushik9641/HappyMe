using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Popup_WatchVideo : Popup
{
    [SerializeField] private Button _buttonWatchAd = null;
    [SerializeField] private Text _textDescription = null;

    public override void OnOpen()
    {
        _textDescription.text = "Get FREE " + Config.addRewardAmount + " coins!";

        _buttonWatchAd.onClick.RemoveAllListeners();
        _buttonWatchAd.onClick.AddListener(ShowAd);
    }

    private void ShowAd()
    {
        ((Popup_Shop)PopupManager.Instance.GetPopup(PopupID.Shop)).ShowAd();
        PopupManager.Instance.PopupClose(this);
    }
}
