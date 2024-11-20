using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DebugNavMeshAgent : MonoBehaviour
{
    NavMeshAgent agent;
    public bool velocity;
    public bool desiredVelocity;
    public bool path;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void OnDrawGizmos()
    {
        if (agent != null)
        {
            if (velocity)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(transform.position, transform.position + agent.velocity);
            }
            if (desiredVelocity)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, transform.position + agent.desiredVelocity);
            }
            if (path)
            {
                Gizmos.color = Color.blue;
                var agentPath = agent.path;
                Vector3 prevCorner = transform.position;
                foreach (var corner in agentPath.corners)
                {
                    Gizmos.DrawLine(prevCorner, corner);
                    Gizmos.DrawSphere(corner, .1f);
                    prevCorner = corner;
                }
            }
        }
        }
    }
