using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
// Oyuncuyu takip etme durumu
public class AiChasePlayerState : AiState
{
    float timer = 0f;
    // Durumun kimli�ini d�nd�r�r (bu durum ChasePlayer oldu�u i�in AiStateId.ChasePlayer d�nd�r�l�r)
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
        // E�er AI devre d��� b�rak�lm��sa i�lem yapma
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
            // AttackState'e ge�i� yap
            agent.stateMachine.ChangeState(AiStateId.AttackPlayer);
            return;
        }
        timer -= Time.deltaTime;
        // E�er AI'n�n bir hedefi yoksa, hedefini oyuncunun pozisyonuna ayarla
        agent.navMeshAgent.destination = agent.playerTransform.position;
        if (!agent.navMeshAgent.hasPath)
        {
            
        }
        if (timer < 0f)
        {
            // Hedefe olan y�nelimi al
            Vector3 direction = (agent.playerTransform.position - agent.navMeshAgent.destination);
            direction.y = 0;
            // E�er oyuncuya olan mesafe, AI'n�n maksimum mesafesinden fazlaysa
            if (direction.sqrMagnitude > agent.config.maxDistance * agent.config.maxDistance)
            {
                // E�er navmesh yol durumu k�smi de�ilse (yani tam bir yol bulunmu�sa)
                if (agent.navMeshAgent.pathStatus != NavMeshPathStatus.PathPartial)
                {
                    // Hedefi oyuncunun pozisyonuna ayarla
                    agent.navMeshAgent.destination = agent.playerTransform.position;
                }
            }
            // Zamanlay�c�y� maksimum s�re ile ayarla
            timer = agent.config.maxTime;
        }
    }
}
