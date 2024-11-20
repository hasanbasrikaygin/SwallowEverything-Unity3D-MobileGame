using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundSoundController : MonoBehaviour
{
    public static BackgroundSoundController Instance;

    public AudioSource musicAudioSource;
    public AudioClip musicClip;

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
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        StartBackgroundMusic();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "PlayGameScene" || scene.name == "LoadingScene")
        {
            if (musicAudioSource.isPlaying)
            {
                musicAudioSource.Stop();
            }
        }
        else
        {
            if (scene.name == "Home" && !musicAudioSource.isPlaying)
            {
                StartBackgroundMusic();
            }
            else if (!musicAudioSource.isPlaying)
            {
                musicAudioSource.UnPause();
            }
        }
    }

    private void StartBackgroundMusic()
    {
        if (musicClip != null && musicAudioSource != null)
        {
            musicAudioSource.clip = musicClip;
            musicAudioSource.loop = true;
            musicAudioSource.Play();
        }
    }
    public void StopBackgroundMusic()
    {
        musicAudioSource.Stop();
    }
}
