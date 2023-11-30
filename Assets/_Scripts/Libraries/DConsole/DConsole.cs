using System;
using UnityEngine;
using UnityEngine.UI;

//22/02/2021
//02/03/2021
//25/12/2021

[DefaultExecutionOrder(-50)]
public class DConsole : SingletonDontDestroy<DConsole>
{
    [SerializeField] private GameObject _panel = null;
    [SerializeField] private Text _text = null;
    [SerializeField] private Button _buttonClearConsole = null;
    [SerializeField] private Button _buttonClearPrefs = null;

    [Header("Time and Frame")]
    [SerializeField] private Slider _sliderTimeScale = null;
    [SerializeField] private Text _textTimeScale = null;
    [SerializeField] private int _frameRate = -1;

    [Header("Settings")]
    [SerializeField] private KeyCode _openByKey = KeyCode.Escape;

    private bool _isShow = false;
    static bool isExceptionHandlingSetup;

    public Text console { get { return _text; } set { _text = value; } }

    //public static DConsole Instance = null;

    protected override void Awake()
    {
        if (!Config.isDebug)
        {
            Destroy(this.gameObject);
            return;
        }

        base.Awake();

        /*if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(this.gameObject);*/        

        _panel.SetActive(false);
        _text.text = "";

        _buttonClearConsole.onClick.RemoveAllListeners();
        _buttonClearConsole.onClick.AddListener(() => ClearConsole());

        _buttonClearPrefs.onClick.RemoveAllListeners();
        _buttonClearPrefs.onClick.AddListener(() => PlayerPrefs.DeleteAll());

        SetupExceptionHandling();

        _sliderTimeScale.value = 1;
        _sliderTimeScale.onValueChanged.RemoveAllListeners();
        _sliderTimeScale.onValueChanged.AddListener(ChangeTimeScale);
        _textTimeScale.text = Time.timeScale.ToString();

        Application.targetFrameRate = _frameRate;

        Log("DConsole Initialized!");
    }

    private void Update()
    {
        if (Input.GetKeyDown(_openByKey))
        {
            if (!_isShow)
            {
                OpenConsole();
            }
            else
            {
                _panel.SetActive(false);
                _isShow = false;
            }
        }
    }

    private void OpenConsole()
    {
        _panel.SetActive(true);
        _isShow = true;
    }

    private void ClearConsole()
    {
        _text.text = "";
    }

    public static void SetupExceptionHandling()
    {
        if (!isExceptionHandlingSetup)
        {
            isExceptionHandlingSetup = true;
            Application.logMessageReceived += HandleException;
        }
    }

    private static void HandleException(string condition, string stackTrace, LogType type)
    {
        if (type == LogType.Exception)
        {
            LogError(condition + "\n" + stackTrace);
        }
    }

    public static void Log(string msg)
    {
        if (Config.isDebug)
        {
            Instance.console.text += msg + "\n";
            Debug.Log(msg);
        }
    }

    public static void Log(string msg, UnityEngine.Object context)
    {
        if (Config.isDebug)
        {
            Instance.console.text += msg + "\n";
            Debug.Log(msg, context);
        }
    }

    public static void LogFormat(string msg, params object[] args)
    {
        if (Config.isDebug)
        {
            Instance.console.text += string.Format(msg, args) + "\n";
            Debug.LogFormat(msg, args);
        }
    }

    public static void LogError(string msg)
    {
        if (Config.isDebug)
        {
            Instance.console.text += "<color=#ff0000>" + msg + "</color>\n";
            Instance.OpenConsole();
            Debug.LogError(msg);
        }
    }

    public static void LogError(string msg, UnityEngine.Object context)
    {
        if (Config.isDebug)
        {
            Instance.console.text += "<color=#ff0000>" + msg + "</color>\n";
            Instance.OpenConsole();
            Debug.LogError(msg, context);
        }
    }

    public static void LogErrorFormat(string msg, params object[] args)
    {
        if (Config.isDebug)
        {
            Instance.console.text += "<color=#ff0000>" + msg + "</color>\n";
            Instance.OpenConsole();
            Debug.LogErrorFormat(msg, args);
        }
    }

    public static void LogException(Exception e)
    {
        if (Config.isDebug)
        {
            Instance.console.text += "<color=#ff0000>" + e.Message + "</color>\n";
            Instance.OpenConsole();
            Debug.LogException(e);
        }
    }

    public static void LogWarning(string msg)
    {
        if (Config.isDebug)
        {
            Instance.console.text += "<color=orange>" + msg + "</color>\n";
            Instance.OpenConsole();
            Debug.LogWarning(msg);
        }
    }

    public static void LogWarning(string msg, UnityEngine.Object context)
    {
        if (Config.isDebug)
        {
            Instance.console.text += "<color=orange>" + msg + "</color>\n";
            Instance.OpenConsole();
            Debug.LogWarning(msg, context);
        }
    }

    public static void LogWarningFormat(string msg, params object[] args)
    {
        if (Config.isDebug)
        {
            Instance.console.text += "<color=orange>" + msg + "</color>\n";
            Instance.OpenConsole();
            Debug.LogWarningFormat(msg, args);
        }
    }

    private void ChangeTimeScale(float value)
    {
        float time = Mathf.Round(value * 10f) / 10f;
        Time.timeScale = time == 0 ? 0.1f : time;

        if (_textTimeScale != null)
            _textTimeScale.text = Time.timeScale.ToString();
    }
}
