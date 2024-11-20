using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstAidKidController : MonoBehaviour
{
    [SerializeField] private int kidHealth = 38;
    [SerializeField] ParticleSystem HealthEffect;
    private bool isEffectRunning = false;
    public AudioManager audioManager;
    public PlayerHealth health;
    private void Awake()
    {
        isEffectRunning = false;
        if (audioManager == null)
        {
            Debug.LogError("audioManager bile�en atanmad�!");
            return;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && !isEffectRunning)
        {
            audioManager.TakeHealAudioSource();
            StartCoroutine(EnableEffectForDuration(2f));
            //Debug.Log("Al�naan can " +  kidHealth);
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<BoxCollider>().enabled = false;
            
            AddHealth();
            health.AddHealth(kidHealth);
            
        }
    }
    void AddHealth()
    {

       PlayerController.playerHealth += kidHealth;
       // audioManager.PlayerTakeDamageAudioSource(audioManager.playerTakeDamageClip);
        //Debug.Log(playerHealth);
    }
    IEnumerator EnableEffectForDuration(float duration)
    {
        isEffectRunning = true; // Efekt ba�lad���nda kontrol de�i�kenini true yap

        HealthEffect.Play(); // Partik�l sistemi ba�lat�l�r
        yield return new WaitForSeconds(duration);
        HealthEffect.Stop(); // Partik�l sistemi durdurulur

        isEffectRunning = false; // Efekt bitti�inde kontrol de�i�kenini false yap
        this.gameObject.SetActive(false);
    }
}
