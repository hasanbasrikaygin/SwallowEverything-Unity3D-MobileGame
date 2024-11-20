using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
// Oyuncuyu takip etme durumu
public class AiChasePlayerState : AiState
{
    float timer = 0f;
    // Durumun kimliðini döndürür (bu durum ChasePlayer olduðu için AiStateId.ChasePlayer döndürülür)
    public AiStateId GetId()
    {
        return AiStateId.ChasePlayer;
    }
    public void Enter(AiAgent agent)
    {
        agent.navMeshAgent.stoppingDistance = 10f;
        agent.GetComponent<WeaponIKEnemy>().isTargetActive = false;
        //Debug.Log("Entered the chase state");
    }

    public void Exit(AiAgent agent)
    {
        //Debug.Log("Exit the chase state");
    }
    public void Update(AiAgent agent)
    {
        // Eðer AI devre dýþý býrakýlmýþsa iþlem yapma
        if (!agent.enabled)
        {
            Debug.Log("!agent.enabled");
            return;
        }
        //Debug.Log(agent.GetComponent<WeaponIKEnemy>().isTargetActive);
        Vector3 playerPosition = agent.playerTransform.position;
        Vector3 enemyPosition = agent.transform.position;
        float distanceToPlayer = Vector3.Distance(playerPosition, enemyPosition);

        if (distanceToPlayer <= agent.config.attackDistance)
        {
            // AttackState'e geçiþ yap
            agent.stateMachine.ChangeState(AiStateId.AttackPlayer);
            return;
        }
        timer -= Time.deltaTime;
        // Eðer AI'nýn bir hedefi yoksa, hedefini oyuncunun pozisyonuna ayarla
        agent.navMeshAgent.destination = agent.playerTransform.position;
        if (!agent.navMeshAgent.hasPath)
        {
            
        }
        if (timer < 0f)
        {
            // Hedefe olan yönelimi al
            Vector3 direction = (agent.playerTransform.position - agent.navMeshAgent.destination);
            direction.y = 0;
            // Eðer oyuncuya olan mesafe, AI'nýn maksimum mesafesinden fazlaysa
            if (direction.sqrMagnitude > agent.config.maxDistance * agent.config.maxDistance)
            {
                // Eðer navmesh yol durumu kýsmi deðilse (yani tam bir yol bulunmuþsa)
                if (agent.navMeshAgent.pathStatus != NavMeshPathStatus.PathPartial)
                {
                    // Hedefi oyuncunun pozisyonuna ayarla
                    agent.navMeshAgent.destination = agent.playerTransform.position;
                }
            }
            // Zamanlayýcýyý maksimum süre ile ayarla
            timer = agent.config.maxTime;
        }
    }
}
