// PlayerHealth.cs

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public CharacterController characterController; // Karakter Controller bile�eni
   
    public float newHeight = 1.0f; // Yeni y�kseklik de�eri

    public static float maxHealth = 100f;
    public static float currentHealth;
    //public static DeadCamManager deadCamManager;

    public Animator animator;
    public Image filledImage; // Dolu durumu temsil eden image
    public Image emptyImage; // Bo� durumu temsil eden image
    [SerializeField] private GameObject player;
    
    [SerializeField] private PlayerInput playerInput;
    //public RigTransform rigComponent;
    public RigBuilder rb;
    [SerializeField] private Canvas thirdPersonCanvas;
    [SerializeField] private Canvas aimCanvas;
    [SerializeField] private Canvas deathCanvas;
    [SerializeField] private Canvas baseCanvas;
    [SerializeField] private GameObject bloodEffectPrefab; // Kan efekti prefab�
    [SerializeField] private GameObject healthEffectPrefab; // Kan efekti prefab�
    [SerializeField] ParticleSystem DebuffEffect;
    private bool isEffectRunning = false;
    [SerializeField] private int numberOfBloodEffects = 3; // Olu�turulacak kan efekti say�s�
    private bool hasSpawned = false; // Kan efekti olu�turuldu mu?


    private void Awake()
    {
        isEffectRunning = false;
        characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        Debug.Log("player: " + (player != null ? "Assigned" : "Not Assigned"));
        //  deadCamManager = FindObjectOfType<DeadCamManager>();
        currentHealth = maxHealth;
        deathCanvas.enabled = false;
    }

    public  void TakeDamage(float damage)
    {
        //currentHealth -= damage;
        UpdateHealthBar();
       

        if (currentHealth <= 0f)
        {
            //deadCamManager.EnabledKillCam();
            Die();
        }
    }
    public  void ResetHealth()
    {
        currentHealth = 100f;
        UpdateHealthBar();
        Debug.Log("cannlllarr gidiyorrrrrrr");
    }

    public  void AddHealth(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        UpdateHealthBar();
    }

    public static void Die()
    {
        // Oyuncunun �l�m�yle ilgili i�lemleri burada yapabilirsiniz.
        Debug.Log("Player Died");
    }

     public void  UpdateHealthBar()
    {
        float fillAmount = currentHealth / maxHealth;

        // Dolu durumu temsil eden Image'i g�ncelle
        filledImage.fillAmount = fillAmount;

        // Bo� durumu temsil eden Image'i g�ncelle
        emptyImage.fillAmount = 1 - fillAmount;
    }
    private bool isProcessed = false; // Bayrak tan�mland�
    public void Update()
    {
        if (currentHealth < 0f)
        {
            if (PlayerHealth.currentHealth < 0 &&  !isProcessed )
            {
                isProcessed = true; // ��lemi i�aretle
                
                ScoreManager.instance.UpdateEndTimerUI();
                ScoreManager.instance.isGameOver = true;
                //string playerName = PlayerPrefs.GetString("UserName", "Unknown");

               
                
               

                aimCanvas.enabled = false;
                thirdPersonCanvas.enabled = false;
                baseCanvas.enabled = false;
                StartCoroutine(ShowRespawnCanvasAfterDelay(3.5f));

                player.GetComponent<PlayerController>().isDead = true;
                
                if (characterController != null)
                {
                    characterController.enabled = false;
                }
                if (rb != null)
                {
                    rb.enabled = false;

                }

                GameManager.Instance.IsGameOver = true;

            }
            SwitchVCam switchVCam = FindObjectOfType<SwitchVCam>();
            if (switchVCam != null)
            {
                Vector3 deathPosition = FindObjectOfType<PlayerHealth>().transform.position; // Oyuncunun pozisyonunu al�n
                switchVCam.TriggerDeathCamera(deathPosition);
            }
            // Character Controller bile�enini g�ncelle
            characterController.height = newHeight;
            // Aiming layer'�n�n index'ini al�n
            int aimingLayerIndex = animator.GetLayerIndex("Aiming");

            // E�er layer bulunamad�ysa (-1 ise) ��k�� yap
            if (aimingLayerIndex == -1)
            {
                Debug.LogError("Aiming layer could not be found!");
                return;
            }

            // Aiming layer'�n�n a��rl���n� azalt�n (0.5 gibi bir de�er deneyebilirsiniz)
            // characterController.center.y = 0.5f;
            animator.SetLayerWeight(aimingLayerIndex, 0.1f);


            animator.SetBool("isDead", true);
            

        }
    }
    private IEnumerator ShowRespawnCanvasAfterDelay(float delay)
    {
        // Kan efekti hen�z olu�turulmad�ysa ve spawn noktas� tan�ml�ysa
       
        yield return new WaitForSeconds(1f);
        SpawnBloodEffect();
        yield return new WaitForSeconds(delay);
        deathCanvas.enabled = true;
        yield return new WaitForSeconds(2f);
        
    }
    private void SpawnBloodEffect()
    {
        if (!hasSpawned && player.transform != null)
        {
            // Kan efekti say�s�na kadar d�ng� olu�tur
            for (int i = 0; i < numberOfBloodEffects; i++)
            {
                // Kan efekti prefab�n� farkl� pozisyonlarda olu�tur
                Vector3 offset = new Vector3(Random.Range(-.1f, .2f), 0f, Random.Range(-.1f, .2f));
                Instantiate(bloodEffectPrefab, player.transform.position + offset, player.transform.rotation);
            }
            // Kan efekti olu�turuldu�unu i�aretle
            hasSpawned = true;
        }
    }




   

    IEnumerator EnableEffectForDuration(float duration)
    {
        isEffectRunning = true; // Efekt ba�lad���nda kontrol de�i�kenini true yap

        DebuffEffect.Play(); // Partik�l sistemi ba�lat�l�r
        yield return new WaitForSeconds(duration);
        DebuffEffect.Stop(); // Partik�l sistemi durdurulur

        isEffectRunning = false; // Efekt bitti�inde kontrol de�i�kenini false yap
    }
}
