using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using static UnityEngine.RuleTile.TilingRuleOutput;

// AI belirli bir hedefi takip etmiyor ve hareket etmek için rasgele bir konuma gidiyor.
public class AiIdleState : AiState
{
    public float patrolRange = 10f; // Düþmanýn devriye yapabileceði maksimum mesafe
    public float obstacleDetectionDistance = 2f; // Engel algýlama mesafesi
    private float nextPatrolTime; // Sonraki devriye zamaný
    private float patrolInterval = 5f; // Devriye aralýðý
    private float originalStoppingDistance; // Orijinal stopping distance deðeri
    private float nextCheckTime = 0f;
    // Durumun kimliðini döndürür (bu durum Idle olduðu için AiStateId.Idle döndürülür)
    public AiStateId GetId()
    {
        return AiStateId.Idle;
    }
    public void Enter(AiAgent agent)
    {
        originalStoppingDistance = agent.navMeshAgent.stoppingDistance;
        agent.navMeshAgent.stoppingDistance = 0f;
        nextCheckTime = Time.time;
        
    }

    public void Exit(AiAgent agent)
    {
        // Devriye sona erdiðinde stopping distance deðerini geri yükle
        agent.navMeshAgent.stoppingDistance = originalStoppingDistance;
      
    }

    public void Update(AiAgent agent)
    {

        AiSensor sensor = agent.sensor;

        // Eðer sensor null deðilse ve sensor'ün içindeki algýlanan nesneler listesi doluysa
        if (sensor != null && sensor.Objects.Count > 0)
        {
            // Algýlanan nesneler arasýnda döngü yap
            foreach (GameObject detectedObject in sensor.Objects)
            {
                //Debug.Log(detectedObject.name + " - " + detectedObject.tag);
                // Eðer algýlanan nesne "Player" etiketine sahipse
                if (detectedObject.CompareTag("SensorObj"))
                {
                    // AI durum makinesinde ChasePlayer durumuna geçiþ yap
                    agent.stateMachine.ChangeState(AiStateId.ChasePlayer);
                    //Debug.Log("Ai Sensor Obj tespit edildi");
                    return; // Durumu deðiþtirdik, bu yüzden fonksiyondan çýk
                }
            }
        }
        else
        {
           // Debug.Log(" Sensor is null");
        }
        Patrolling(agent);



    }
    
    public void Patrolling(AiAgent agent)
    {
        if (Time.time >= nextCheckTime)
        {
            nextCheckTime = Time.time + 2; // Bir sonraki kontrol zamanýný ayarla

            float distance = Vector3.Distance(agent.transform.position, agent.playerTransform.position);

            // Mesafeyi çiz
            //Debug.DrawLine(agent.transform.position, agent.playerTransform.position, Color.red);
            // Belirli mesafeden az ise baþka bir duruma geç
            if (distance > agent.detectionRange)
            {
                // Baþka bir duruma geçiþ yap
                agent.stateMachine.ChangeState(AiStateId.FindWeapon);
            }
        }
        if (Time.time < nextPatrolTime)
        {
            return;
        }

        // Eðer ajanýn hedefi yoksa veya devriye aralýðý dolmuþsa
        if (!agent.navMeshAgent.hasPath)
        {
            Vector3 randomPosition = GetRandomPositionAroundAgent(agent.transform.position, patrolRange);

            // Engelleri algýla ve engelden kaç
            Vector3 newDestination = AvoidObstacles(agent, randomPosition);
            agent.navMeshAgent.destination = newDestination;

            // Hedefe doðru bak
            agent.transform.LookAt(newDestination);

            // Sonraki devriye zamanýný ayarla
            patrolInterval = Random.Range(2, 9);
            nextPatrolTime = Time.time + patrolInterval;
        }

        // Oyuncu düþmanýn görme mesafesine girdiyse ve pozitif yönde ise ChasePlayer durumuna geç
        Vector3 playerDirection = agent.playerTransform.position - agent.transform.position;
        playerDirection.Normalize();
        Vector3 agentDirection = agent.transform.forward;
        float dotProduct = Vector3.Dot(playerDirection, agentDirection);
        if (playerDirection.magnitude <= agent.config.maxSightDistance && dotProduct > 0)
        {
            //agent.stateMachine.ChangeState(AiStateId.ChasePlayer);
        }

    }

    private Vector3 AvoidObstacles(AiAgent agent, Vector3 targetPosition)
    {
        RaycastHit hit;
        Vector3 directionToTarget = targetPosition - agent.transform.position;

        // Engel algýlama için bir raycast kullan
        if (Physics.Raycast(agent.transform.position, directionToTarget.normalized, out hit, obstacleDetectionDistance))
        {
            // Engeli algýladýk, yeni bir hedef belirleyeceðiz
            Vector3 newDestination = GetRandomPositionAroundAgent(agent.transform.position, patrolRange);
            return newDestination;
        }

        // Engel yoksa, hedef noktayý kullan
        return targetPosition;
    }

    private Vector3 GetRandomPositionAroundAgent(Vector3 center, float range)
    {
        Vector3 randomDirection = Random.insideUnitSphere * range;
        randomDirection += center;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, range, NavMesh.AllAreas);
        return navHit.position;
    }
   
}

