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

        // E�er oyun sahnesi (�rne�in, "GameScene") y�klendiyse m�zi�i durdur
        if (scene.name == "PlayGameScene")
        {
            PlaySounds(false);
        }
        else
        {
            // Di�er sahnelerde m�zi�i ba�lat
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
