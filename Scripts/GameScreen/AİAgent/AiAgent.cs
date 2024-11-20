using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
// AiAgent sýnýfý, AI karakterinin davranýþýný ve özelliklerini tanýmlar.
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

        // AI'nýn durum makinesini oluþtur ve bu sýnýfýn örneði ile iliþkilendir
        stateMachine = new AiStateMachine(this);
        
        // Farklý AI durumlarýný kaydet
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
        isEffectRunning = true; // Efekt baþladýðýnda kontrol deðiþkenini true yap
        //Debug.Log(gameObject.name);
        agent.debuffEffect.Play(); // Partikül sistemi baþlatýlýr
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
        isEffectRunning = true; // Efekt baþladýðýnda kontrol deðiþkenini true yap
        //Debug.Log(gameObject.name);
        debuffEffect.Play(); // Partikül sistemi baþlatýlýr
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
    //    // Detection range için bir küre çiz
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(gameObject.transform.position, detectionRange);
    //    // Agent ile player arasýndaki mesafe çizgisi
    //    //Gizmos.color = Color.red;
    //    //Gizmos.DrawLine(agent.transform.position, gameObject.playerTransform.position);
    //}
}
