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
            Debug.LogError("audioManager bileþen atanmadý!");
            return;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && !isEffectRunning)
        {
            audioManager.TakeHealAudioSource();
            StartCoroutine(EnableEffectForDuration(2f));
            //Debug.Log("Alýnaan can " +  kidHealth);
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
        isEffectRunning = true; // Efekt baþladýðýnda kontrol deðiþkenini true yap

        HealthEffect.Play(); // Partikül sistemi baþlatýlýr
        yield return new WaitForSeconds(duration);
        HealthEffect.Stop(); // Partikül sistemi durdurulur

        isEffectRunning = false; // Efekt bittiðinde kontrol deðiþkenini false yap
        this.gameObject.SetActive(false);
    }
}
