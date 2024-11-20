using System.Collections;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    
    [SerializeField] ParticleSystem BuffEffect;
    private bool isEffectRunning = false;
    public AudioManager audioManager;
    private void Awake()
    {
        if (audioManager == null)
        {
            Debug.LogError("audioManager bile�en atanmad�!");
            return;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "Player" && !isEffectRunning)
        {
            audioManager.TakeBulletAudioSource();
            //StartCoroutine(EnableEffectForDuration(2f));
            this.gameObject.SetActive(false);
            AddBullets();
        }
    }

    void AddBullets()
    {
        if (Bullet.bulletCount == 0)
        {
            Bullet.bulletCount += 10;
            Bullet.spareBulletCount += 10;
        }
        else
        {
            Bullet.spareBulletCount += 20;
        }
    }

    IEnumerator EnableEffectForDuration(float duration)
    {
        isEffectRunning = true; // Efekt ba�lad���nda kontrol de�i�kenini true yap

        BuffEffect.Play(); // Partik�l sistemi ba�lat�l�r
        yield return new WaitForSeconds(duration);
        BuffEffect.Stop(); // Partik�l sistemi durdurulur

        isEffectRunning = false; // Efekt bitti�inde kontrol de�i�kenini false yap
    }
}
