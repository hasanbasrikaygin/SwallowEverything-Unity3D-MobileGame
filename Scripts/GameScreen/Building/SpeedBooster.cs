using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class SpeedBooster : MonoBehaviour
{

    private float originalPlayerSpeed;
    private float originalEnemySpeed = 6;
    private float originalRotationSpeed;
    [SerializeField] private GameObject player;
    private PlayerController playerController;
    private NavMeshAgent navMeshAgent;
    [SerializeField] private float boostDuration = 5f;
    [SerializeField] private float enemyBoostDuration = 5f;
    private EnemyController enemyController;
    public AudioManager audioManager;
    [SerializeField] ParticleSystem BuffEffect; 
    private bool isEffectRunning = false;
    private void Start()
    {
        isEffectRunning = false;
        if (audioManager == null)
        {
            Debug.LogError("audioManager bileþen atanmadý!");
            return;
        }
        playerController = player.GetComponent<PlayerController>();
        originalPlayerSpeed = playerController.playerSpeed;
        originalRotationSpeed = playerController.rotationSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Collision detected with tag: " + other.tag);

        if (other.CompareTag("Enemy"))
        {
            
            enemyController = other.GetComponent<EnemyController>();
             Transform firstChild = transform.GetChild(0);
             gameObject.GetComponent<BoxCollider>().enabled = false;
            
            if (firstChild != null)
            {
                firstChild.gameObject.SetActive(false);
            }

            if (!enemyController.isSpeedBoosted)
            {
                StartCoroutine(EnemySpeedBoost(other.gameObject , enemyController));
            }
            else
            {
                // Hýzlandýrýcýyý tekrar aldýðýnda süreyi uzat
                enemyController.boostTimer += enemyBoostDuration;
            }
        }
        if (other.CompareTag("Player"))
        {
            StartCoroutine(EnableEffectForDuration(2f));
            //Debug.Log( gameObject);
            audioManager.TakeSpeedBoosterAudioSource();
            gameObject.GetComponent<BoxCollider>().enabled = false;
            Transform firstChild = transform.GetChild(0);
            if (firstChild != null)
            {
                firstChild.gameObject.SetActive(false);
            }

            if (!playerController.isSpeedBoosted)
            {
                StartCoroutine(SpeedBoost());
            }
            else
            {
                // Hýzlandýrýcýyý tekrar aldýðýnda süreyi uzat
                playerController.boostTimer += boostDuration;
            }
        }

    }

    IEnumerator SpeedBoost()
    {
        playerController.isSpeedBoosted = true;
        playerController.boostTimer = Time.time + boostDuration;

        playerController.playerSpeed = 14;
        playerController.rotationSpeed = 3;

        while (Time.time < playerController.boostTimer)
        {
            yield return null;
        }

        playerController.isSpeedBoosted = false;
        playerController.playerSpeed = originalPlayerSpeed;
        playerController.rotationSpeed = originalRotationSpeed;
    }
    IEnumerator EnemySpeedBoost(GameObject obj , EnemyController enemyController)
    {
        
        enemyController.isSpeedBoosted = true;
        enemyController.boostTimer = Time.time + enemyBoostDuration;


        if (obj.GetComponent<NavMeshAgent>() != null)
        {

            obj.GetComponent<NavMeshAgent>().speed = 14;

            while (Time.time < enemyController.boostTimer)
            {
                //Debug.Log("While Loop - " + obj.name);
                yield return null;
            }

            enemyController.isSpeedBoosted = false;

            if (obj != null && obj.GetComponent<NavMeshAgent>() != null)
            {
                //Debug.Log(obj.name);
                //Debug.Log(obj.GetComponent<NavMeshAgent>().speed);
                obj.GetComponent<NavMeshAgent>().speed = originalEnemySpeed;
            }
            else
            {
                Debug.LogWarning("obj veya NavMeshAgent null.");
            }
            
        }
        

    }
    IEnumerator EnableEffectForDuration(float duration)
    {
        isEffectRunning = true; // Efekt baþladýðýnda kontrol deðiþkenini true yap

        BuffEffect.Play(); // Partikül sistemi baþlatýlýr
        yield return new WaitForSeconds(duration);
        BuffEffect.Stop(); // Partikül sistemi durdurulur
        //this.gameObject.SetActive(false);
        isEffectRunning = false; // Efekt bittiðinde kontrol deðiþkenini false yap
    }
}

