using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AiAgentConfig : ScriptableObject
{
    public float maxTime = 1f;
    public float maxDistance = 1f;
    public float dieForce = 10f;
    public float maxSightDistance = 7f;
    public float maxPatrolDistance = 300f;
    public float attackDistance = 7f;
    //public Vector3 walkPoint;
    //public bool walkPointSet = true;
    //public float timeSinceLastPatrol = 0f;
    public float maxAttackDistance=11f;
    public float walkPointRange;
    public LayerMask whatIsGround;
}
