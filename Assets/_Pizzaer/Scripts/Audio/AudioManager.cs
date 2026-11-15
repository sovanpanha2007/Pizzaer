
using UnityEngine;
using UnityEngine.SceneManagement;

// Central controller for music and sound effects.
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Music")]
    public AudioSource musicSource;
    public AudioClip menuMusic;
    public AudioClip townMusic;

    [Header("Sound Effects")]
    public AudioSource sfxSource;

    [Header("SFX Clips")]
    public AudioClip pickupClip;
    public AudioClip deliverClip;
    public AudioClip wrongClip;
    public AudioClip winClip;
    public AudioClip clickClip;

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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        // Play music for the scene where the game starts.
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
        {
            PlayMusic(menuMusic);
        }
        else if (scene.name == "Town")
        {
            PlayMusic(townMusic);
        }
    }

    // Background music
    public void PlayMusic(AudioClip music)
    {
        if (music == null || musicSource == null)
        {
            Debug.LogWarning("Music clip or Music Source is missing!");
            return;
        }

        if (musicSource.clip == music && musicSource.isPlaying)
            return;

        musicSource.Stop();
        musicSource.clip = music;
        musicSource.loop = true;
        musicSource.Play();
    }

    // Sound effects
    public void Play(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    public void PlayPickup() => Play(pickupClip);
    public void PlayDeliver() => Play(deliverClip);
    public void PlayWrong() => Play(wrongClip);
    public void PlayWin() => Play(winClip);
    public void PlayClick() => Play(clickClip);
}
