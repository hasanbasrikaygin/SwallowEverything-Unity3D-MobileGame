using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiDeathState : AiState
{
    public Vector3 direction;
    public AiStateId GetId()
    {
        return AiStateId.Death;
    }

    public void Enter(AiAgent agent)
    {
        
        //Debug.Log("Entered the Death state");
        agent.ragdoll.ActivateRagdoll();
        direction.y = 1;
        agent.ragdoll.ApplyForce(direction * agent.config.dieForce);
        //agent.uiHealthBar.gameObject.SetActive(false);
        agent.skinnedMeshRenderer.updateWhenOffscreen = true;
        agent.GetComponent<WeaponIKEnemy>().enabled = false;
        

    }

    public void Exit(AiAgent agent)
    {
        //Debug.Log("Exit the Death state");
    }

    public void Update(AiAgent agent)
    {
       
    }
}
