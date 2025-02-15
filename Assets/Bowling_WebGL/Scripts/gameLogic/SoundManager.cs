using meetme.mics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviourSingleton<SoundManager>
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Music")]
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private bool playMusicOnStart = true;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip ballLaunchSound;
    [SerializeField] private AudioClip pinSound;  // Combined sound for hit/collapse

    [Header("Volume Settings")]
    [SerializeField] private float sfxVolume = 1f;
    [SerializeField] private float musicVolume = 0.5f;
    
    private bool isMusicPlaying = false;

    private bool IsValid => this != null && musicSource != null && sfxSource != null;

    private void Awake()
    {
        CreateSingleton(true);  // Set to true to persist between scenes
        SetupAudioSources();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
            if (musicSource != null)
                musicSource.Stop();
            if (sfxSource != null)
                sfxSource.Stop();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Ensure music continues playing through scene changes
        if (!isMusicPlaying && playMusicOnStart)
        {
            PlayBackgroundMusic();
        }
    }

    private void Start()
    {
        if (playMusicOnStart)
        {
            PlayBackgroundMusic();
        }
    }

    private void SetupAudioSources()
    {
        if (musicSource == null)
            musicSource = gameObject.AddComponent<AudioSource>();
        if (sfxSource == null)
            sfxSource = gameObject.AddComponent<AudioSource>();

        // Setup music source
        musicSource.loop = true;
        musicSource.volume = musicVolume;
        musicSource.playOnAwake = false;
        if (backgroundMusic != null)
            musicSource.clip = backgroundMusic;

        // Setup SFX source
        sfxSource.volume = sfxVolume;
        sfxSource.playOnAwake = false;
    }

    public void PlayBackgroundMusic()
    {
        if (musicSource != null && backgroundMusic != null && !isMusicPlaying)
        {
            musicSource.Play();
            isMusicPlaying = true;
        }
    }

    public void StopBackgroundMusic()
    {
        if (musicSource != null && isMusicPlaying)
        {
            musicSource.Stop();
            isMusicPlaying = false;
        }
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        if (musicSource != null)
            musicSource.volume = musicVolume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        if (sfxSource != null)
            sfxSource.volume = sfxVolume;
    }

    public void PlayBallLaunch()
    {
        if (IsValid && ballLaunchSound != null)
            sfxSource.PlayOneShot(ballLaunchSound);
    }

    public void PlayPinSound()
    {
        if (IsValid && pinSound != null)
            sfxSource.PlayOneShot(pinSound);
    }

    // Remove unused methods
    public void PlayPinHit() => PlayPinSound();
    public void PlayPinCollapse() => PlayPinSound();

    public void RestartMusic()
    {
        if (IsValid)
        {
            musicSource.Stop();
            musicSource.Play();
            isMusicPlaying = true;
        }
    }
}
