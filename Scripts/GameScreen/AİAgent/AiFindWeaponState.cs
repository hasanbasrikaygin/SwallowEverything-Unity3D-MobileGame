using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class AiFindWeaponState : AiState
{
    private float checkInterval = 2f; // Mesafe kontrolü için zaman aralýðý (yarým saniye)
    private float nextCheckTime = 0f; // Bir sonraki kontrol zamaný
    
    public AiStateId GetId()
    {
        return AiStateId.FindWeapon;

    }

    public void Enter(AiAgent agent)
    {

        nextCheckTime = Time.time; // Zamanlayýcýyý sýfýrla
    }

    public void Exit(AiAgent agent)
    {

    }

    public void Update(AiAgent agent)
    {
        if (Time.time >= nextCheckTime)
        {
            nextCheckTime = Time.time + checkInterval; // Bir sonraki kontrol zamanýný ayarla

            float distance = Vector3.Distance(agent.transform.position, agent.playerTransform.position);

            // Mesafeyi çiz
            //Debug.DrawLine(agent.transform.position, agent.playerTransform.position, Color.red);
            // Belirli mesafeden az ise baþka bir duruma geç
            if (distance < agent.detectionRange)
            {
                // Baþka bir duruma geçiþ yap
                agent.stateMachine.ChangeState(AiStateId.Idle);
            }
        }
    }
}
