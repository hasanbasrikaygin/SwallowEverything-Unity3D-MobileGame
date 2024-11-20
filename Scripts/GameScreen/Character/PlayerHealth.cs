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
    public CharacterController characterController; // Karakter Controller bileþeni
   
    public float newHeight = 1.0f; // Yeni yükseklik deðeri

    public static float maxHealth = 100f;
    public static float currentHealth;
    //public static DeadCamManager deadCamManager;

    public Animator animator;
    public Image filledImage; // Dolu durumu temsil eden image
    public Image emptyImage; // Boþ durumu temsil eden image
    [SerializeField] private GameObject player;
    
    [SerializeField] private PlayerInput playerInput;
    //public RigTransform rigComponent;
    public RigBuilder rb;
    [SerializeField] private Canvas thirdPersonCanvas;
    [SerializeField] private Canvas aimCanvas;
    [SerializeField] private Canvas deathCanvas;
    [SerializeField] private Canvas baseCanvas;
    [SerializeField] private GameObject bloodEffectPrefab; // Kan efekti prefabý
    [SerializeField] private GameObject healthEffectPrefab; // Kan efekti prefabý
    [SerializeField] ParticleSystem DebuffEffect;
    private bool isEffectRunning = false;
    [SerializeField] private int numberOfBloodEffects = 3; // Oluþturulacak kan efekti sayýsý
    private bool hasSpawned = false; // Kan efekti oluþturuldu mu?


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
        // Oyuncunun ölümüyle ilgili iþlemleri burada yapabilirsiniz.
        Debug.Log("Player Died");
    }

     public void  UpdateHealthBar()
    {
        float fillAmount = currentHealth / maxHealth;

        // Dolu durumu temsil eden Image'i güncelle
        filledImage.fillAmount = fillAmount;

        // Boþ durumu temsil eden Image'i güncelle
        emptyImage.fillAmount = 1 - fillAmount;
    }
    private bool isProcessed = false; // Bayrak tanýmlandý
    public void Update()
    {
        if (currentHealth < 0f)
        {
            if (PlayerHealth.currentHealth < 0 &&  !isProcessed )
            {
                isProcessed = true; // Ýþlemi iþaretle
                
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
                Vector3 deathPosition = FindObjectOfType<PlayerHealth>().transform.position; // Oyuncunun pozisyonunu alýn
                switchVCam.TriggerDeathCamera(deathPosition);
            }
            // Character Controller bileþenini güncelle
            characterController.height = newHeight;
            // Aiming layer'ýnýn index'ini alýn
            int aimingLayerIndex = animator.GetLayerIndex("Aiming");

            // Eðer layer bulunamadýysa (-1 ise) çýkýþ yap
            if (aimingLayerIndex == -1)
            {
                Debug.LogError("Aiming layer could not be found!");
                return;
            }

            // Aiming layer'ýnýn aðýrlýðýný azaltýn (0.5 gibi bir deðer deneyebilirsiniz)
            // characterController.center.y = 0.5f;
            animator.SetLayerWeight(aimingLayerIndex, 0.1f);


            animator.SetBool("isDead", true);
            

        }
    }
    private IEnumerator ShowRespawnCanvasAfterDelay(float delay)
    {
        // Kan efekti henüz oluþturulmadýysa ve spawn noktasý tanýmlýysa
       
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
            // Kan efekti sayýsýna kadar döngü oluþtur
            for (int i = 0; i < numberOfBloodEffects; i++)
            {
                // Kan efekti prefabýný farklý pozisyonlarda oluþtur
                Vector3 offset = new Vector3(Random.Range(-.1f, .2f), 0f, Random.Range(-.1f, .2f));
                Instantiate(bloodEffectPrefab, player.transform.position + offset, player.transform.rotation);
            }
            // Kan efekti oluþturulduðunu iþaretle
            hasSpawned = true;
        }
    }




   

    IEnumerator EnableEffectForDuration(float duration)
    {
        isEffectRunning = true; // Efekt baþladýðýnda kontrol deðiþkenini true yap

        DebuffEffect.Play(); // Partikül sistemi baþlatýlýr
        yield return new WaitForSeconds(duration);
        DebuffEffect.Stop(); // Partikül sistemi durdurulur

        isEffectRunning = false; // Efekt bittiðinde kontrol deðiþkenini false yap
    }
}
