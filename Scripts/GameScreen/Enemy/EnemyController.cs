
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class EnemyController : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;
    public float health = 100;
    AudioManager audioManager;
    //Patroling // Patroling (Devriye Gezme) i�in gerekli de�i�kenler
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;
    int shootAnimation;
    int deadAnimation;
    int runAnimation;
    int idleAnimation;
    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    // public GameObject enemyBullet;
    [SerializeField] public float boostTimer = 5f;
    [SerializeField] public bool isSpeedBoosted = false;
    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    private Animator animator;
    [SerializeField] private GameObject bulletStartPoint;
    [SerializeField] private Transform enemyTarget;
    private Collider enemyCollider;
    private Collider capsuleCollider;
    private Transform transformy;
    private bool isDeadEffectWorking = false;
    [SerializeField] EnemyBulletPool enemyBulletPool;
    bool isEffectRunning = false;
    [SerializeField] private ParticleSystem debuffEffect;
    [SerializeField] private Canvas canvas;
    public float rotationSpeed = 2.25f; // D�n�� h�z�n� ayarlayabilece�iniz bir de�i�ken
    private Vector3 initialPosition;
    private Vector3 previousPosition;
    private float movementThreshold = 0.01f;
    [SerializeField] private float randomSpreadX;
    [SerializeField] private float randomSpreadY;
    public float patrolInterval = 20f; // Yeni hedef belirleme aral��� (saniye)
    private float timeSinceLastPatrol = 0f;
    private EnemyRagdoll enemyRagdoll;

    private void Awake()
    {
        player = GameObject.Find("PlayerSuit").transform;
        agent = GetComponent<NavMeshAgent>();
        shootAnimation = Animator.StringToHash("infantry_combat_shoot");
        deadAnimation = Animator.StringToHash("Dying");
        runAnimation = Animator.StringToHash("combat_run");
        idleAnimation = Animator.StringToHash("combat_idle");
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<Collider>();
        transformy = GetComponent<Transform>();
        audioManager = GameObject.FindWithTag("Audio").GetComponent<AudioManager>();
        enemyCollider = GetComponent<Collider>();
        initialPosition = transform.position;
        enemyRagdoll = GetComponent<EnemyRagdoll>();


        //Debug.Log(bulletStartPoint);
    }

    private void FixedUpdate()
    {
        //Check for sight and attack range // G�rme ve sald�r� menzillerini kontrol et
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange)
        {
            Patroling();

            // Yeni hedef belirleme kontrol�
            timeSinceLastPatrol += Time.fixedDeltaTime;
            if (timeSinceLastPatrol >= patrolInterval)
            {
               
                SearchWalkPoint();
                timeSinceLastPatrol = 0f;
            }
        } 
        //if (!playerInSightRange && !playerInAttackRange) Patroling();
        // oyuncu g�r�� ve sald�r� menzillerinde de�ilse, devriye modunu ba�lat

        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        // oyuncu g�r�� menzilindeyse ancak sald�r� menzilinde de�ilse, oyuncuyu takip etme modunu ba�lat

       
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
        // oyuncu hem g�r�� hem de sald�r� menzillerindeyse, sald�r� modunu ba�lat


        //float distanceToInitialPosition = Vector3.Distance(transform.position, initialPosition);
        //if (!playerInSightRange && Vector3.Distance(transform.position, initialPosition) < 10f)
        //{
        //    animator.SetBool("isRunning", false);
        //}
        float distance = Vector3.Distance(transform.position, previousPosition);
        if (distance < movementThreshold)
        {
            // D��man hareketsiz kald�, bu durumu ele al
            animator.SetBool("isWalking", false);
        }
        previousPosition = transform.position;
    }
    
    private void Patroling()

    {

        if (!walkPointSet) // yeni hedef yoksa
        {
            SearchWalkPoint();
            Invoke("ResetWalkPointSet", 3f);
            timeSinceLastPatrol = 0f; // Yeni hedef belirlendi�inde s�reyi s�f�rla
        }

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
            // Y�r�me noktas� belirlendiyse, agent'i o noktaya y�nlendir

            Vector3 distanceToWalkPoint = transform.position - walkPoint;
            // d��man�n mevcut pozisyonu ile belirlenen y�r�me noktas� aras�ndaki uzakl��� hesaplar

            if (distanceToWalkPoint.magnitude < 5f)
            {
                animator.SetBool("isWalking", false); // Y�r�me animasyonunu durdur
               // walkPointSet = false; // Y�r�me noktas�na ula��ld���nda, yeni bir nokta belirle
            }
            else
            {
               animator.SetBool("isWalking", true); // Y�r�me animasyonunu ba�lat
            }

        }


    }
    private void ResetWalkPointSet()
    {
        walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range // Rastgele bir y�r�me noktas� belirle
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        // Belirlenen noktan�n zemine temas etti�ini kontrol et
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
        walkPointSet = true;
        }
            
    }
    private void ChasePlayer()
    {
        animator.SetBool("isWalking",false);
		
		// Oyuncuyu takip etmek i�in agent'i g�ncelle
		if (agent.remainingDistance < agent.stoppingDistance)
        // d��man�n hedefine olan uzakl��� < d��man�n durmas� gereken mesafe
        {
            animator.SetBool("isRunning", false);
        }
        else
        {
            animator.SetBool("isRunning", true);
        }
        agent.SetDestination(player.position);

    }
  private void AttackPlayer()
  {
      //animator.SetBool("isRunning", false);
      animator.CrossFade(shootAnimation, 0.015f);
        //Make sure enemy doesn't move // D��man�n hareket etmemesini sa�la
       // d��man�n sald�r� animasyonunu oynat�rken yerinde durmas�n� sa�la
      agent.SetDestination(transform.position);
      // Oyuncuya do�ru d�n
     // transform.LookAt(player);

      if (!alreadyAttacked)
      {
          if (!isDeadEffectWorking)
          {
              GameObject g = enemyBulletPool.GetObject();
              EnemyBulletController bulletController = g.GetComponent<EnemyBulletController>();
                //bulletController.target = ApplyRandomDeviation(enemyTarget.position);

                //bulletController.target = enemyTarget.position ;
              if (g != null)
              {
                  g.transform.position = bulletStartPoint.transform.position;
                  g.transform.rotation = Quaternion.identity;
                  Rigidbody rb = g.GetComponent<Rigidbody>();
                  g.SetActive(true);
                  if (rb != null)
                  {
                        // Rigidbody rb = Instantiate(enemyBullet, bulletStartPoint.transform.position, Quaternion.identity).GetComponent<Rigidbody>();
                        //rb.AddForce(transform.forward * 20f, ForceMode.Impulse);
                        //rb.AddForce(transform.up * 8f, ForceMode.Impulse);
                        //float randomSpread = Random.Range(-5f, 5f); // Rastgele bir yay�lma a��s� belirle
                        //Vector3 shotDirection = Quaternion.Euler(0, randomSpread, 0) * transform.forward; // Yay�lma a��s�n� kullanarak yeni at�� y�n�n� hesapla
                        //rb.AddForce(shotDirection * 1f, ForceMode.Impulse);
                         //audioManager.EnemyGunAudioSource(audioManager.enemyGunClip);
                    }

                  else
                  {
                      Debug.LogError("Rigidbody component not found on the bullet GameObject.");
                  }
                  alreadyAttacked = true;
                  Invoke(nameof(ResetAttack), timeBetweenAttacks);
              }
              else
              {
                  Debug.LogError("Failed to get bullet object or bulletStartPoint is not set.");
              }
          }

      }
  }
    private Vector3 ApplyRandomDeviation(Vector3 originalTarget)
    {
        // Rastgele bir a�� se�, bu a�� mermi rotasyonunu etkileyecek
        float randomSpread = Random.Range(randomSpreadX, randomSpreadY);
        Vector3 shotDirection = (originalTarget - transform.position).normalized;
        Vector3 randomDeviation = Quaternion.Euler(0, randomSpread, 0) * shotDirection;
        return originalTarget + randomDeviation * 2f;
    }

    // Sald�r�y� s�f�rla
    private void ResetAttack()
  {
      alreadyAttacked = false;
  }
  public EnemyHealth enemyHealthComponent;
  public void TakeDamage(int damage)
  {
      //audioManager.EnemyTakeDamageAudioSource(audioManager.enemyTakeDamageClip);
      if (health <= 0)
      {
          isDeadEffectWorking = true;
          enemyCollider.enabled = false;
          canvas.enabled = false;
          ResetAttack();
          StartCoroutine("DeadEffect");
      }

  }
  IEnumerator DeadEffect()
  {
        animator.enabled = false;
        enemyRagdoll.EnableRagdoll();
        //DestroyEnemy();
        //audioManager.EnemyDeadAudioSource(audioManager.enemyDeadClip);

      yield return new WaitForSeconds(2.6f);
      Destroy(gameObject);

  }

  private void OnDrawGizmosSelected()
  {
      // Gizmo'lar� �izerek sald�r� ve g�rme menzillerini g�rselle�tir

      Gizmos.color = Color.red;
      Gizmos.DrawWireSphere(transform.position, attackRange);
      Gizmos.color = Color.yellow;
      Gizmos.DrawWireSphere(transform.position, sightRange);
  }
  void UpdateTargetPosition(Transform target , GameObject player)
  {
      // Hedefin pozisyonunu g�ncelle (player'�n pozisyonunu kullanarak �rne�in)
      if (player != null)
      {
         //target = player.transform.position;
      }
  }
    //void OnTriggerEnter(Collider other)
    //{
    //    // Karakterin �ocuk nesnelerini kontrol et
    //    foreach (Collider col  in childColliders)
    //    {
    //        // E�er �ocuk bir collider i�eriyorsa

    //        if (col != null && other.tag == "PlayerBullet")
    //        {
    //            // Etkile�ime ge�ilen collider'� belirle
    //            Debug.Log($"{col.name} ile etkile�ime ge�ildi!");
    //            // Burada �zel i�lemleri ger�ekle�tir
    //            TakeDamage(44);
    //            StartCoroutine(EnableEffectForDuration(2f));
    //        }
    //    }
    //}
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerBullet")
        {
           // TakeDamage(((int)SelectedWeapon.damage));
           // StartCoroutine(EnableEffectForDuration(2f));
        }
    }
        //}
    IEnumerator EnableEffectForDuration(float duration)        
    {
        isEffectRunning = true; // Efekt ba�lad���nda kontrol de�i�kenini true yap
        //Debug.Log(gameObject.name);
        debuffEffect.Play(); // Partik�l sistemi ba�lat�l�r
        yield return new WaitForSeconds(duration);
        debuffEffect.Stop();
        isEffectRunning = false;
    }

}
