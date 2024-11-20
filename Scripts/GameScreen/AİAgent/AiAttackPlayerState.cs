using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class AiAttackPlayerState : AiState
{

    float timer = 0f;
    private int isShoot;
    private float rotationSpeed = 7f;
    private Collider enemyCollider;
    private bool isPlayVoice = true;
    public AiStateId GetId()
    {
        return AiStateId.AttackPlayer;
    }

    public void Enter(AiAgent agent)
    {
        isShoot = Animator.StringToHash("isShoot");
        //Debug.Log("Entered the Attack Player state");
        agent.navMeshAgent.destination = agent.playerTransform.position;
        agent.GetComponent<WeaponIKEnemy>().isTargetActive = true;
    }

    public void Exit(AiAgent agent)
    {
        //Debug.Log("Exit the Attack Player state");
    }

    public void Update(AiAgent agent)
    {
        agent.navMeshAgent.destination = agent.playerTransform.position;

        timer -= Time.deltaTime;
        Vector3 playerPosition = agent.playerTransform.position;
        Vector3 enemyPosition = agent.transform.position;
        float distanceToPlayer = Vector3.Distance(playerPosition, enemyPosition);
        Vector3 directionToPlayer = playerPosition - enemyPosition;

        //directionToPlayer.y = 0; // Y ekseninde dönüþ olmasýný istemiyoruz
        if (directionToPlayer != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

        if (timer < 0f)
        {
            Vector3 direction = (agent.playerTransform.position - agent.navMeshAgent.destination);
            direction.y = 0;

            if (direction.sqrMagnitude > agent.config.maxDistance * agent.config.maxDistance)
            {
                agent.GetComponent<WeaponIKEnemy>().isTargetActive = true;
                if (agent.navMeshAgent.pathStatus != NavMeshPathStatus.PathPartial)
                {
                    agent.navMeshAgent.destination = agent.playerTransform.position;
                }
            }

            if (distanceToPlayer <= 17f) //  && distanceToPlayer >= 10f
            {
                ShootPlayer(agent);
                if(isPlayVoice && agent.enemyVoice != null)
                {
                    agent.audioSource.PlayOneShot(agent.enemyVoice);
                }
                isPlayVoice = false;
            }

            //if (distanceToPlayer < 10f)
            //{
            //    ShootPlayer(agent);
            //    //animator.SetBool(isShoot, true);
            //}

            timer = agent.config.maxTime;
        }
    }

    private void ShootPlayer(AiAgent agent)
    {
        if (!agent.alreadyAttacked && !agent.isDeadEffectWorking)
        {
            EnemyShootRaycast enemyShootRaycast = agent.GetComponent<EnemyShootRaycast>();
            if (enemyShootRaycast != null)
            {
                if (agent.bulletStartPoint != null && agent.enemyTarget != null)
                {
                    Vector3 origin = agent.bulletStartPoint.transform.position;
                    Vector3 target = agent.enemyTarget.position;
                    agent.alreadyAttacked = true;
                    enemyShootRaycast.Shoot(origin, target, agent);
                    agent.Invoke(nameof(agent.ResetAttack), agent.config.maxTime / 3);
                }
                else
                {
                    if (agent.bulletStartPoint == null)
                    {
                        Debug.LogError("Bullet start point is not set.");
                    }
                    if (agent.enemyTarget == null)
                    {
                        Debug.LogError("Enemy target is not set.");
                    }
                }
            }
            else
            {
                Debug.LogError("EnemyShootRaycast bileþeni bulunamadý!");
            }
        }
    
    /*
    animator.SetBool(isShoot, true);

    if (!agent.alreadyAttacked && !agent.isDeadEffectWorking)
    {
        EnemyShootRaycast enemyShootRaycast = agent.GetComponent<EnemyShootRaycast>();
        if (agent != null)
        {

            Vector3 origin = agent.bulletStartPoint.transform.position;
            Vector3 target = agent.enemyTarget.position;

            // Mermiyi hedefe doðru ateþle
            enemyShootRaycast.Shoot(origin, target, agent);

            agent.alreadyAttacked = true;
            agent.AttackController();
        }
        else
        {
            Debug.LogError("Failed to get bullet object or bulletStartPoint is not set.");
        }


        //GameObject g = agent.enemyBulletPool.GetObject();
        //EnemyBulletController bulletController = g.GetComponent<EnemyBulletController>();

        //// bulletController.target = ApplyRandomDeviation(agent.enemyTarget.position , agent);



        ////Debug.Log("Þaun buradasýnýz");
        //if (g != null)
        //{
        //    g.transform.position = agent.bulletStartPoint.transform.position;
        //    g.transform.rotation = Quaternion.identity;
        //    Rigidbody rb = g.GetComponent<Rigidbody>();
        //    g.SetActive(true);
        //    if (rb != null)
        //    {
        //       // audioManager.EnemyGunAudioSource(audioManager.enemyGunClip);
        //    }

        //    else
        //    {
        //        Debug.LogError("Rigidbody component not found on the bullet GameObject.");
        //    }
        //    agent.alreadyAttacked = true;
        //    agent.AttackController();
        //}
        //else
        //{
        //    Debug.LogError("Failed to get bullet object or bulletStartPoint is not set.");
        //}



    } */
}

}
