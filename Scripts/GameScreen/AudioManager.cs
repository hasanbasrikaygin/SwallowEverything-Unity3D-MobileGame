using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    public AudioSource runAudioSource;
    public AudioSource jumpAudioSource;
    public AudioSource gunAudioSource;
    public AudioSource reloadAudioSource;

    public AudioSource playerDeadAudioSource;
    public AudioSource gameOverAudioSource;
    public AudioSource gameWinAudioSource;
    public AudioSource gameStartAudioSource;
    public AudioSource playerTakeDamageAudioSource;
    public AudioSource settingsOpenAudioSource;
    public AudioSource settingsCloseAudioSource;
    public AudioSource buildingDestroyAudioSource;
    public AudioSource buildingDamageAudioSource;
    public AudioSource takeHealAudioSource;
    public AudioSource takeBulletAudioSource;
    public AudioSource takeSpeedBoosterAudioSource;
    public AudioSource takeJumpBoosterAudioSource;
    public AudioSource windAudioSource;
    public AudioSource buttonPositionSaveAudioSource;



    [Header("Audio Clip")]
    public AudioClip runClip;
    public AudioClip jumpClip;
    public AudioClip gunClip;
    public AudioClip reloadClip;
    
    public AudioClip playerDeadClip;
    public AudioClip gameOverClip;
    public AudioClip gameWinClip;
    public AudioClip gameStartClip;
    public AudioClip playerTakeDamageClip;
    public AudioClip settingsOpenClip;
    public AudioClip settingsCloseClip;
    public AudioClip buildingDamageClip;
    public AudioClip buildingDestroyClip;
    public AudioClip takeHealClip;
    public AudioClip takeBulletClip;
    public AudioClip takeSpeedBoosterClip;
    public AudioClip takeJumpBoosterClip;
    public AudioClip buttonPositionSaveClip;


    [SerializeField] private GameObject settingsPanel;
    void Start()
    {

    }
    public void PlaySound(AudioClip clip)
    {

    }
    public void SettingsOpenAudioSource()
    {
        if(settingsPanel.activeSelf)
        settingsOpenAudioSource.PlayOneShot(settingsOpenClip);
        else
        settingsCloseAudioSource.PlayOneShot(settingsCloseClip);
    }
    public void SettingsCloseAudioSource()
    {
        settingsCloseAudioSource.PlayOneShot(settingsCloseClip);
    }
    public void RunAudioSource()
    {
        if (!runAudioSource.isPlaying)
        {
            runAudioSource.clip = runClip;
            runAudioSource.Play();
        }
    }
    public void GunAudioSource()
    {
        gunAudioSource.PlayOneShot(gunClip);
    }
    public void ReloadAudioSource()
    {
        reloadAudioSource.PlayOneShot(reloadClip);
    }
    public void JumpAudioSource()
    {
        jumpAudioSource.PlayOneShot(jumpClip);
    }
    public void PlayerDeadAudioSource()
    {
        playerDeadAudioSource.PlayOneShot(playerDeadClip);
    }    public void ButtonPositionSaveAudioSource()
    {
        buttonPositionSaveAudioSource.PlayOneShot(buttonPositionSaveClip);
    }
    public void GameOverAudioSource()
    {
        runAudioSource.Stop();
        gameOverAudioSource.PlayOneShot(gameOverClip);
    } 
    public void GameWinAudioSource()
    {
        gameWinAudioSource.PlayOneShot(gameWinClip);
    }
    public void GameStartAudioSource()
    {
        gameStartAudioSource.PlayOneShot(gameStartClip);
    }
    public void PlayerTakeDamageAudioSource()
    {
        playerTakeDamageAudioSource.PlayOneShot(playerTakeDamageClip);
    }    
    public void BuildingDamageAudioSource()
    {
        buildingDamageAudioSource.PlayOneShot(buildingDamageClip);
    }    
    public void BuildingDestroyAudioSource()
    {
        buildingDestroyAudioSource.PlayOneShot(buildingDestroyClip);
    }  
    public void TakeHealAudioSource()
    {
        takeHealAudioSource.PlayOneShot(takeHealClip);
    }   
    public void TakeBulletAudioSource()
    {
        takeBulletAudioSource.PlayOneShot(takeBulletClip);
    }  
    public void TakeSpeedBoosterAudioSource()
    {
        takeSpeedBoosterAudioSource.PlayOneShot(takeSpeedBoosterClip);
    }   
    public void TakeJumpBoosterAudioSource()
    {
        takeJumpBoosterAudioSource.PlayOneShot(takeJumpBoosterClip);
    }


    // Ses seviyesini azaltan korutin
    private IEnumerator FadeOutCoroutine()
    {
        float targetVolume = 0.001f;
        float startVolume = windAudioSource.volume;
        float elapsedTime = 0.0f;

        while (elapsedTime < 2f)
        {
            elapsedTime += Time.deltaTime;
            windAudioSource.volume = Mathf.Lerp(startVolume, targetVolume, elapsedTime / 1.5f);
            yield return null;
        }

        windAudioSource.volume = targetVolume; // Ses seviyesini tam olarak hedef seviyeye ayarlayýn
    }
}