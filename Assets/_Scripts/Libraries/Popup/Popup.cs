using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

//22/02/2021

public enum AnimType { Left, Right, Top, Down }

[System.Serializable]
public class AnimationSettings
{
    public bool enable = true;
    public bool animateBackground = false;

    public AnimType openAnimation = AnimType.Left;
    public float openDuration = 0.2f;
    public float openDelay = 0f;

    public AnimType closeAnimation = AnimType.Right;
    public float closeDuration = 0.2f;
}

// Limitation only one type of popup can be shown at a time
public class Popup : MonoBehaviour
{
    [SerializeField] protected PopupID _popupID = PopupID.None;
    [SerializeField] private GameObject _popupContainer = null;
    [SerializeField] private Button[] _buttonClose = null;
    [SerializeField] private bool _setGamePause = true;
    [SerializeField] private bool _onBackPressClose = false;
    [SerializeField] private bool _onCloseSound = true;

    [SerializeField] private AnimationSettings _animationSettings = null;

    protected RectTransform _rt;
    protected float _offset = 1f;
    private Transform _popup;

    public PopupID popupID => _popupID;
    public bool setGamePause => _setGamePause;
    public bool onBackPressClose => _onBackPressClose;

    public virtual void OnStart() 
    {
        gameObject.SetActive(false);

        _popup = transform;
        if (!_animationSettings.animateBackground && _popupContainer != null)
            _popup = _popupContainer.transform;

        _rt = GetComponent<RectTransform>();

        ResetPosition();
    }

    public virtual void OnOpen() {}

    public virtual void OnClose() {}

    private void ResumeGame()
    {
        if (_setGamePause && PopupManager.Instance.CheckIfAllPopupClosed())
            Config.PauseGame(true);
    }

    public void Open()
    {
        gameObject.SetActive(true);
        gameObject.transform.SetAsLastSibling();

        if (_setGamePause)
            Config.PauseGame(true);

        if (_buttonClose != null && _buttonClose.Length > 0)
        {
            foreach (var item in _buttonClose)
            {
                if (item == null)
                    return;

                item.onClick.RemoveAllListeners();
                item.onClick.AddListener(() =>
                {
                    PopupManager.Instance.PopupClose(this);
                });
            }
        }

        if (_animationSettings.enable)
        {
            if (_animationSettings.openDelay > 0f)
                _popup.DOLocalMove(Vector3.zero, _animationSettings.openDuration).SetDelay(_animationSettings.openDelay);
            else
                _popup.DOLocalMove(Vector3.zero, _animationSettings.openDuration);
        }

        OnOpen();
    }

    public void Close()
    {
        if (_animationSettings.enable)
        {
            Vector2 pos = new Vector2(_rt.rect.width * _offset, 0);
            if (_animationSettings.closeAnimation == AnimType.Left)
                pos = new Vector2(_rt.rect.width * -_offset, 0);
            else if (_animationSettings.closeAnimation == AnimType.Top)
                pos = new Vector2(0, _rt.rect.height * _offset);
            else if (_animationSettings.closeAnimation == AnimType.Down)
                pos = new Vector2(0, _rt.rect.height * -_offset);

            _popup.DOLocalMove(pos, _animationSettings.closeDuration).OnComplete(() =>
            {
                gameObject.SetActive(false);

                if (_animationSettings.openAnimation != _animationSettings.closeAnimation)
                    ResetPosition();
            });
        }
        else
            gameObject.SetActive(false);

        if (_onCloseSound)
            AudioManager.Click();

        ResumeGame();
        OnClose();
    }

    private void ResetPosition()
    {
        if (!_animationSettings.enable)
            return;

        if (_animationSettings.openAnimation == AnimType.Left)
        {
            //DConsole.Log(_popupID.ToString() + ": " + (_rt.rect.width * -_offset) + " " + _rt.rect.width);
            _popup.DOLocalMove(new Vector2(_rt.rect.width * -_offset, 0f), 0f);
        }
        else if (_animationSettings.openAnimation == AnimType.Right)
            _popup.DOLocalMove(new Vector2(_rt.rect.width * _offset, 0f), 0f);
        else if (_animationSettings.openAnimation == AnimType.Top)
            _popup.DOLocalMove(new Vector2(0f, _rt.rect.height * _offset), 0f);
        else if (_animationSettings.openAnimation == AnimType.Down)
            _popup.DOLocalMove(new Vector2(0f, _rt.rect.height * -_offset), 0f);
    }
}
