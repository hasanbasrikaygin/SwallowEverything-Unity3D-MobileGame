using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using static UnityEngine.RuleTile.TilingRuleOutput;

// AI belirli bir hedefi takip etmiyor ve hareket etmek i�in rasgele bir konuma gidiyor.
public class AiIdleState : AiState
{
    public float patrolRange = 10f; // D��man�n devriye yapabilece�i maksimum mesafe
    public float obstacleDetectionDistance = 2f; // Engel alg�lama mesafesi
    private float nextPatrolTime; // Sonraki devriye zaman�
    private float patrolInterval = 5f; // Devriye aral���
    private float originalStoppingDistance; // Orijinal stopping distance de�eri
    private float nextCheckTime = 0f;
    // Durumun kimli�ini d�nd�r�r (bu durum Idle oldu�u i�in AiStateId.Idle d�nd�r�l�r)
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
        // Devriye sona erdi�inde stopping distance de�erini geri y�kle
        agent.navMeshAgent.stoppingDistance = originalStoppingDistance;
      
    }

    public void Update(AiAgent agent)
    {

        AiSensor sensor = agent.sensor;

        // E�er sensor null de�ilse ve sensor'�n i�indeki alg�lanan nesneler listesi doluysa
        if (sensor != null && sensor.Objects.Count > 0)
        {
            // Alg�lanan nesneler aras�nda d�ng� yap
            foreach (GameObject detectedObject in sensor.Objects)
            {
                //Debug.Log(detectedObject.name + " - " + detectedObject.tag);
                // E�er alg�lanan nesne "Player" etiketine sahipse
                if (detectedObject.CompareTag("SensorObj"))
                {
                    // AI durum makinesinde ChasePlayer durumuna ge�i� yap
                    agent.stateMachine.ChangeState(AiStateId.ChasePlayer);
                    //Debug.Log("Ai Sensor Obj tespit edildi");
                    return; // Durumu de�i�tirdik, bu y�zden fonksiyondan ��k
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
            nextCheckTime = Time.time + 2; // Bir sonraki kontrol zaman�n� ayarla

            float distance = Vector3.Distance(agent.transform.position, agent.playerTransform.position);

            // Mesafeyi �iz
            //Debug.DrawLine(agent.transform.position, agent.playerTransform.position, Color.red);
            // Belirli mesafeden az ise ba�ka bir duruma ge�
            if (distance > agent.detectionRange)
            {
                // Ba�ka bir duruma ge�i� yap
                agent.stateMachine.ChangeState(AiStateId.FindWeapon);
            }
        }
        if (Time.time < nextPatrolTime)
        {
            return;
        }

        // E�er ajan�n hedefi yoksa veya devriye aral��� dolmu�sa
        if (!agent.navMeshAgent.hasPath)
        {
            Vector3 randomPosition = GetRandomPositionAroundAgent(agent.transform.position, patrolRange);

            // Engelleri alg�la ve engelden ka�
            Vector3 newDestination = AvoidObstacles(agent, randomPosition);
            agent.navMeshAgent.destination = newDestination;

            // Hedefe do�ru bak
            agent.transform.LookAt(newDestination);

            // Sonraki devriye zaman�n� ayarla
            patrolInterval = Random.Range(2, 9);
            nextPatrolTime = Time.time + patrolInterval;
        }

        // Oyuncu d��man�n g�rme mesafesine girdiyse ve pozitif y�nde ise ChasePlayer durumuna ge�
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

        // Engel alg�lama i�in bir raycast kullan
        if (Physics.Raycast(agent.transform.position, directionToTarget.normalized, out hit, obstacleDetectionDistance))
        {
            // Engeli alg�lad�k, yeni bir hedef belirleyece�iz
            Vector3 newDestination = GetRandomPositionAroundAgent(agent.transform.position, patrolRange);
            return newDestination;
        }

        // Engel yoksa, hedef noktay� kullan
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

