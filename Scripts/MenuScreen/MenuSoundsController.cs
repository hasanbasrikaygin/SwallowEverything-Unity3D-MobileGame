using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MenuSoundsController : MonoBehaviour
{


    [Header("Audio Source")]
    public AudioSource gameSfxAudioSource;
    public AudioSource buttonAudioSource;

    [Header("Audio Clip")]
    public AudioClip gameSfxClip;
    public AudioClip buttonClip;
    
    [Header("Volume Settings")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxButtonSlider;
    [SerializeField] private Slider sfxGameSlider;



    private void Start()
    {
          if (PlayerPrefs.HasKey("sfxGameVolume"))
            {
                LoadSfxGameVolume();
            }
            else
            {
                SetSfxGameVolume();
            }
            if (PlayerPrefs.HasKey("sfxButtonGameVolume"))
            {
                LoadSfxButtonVolume();
            }
            else
            {
                SetSfxButtonVolume();
            }
            if (PlayerPrefs.HasKey("musicVolume"))
            {
                LoadMusicVolume();
            }
            else
            {
                SetMusicVolume();
            }
        

    }

    public void SetSfxGameVolume()
    {
        float sfxVolume = sfxGameSlider.value;
        audioMixer.SetFloat("sfxGame", Mathf.Log10(sfxVolume) * 20);
        PlayerPrefs.SetFloat("sfxGameVolume", sfxVolume);
    }

    private void LoadSfxGameVolume()
    {
        sfxGameSlider.value = PlayerPrefs.GetFloat("sfxGameVolume");
        SetSfxGameVolume();
    }

    public void SetMusicVolume()
    {
        float musicVolume = musicSlider.value;
        audioMixer.SetFloat("music", Mathf.Log10(musicVolume) * 20);
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
        //BackgroundSoundController.Instance.musicAudioSource.volume = musicVolume; // Arka plan müziðinin ses seviyesini güncelle
    }

    private void LoadMusicVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        SetMusicVolume();
    }

    public void SetSfxButtonVolume()
    {
        float sfxVolume = sfxButtonSlider.value;
        audioMixer.SetFloat("sfxButtonGame", Mathf.Log10(sfxVolume) * 20);
        PlayerPrefs.SetFloat("sfxButtonGameVolume", sfxVolume);
    }

    private void LoadSfxButtonVolume()
    {
        sfxButtonSlider.value = PlayerPrefs.GetFloat("sfxButtonGameVolume");
        SetSfxButtonVolume();
    }

    public void GameSfxAudioSource(AudioClip clip)
    {
        gameSfxAudioSource.PlayOneShot(clip);
    }

    public void ButtonAudioSource(AudioClip clip)
    {
        buttonAudioSource.PlayOneShot(clip);
    }
    public void PlayButtonsSound()
    {
        buttonAudioSource.PlayOneShot(buttonClip);
    }
}
