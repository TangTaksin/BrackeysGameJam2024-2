using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Music")]
    public AudioClip music_Bg;
    public AudioClip ambient_Rain;


    [Header("SFX")]
    public AudioClip pcikUp_sfx;
    public AudioClip useItem_sfx;
    public AudioClip openDoor_sfx;
    public AudioClip closeDoor_sfx;
    public AudioClip[] walking_sfx;
    public AudioClip morning_sfx;
    public AudioClip[] bound_sfx;



    [Header("Settings")]
    [SerializeField] public float minTimeBetween = 0.3f;
    [SerializeField] public float maxTimeBetween = 0.6f;

    [SerializeField] public float minTimeBetweenBound = 0.3f;
    [SerializeField] public float maxTimeBetweenBound = 0.6f;

    private float timeSinceLast;
    private int StepCount = 0;

    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        PlayMusic(music_Bg);

    }


    public void PlayMusic(AudioClip audioClip)
    {
        musicSource.clip = audioClip;
        musicSource.Play();

    }

    public void PlaySFX(AudioClip audioClip)
    {
        sfxSource.PlayOneShot(audioClip);
    }

    public void PlayWalkingSFX()
    {
        if (Time.time - timeSinceLast >= Random.Range(minTimeBetween, maxTimeBetween))
        {
            AudioClip sliceStepSound = walking_sfx[Random.Range(0, walking_sfx.Length)];
            sfxSource.PlayOneShot(sliceStepSound);
            timeSinceLast = Time.time;
        }
    }

    public void PlayBoundSFX()
    {
        if (Time.time - timeSinceLast >= Random.Range(minTimeBetweenBound, maxTimeBetweenBound))
        {
            AudioClip sliceStepSound = bound_sfx[Random.Range(0, bound_sfx.Length)];
            sfxSource.PlayOneShot(sliceStepSound);
            timeSinceLast = Time.time;
        }
    }
}