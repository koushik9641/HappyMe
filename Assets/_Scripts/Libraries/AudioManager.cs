using UnityEngine;
using UnityEngine.UI;

public enum AudioTag { None, Click, Drag, Drop, Background }

[System.Serializable]
public class AudioClips
{
    [SerializeField] private AudioTag _audioTag = AudioTag.None;
    [SerializeField] private AudioClip _clip = null;
    [SerializeField] private float _volume = 1f;

    public AudioTag audioTag => _audioTag;
    public AudioClip clip => _clip;
    public float volume => _volume;
}

public class AudioManager : SingletonDontDestroy<AudioManager>
{
	public AudioClips[] audioClips;

    public bool isMusicOnAwake = true;
    public bool isSKFOnAwake = true;

    private static bool _isSFX = true;
    private static bool _isMusic = true;

    private AudioSource _audioSource;
    private AudioSource _audioSourceBackground;

    public static bool isSFX => _isSFX;

    protected override void Awake()
    {
        base.Awake();

        GameObject go = new GameObject("Audio Source");
        go.transform.SetParent(transform);

        _audioSource = (AudioSource) go.AddComponent(typeof(AudioSource));
        _audioSource.playOnAwake = false;


        go = new GameObject("Audio Source Background");
        go.transform.SetParent(transform);

        _audioSourceBackground = (AudioSource) go.AddComponent(typeof(AudioSource));
        _audioSourceBackground.playOnAwake = false;
        _audioSourceBackground.loop = true;

        _isSFX = PlayerPrefs.GetInt(KeyName.settingSFX, Instance.isSKFOnAwake ? 1 : 0) == 1;
        _isMusic = PlayerPrefs.GetInt(KeyName.settingMusic, Instance.isMusicOnAwake ? 1 : 0) == 1;
    }

    public void Start()
    {
        if (Config.isFirstTime)
            FirstTimeInit();

        Background();
    }

    // should be called for the first time the user's installs the app
    public static void FirstTimeInit()
    {
        if (!PlayerPrefs.HasKey(KeyName.settingSFX))
            PlayerPrefs.SetInt(KeyName.settingSFX, Instance.isSKFOnAwake ? 1 : 0);

        if (!PlayerPrefs.HasKey(KeyName.settingMusic))
            PlayerPrefs.SetInt(KeyName.settingMusic, Instance.isMusicOnAwake ? 1 : 0);
    }

    public static void Click()
    {
        if (_isSFX)
            Play(AudioTag.Click);
    }

    public static void Background()
    {
        if (Instance._audioSourceBackground == null)
        {
            if (_isMusic)
                DConsole.LogWarning("Audio Background Source not found!");
        }
        else
        {
            AudioClips audio = Instance.GetClip(AudioTag.Background);
            if (audio != null)
            {
                Instance._audioSourceBackground.clip = audio.clip;
                Instance._audioSourceBackground.volume = audio.volume;

                if (_isMusic)
                    Instance._audioSourceBackground.Play();
            }
        }
    }

    public void ToggleSFX(bool flag)
    {
        _isSFX = flag;
    }

    public void ToggleMusic(bool flag)
    {
        // this will not replay if the music is already playing
        if (_isMusic == flag)
            return;

        _isMusic = flag;

        if (_isMusic)
            _audioSourceBackground.Play();
        else
            _audioSourceBackground.Stop();
    }
	
	public static void Play(AudioTag tag)
	{
		if ( ! _isSFX)
			return;

        AudioClips audio = Instance.GetClip(tag);
        if (audio != null)
        {
            Instance._audioSource.clip = audio.clip;
            Instance._audioSource.volume = audio.volume;
            Instance._audioSource.Play();
        }
	}

    private AudioClips GetClip(AudioTag tag)
    {
        if (audioClips != null)
        foreach (var item in audioClips)
        {
            if (item.audioTag == tag)
                return item;
        }

        DConsole.LogWarning("Audio Clip not found! Clip: " + tag.ToString());
        return null;
    }
}
