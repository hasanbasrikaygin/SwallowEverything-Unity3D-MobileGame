
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
    //Patroling // Patroling (Devriye Gezme) için gerekli deðiþkenler
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
    public float rotationSpeed = 2.25f; // Dönüþ hýzýný ayarlayabileceðiniz bir deðiþken
    private Vector3 initialPosition;
    private Vector3 previousPosition;
    private float movementThreshold = 0.01f;
    [SerializeField] private float randomSpreadX;
    [SerializeField] private float randomSpreadY;
    public float patrolInterval = 20f; // Yeni hedef belirleme aralýðý (saniye)
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
        //Check for sight and attack range // Görme ve saldýrý menzillerini kontrol et
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange)
        {
            Patroling();

            // Yeni hedef belirleme kontrolü
            timeSinceLastPatrol += Time.fixedDeltaTime;
            if (timeSinceLastPatrol >= patrolInterval)
            {
               
                SearchWalkPoint();
                timeSinceLastPatrol = 0f;
            }
        } 
        //if (!playerInSightRange && !playerInAttackRange) Patroling();
        // oyuncu görüþ ve saldýrý menzillerinde deðilse, devriye modunu baþlat

        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        // oyuncu görüþ menzilindeyse ancak saldýrý menzilinde deðilse, oyuncuyu takip etme modunu baþlat

       
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
        // oyuncu hem görüþ hem de saldýrý menzillerindeyse, saldýrý modunu baþlat


        //float distanceToInitialPosition = Vector3.Distance(transform.position, initialPosition);
        //if (!playerInSightRange && Vector3.Distance(transform.position, initialPosition) < 10f)
        //{
        //    animator.SetBool("isRunning", false);
        //}
        float distance = Vector3.Distance(transform.position, previousPosition);
        if (distance < movementThreshold)
        {
            // Düþman hareketsiz kaldý, bu durumu ele al
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
            timeSinceLastPatrol = 0f; // Yeni hedef belirlendiðinde süreyi sýfýrla
        }

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
            // Yürüme noktasý belirlendiyse, agent'i o noktaya yönlendir

            Vector3 distanceToWalkPoint = transform.position - walkPoint;
            // düþmanýn mevcut pozisyonu ile belirlenen yürüme noktasý arasýndaki uzaklýðý hesaplar

            if (distanceToWalkPoint.magnitude < 5f)
            {
                animator.SetBool("isWalking", false); // Yürüme animasyonunu durdur
               // walkPointSet = false; // Yürüme noktasýna ulaþýldýðýnda, yeni bir nokta belirle
            }
            else
            {
               animator.SetBool("isWalking", true); // Yürüme animasyonunu baþlat
            }

        }


    }
    private void ResetWalkPointSet()
    {
        walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range // Rastgele bir yürüme noktasý belirle
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        // Belirlenen noktanýn zemine temas ettiðini kontrol et
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
        walkPointSet = true;
        }
            
    }
    private void ChasePlayer()
    {
        animator.SetBool("isWalking",false);
		
		// Oyuncuyu takip etmek için agent'i güncelle
		if (agent.remainingDistance < agent.stoppingDistance)
        // düþmanýn hedefine olan uzaklýðý < düþmanýn durmasý gereken mesafe
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
        //Make sure enemy doesn't move // Düþmanýn hareket etmemesini saðla
       // düþmanýn saldýrý animasyonunu oynatýrken yerinde durmasýný saðla
      agent.SetDestination(transform.position);
      // Oyuncuya doðru dön
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
                        //float randomSpread = Random.Range(-5f, 5f); // Rastgele bir yayýlma açýsý belirle
                        //Vector3 shotDirection = Quaternion.Euler(0, randomSpread, 0) * transform.forward; // Yayýlma açýsýný kullanarak yeni atýþ yönünü hesapla
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
        // Rastgele bir açý seç, bu açý mermi rotasyonunu etkileyecek
        float randomSpread = Random.Range(randomSpreadX, randomSpreadY);
        Vector3 shotDirection = (originalTarget - transform.position).normalized;
        Vector3 randomDeviation = Quaternion.Euler(0, randomSpread, 0) * shotDirection;
        return originalTarget + randomDeviation * 2f;
    }

    // Saldýrýyý sýfýrla
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
      // Gizmo'larý çizerek saldýrý ve görme menzillerini görselleþtir

      Gizmos.color = Color.red;
      Gizmos.DrawWireSphere(transform.position, attackRange);
      Gizmos.color = Color.yellow;
      Gizmos.DrawWireSphere(transform.position, sightRange);
  }
  void UpdateTargetPosition(Transform target , GameObject player)
  {
      // Hedefin pozisyonunu güncelle (player'ýn pozisyonunu kullanarak örneðin)
      if (player != null)
      {
         //target = player.transform.position;
      }
  }
    //void OnTriggerEnter(Collider other)
    //{
    //    // Karakterin çocuk nesnelerini kontrol et
    //    foreach (Collider col  in childColliders)
    //    {
    //        // Eðer çocuk bir collider içeriyorsa

    //        if (col != null && other.tag == "PlayerBullet")
    //        {
    //            // Etkileþime geçilen collider'ý belirle
    //            Debug.Log($"{col.name} ile etkileþime geçildi!");
    //            // Burada özel iþlemleri gerçekleþtir
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
        isEffectRunning = true; // Efekt baþladýðýnda kontrol deðiþkenini true yap
        //Debug.Log(gameObject.name);
        debuffEffect.Play(); // Partikül sistemi baþlatýlýr
        yield return new WaitForSeconds(duration);
        debuffEffect.Stop();
        isEffectRunning = false;
    }

}
