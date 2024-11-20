using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
// AiAgent s�n�f�, AI karakterinin davran���n� ve �zelliklerini tan�mlar.
public class AiAgent : MonoBehaviour
{
    public AiStateMachine stateMachine;
    public AiStateId initialState;
    public NavMeshAgent navMeshAgent;
    public AiAgentConfig config;
    public Ragdoll ragdoll;
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public UIHealthBar uiHealthBar;
    public Transform playerTransform;
    public AiSensor sensor;
    public bool walkPointSet = true;
    public Vector3 walkPoint;
    public GameObject weapon;
    public float randomSpreadX;
    public float randomSpreadY;
    public EnemyBulletPool enemyBulletPool;
    public GameObject bulletStartPoint;
    public Canvas canvas;
    public float timeBetweenAttacks;
    public ParticleSystem debuffEffect;
    public Transform enemyTarget;
    public bool alreadyAttacked;
    public float health = 100;
    public bool isEffectRunning = false;
    public bool isDeadEffectWorking = false;
    public AudioSource audioSource;
    public AudioClip enemyVoice;
    public float detectionRange = 70f; // Tespit menzili (belirli mesafe) 
    public CinemachineVirtualCamera virtualCamera;
    void Start()
    {
        if (playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        }
        sensor.GetComponent<AiSensor>();
        ragdoll = GetComponent<Ragdoll>();
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        uiHealthBar = GetComponentInChildren<UIHealthBar>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        // AI'n�n durum makinesini olu�tur ve bu s�n�f�n �rne�i ile ili�kilendir
        stateMachine = new AiStateMachine(this);
        
        // Farkl� AI durumlar�n� kaydet
        stateMachine.RegisterState(new AiChasePlayerState());
        stateMachine.RegisterState(new AiDeathState());
        stateMachine.RegisterState(new AiIdleState());
        stateMachine.RegisterState(new AiFindWeaponState());
        stateMachine.RegisterState(new AiAttackPlayerState());
        stateMachine.ChangeState(initialState);
        
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
    }
    public IEnumerator EnableEffectForDuration(float duration, AiAgent agent)
    {
        isEffectRunning = true; // Efekt ba�lad���nda kontrol de�i�kenini true yap
        //Debug.Log(gameObject.name);
        agent.debuffEffect.Play(); // Partik�l sistemi ba�lat�l�r
        yield return new WaitForSeconds(duration);
        agent.debuffEffect.Stop();
        isEffectRunning = false;
    }

    //public void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag == "PlayerBullet")
    //    {
    //        TakeDamage(((int)SelectedWeapon.damage));
    //        StartCoroutine(EnableEffectForDuration(2f));
    //    }
    //}
    public void ResetAttackController()
    {
       
        //Invoke(nameof(ResetAttack), config.maxTime / 3);
    }
    public void ResetAttack()
    {
        alreadyAttacked = false;
       
    }


    public IEnumerator EnableEffectForDuration(float duration)
    {
        isEffectRunning = true; // Efekt ba�lad���nda kontrol de�i�kenini true yap
        //Debug.Log(gameObject.name);
        debuffEffect.Play(); // Partik�l sistemi ba�lat�l�r
        yield return new WaitForSeconds(duration);
        debuffEffect.Stop();
        isEffectRunning = false;
    }
    public void AttackController()
    {
       
    }public void DisableAgent()
    {
       gameObject.SetActive(false);
    }
    //public void OnDrawGizmos()
    //{
    //    // Detection range i�in bir k�re �iz
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(gameObject.transform.position, detectionRange);
    //    // Agent ile player aras�ndaki mesafe �izgisi
    //    //Gizmos.color = Color.red;
    //    //Gizmos.DrawLine(agent.transform.position, gameObject.playerTransform.position);
    //}
}
