using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Health : MonoBehaviour
{
    public GameObject healthBar;
    public float maxHealth = 100;
    [HideInInspector]
    public float currentHealth;
    public float CurrentHealth
    {
        get { return currentHealth; }
    }
    AiAgent agent;
    //UIHealthBar healthBar;

    SkinnedMeshRenderer skinnedMeshRenderer;
    public float blinkIntensity;
    public float blinkDuration;
    float blinkTimer;

    private bool isDead = false; // Düþmanýn ölüp ölmediðini takip eden deðiþken

    void Start()
    {
        //healthBar = GetComponentInChildren<UIHealthBar>();
        agent = GetComponent<AiAgent>();
        currentHealth = maxHealth;
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        var rigidBodies = GetComponentsInChildren<Rigidbody>();
        foreach (var rb in rigidBodies)
        {
            HitBox hitBox = rb.gameObject.AddComponent<HitBox>();
            hitBox.health = this;
        }
    }

    public void TakeDamage(float damage, Vector3 direction)
    {
        if (isDead) return; // Düþman öldüyse daha fazla zarar almaz

        currentHealth -= damage;

        //healthBar.SetHealthBarPercentage(currentHealth / maxHealth);
        if (currentHealth <= 0 != isDead)
        {
            Die(direction);
        }
        blinkTimer = blinkDuration;
    }

    public void Die(Vector3 direction)
    {
        if (isDead) return; // Düþman öldüyse tekrar ölemez

        isDead = true; // Düþmaný öldü olarak iþaretle

        healthBar.SetActive(false);
        AiDeathState deathState = agent.stateMachine.GetState(AiStateId.Death) as AiDeathState;
        deathState.direction = direction;
        ScoreManager.instance.DecreaseEnemyCount();

        StartCoroutine(DeadAgents(3f));
        agent.stateMachine.ChangeState(AiStateId.Death);
        
    }

    // Update is called once per frame
    void Update()
    {
        blinkTimer -= Time.deltaTime;
        float lerp = Mathf.Clamp01(blinkTimer / blinkDuration);
        float intensity = (lerp * blinkIntensity) + 1f;
        skinnedMeshRenderer.material.color = Color.white * intensity;
        //Debug.Log("Can" + currentHealth);
    }
    
        public IEnumerator DeadAgents(float duration)
    {

        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }
}
