using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//22/02/2021

public enum PopupID { None, NoInternet, GameOver, GamePause, RateUs, AdFailedToLoad, GotReward, NoInternetAdRetry, Shop, Settings, DailyHint, UpdateAvailable, WatchVideo, PurchaseSuccess, OutOfMove }

public class PopupManager : Singleton<PopupManager>
{
    [SerializeField] private Popup[] _popups = null;
    [SerializeField] private PopupID _onBackPressOpenPopup = PopupID.None;
    [SerializeField] private string _onBackPressSceneChange = "";
    [SerializeField] private bool _onBackPressQuitGame = false;

    private Dictionary<int, Popup> _openStack = new Dictionary<int, Popup>();

    // Awake is important if we want to load any popup on Start
    /*protected override void Awake()
    {
        base.Awake();
        foreach (var item in _popups)
        {
            item.OnStart();
        }
    }*/

    // Start is important as rect.width and height (needed for reset position of Animations) is not correct at Awake()
    private void Start()
    {
        foreach (var item in _popups)
        {
            item.OnStart();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_openStack.Count > 0 && _openStack[_openStack.Count - 1] != null)
            {
                Popup popup = _openStack[_openStack.Count - 1];
                if (popup.onBackPressClose)
                    PopupClose(popup);
            }
            else if (_onBackPressOpenPopup != PopupID.None)
            {
                //AudioManager.Click();
                PopupOpen(_onBackPressOpenPopup);
            }
            else if (!string.IsNullOrEmpty(_onBackPressSceneChange) && !string.IsNullOrWhiteSpace(_onBackPressSceneChange))
            {
                //AudioManager.Click();
                UnityEngine.SceneManagement.SceneManager.LoadScene(_onBackPressSceneChange);
            }
            else if (_onBackPressQuitGame)
            {
                if (Alert.alertStack.Count > 0)
                    return;

                //AudioManager.Click();

                var buttons = new Dictionary<string, Alert.ButtonDelegate>() {
                    {"Yes", Application.Quit },
                    {"No", null }
                };

                new Alert("Quit?", "Do you want to Quit?", 1, null, buttons);
            }
        }
    }

    public Popup GetPopup(PopupID popupID)
    {
        foreach (Popup item in _popups)
        {
            if (item.popupID == popupID)
                return item;
        }

        return null;
    }

    public void PopupOpen(PopupID popupID, bool isDelay = false)
    {
        foreach (var item in _popups)
            if (item.popupID == popupID)
            {
                if (isDelay)
                    StartCoroutine(DelayPopupOpen(item));
                else
                    PopupOpen(item);

                return;
            }

        DConsole.LogWarning("Popup not found! Popup: " + popupID.ToString());
    }

    public void PopupOpen(Popup popup)
    {
        popup.Open();

        //DConsole.Log(_openStack.Count);
        _openStack.Add(_openStack.Count, popup);
    }

    public void PopupClose(PopupID popupID)
    {
        foreach (var item in _popups)
            if (item.popupID == popupID)
                PopupClose(item);
    }

    public void PopupCloseLast()
    {
        if (_openStack.Count > 0 && _openStack[_openStack.Count - 1] != null)
        {
            Popup popup = _openStack[_openStack.Count - 1];
            PopupClose(popup);
        }
    }

    public void PopupClose(Popup popup)
    {
        if (_openStack.ContainsKey(_openStack.Count - 1))
            _openStack.Remove(_openStack.Count - 1);

        popup.Close();
    }

    public bool CheckIfAllPopupClosed()
    {
        foreach (var item in _openStack.Values)
        {
            if (item.setGamePause)
            {
                //DConsole.Log(item.gameObject.name);
                return false;
            }
        }

        return true;
    }

    private IEnumerator DelayPopupOpen(Popup item)
    {
        yield return null;
        PopupOpen(item);
    }
}
