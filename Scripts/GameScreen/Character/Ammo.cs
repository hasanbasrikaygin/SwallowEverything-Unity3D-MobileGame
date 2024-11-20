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
            Debug.LogError("audioManager bileþen atanmadý!");
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
        isEffectRunning = true; // Efekt baþladýðýnda kontrol deðiþkenini true yap

        BuffEffect.Play(); // Partikül sistemi baþlatýlýr
        yield return new WaitForSeconds(duration);
        BuffEffect.Stop(); // Partikül sistemi durdurulur

        isEffectRunning = false; // Efekt bittiðinde kontrol deðiþkenini false yap
    }
}
