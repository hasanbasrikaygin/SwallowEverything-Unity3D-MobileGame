using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundsController : MonoBehaviour
{

    private AudioSource audioSource;
    public AudioClip backgroundClip;
    [SerializeField] Button sound;
    [SerializeField] Image openImage;
    [SerializeField] Image closeImage;
    private bool soundIsOpen = true;

    void Awake()
    {
        Debug.Log(gameObject.name + " Sounds Controller");
        closeImage.enabled = false;
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.clip = backgroundClip;
        DontDestroyOnLoad(gameObject);
        // SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        // Eðer oyun sahnesi (örneðin, "GameScene") yüklendiyse müziði durdur
        if (scene.name == "PlayGameScene")
        {
            PlaySounds(false);
        }
        else
        {
            // Diðer sahnelerde müziði baþlat
            PlaySounds(true);
        }
    }
    public void PlaySoundButton()
    {
        soundIsOpen = !soundIsOpen;

        openImage.enabled = soundIsOpen;
        closeImage.enabled = !soundIsOpen;

        PlaySounds(soundIsOpen);
    }

    private void PlaySounds(bool play)
    {
        if (audioSource != null)
        {
            if (play)
            {
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
            }
            else
            {
                if (audioSource.isPlaying)
                {
                    audioSource.Stop();
                }
            }
        }
        else
        {
            Debug.LogError("audioSource is null. Check if SoundsController is properly initialized.");
        }
    }

    public bool SoundIsOpen
    {
        get { return soundIsOpen; }
    }
}
