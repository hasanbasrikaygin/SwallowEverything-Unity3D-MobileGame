using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class AiFindWeaponState : AiState
{
    private float checkInterval = 2f; // Mesafe kontrol� i�in zaman aral��� (yar�m saniye)
    private float nextCheckTime = 0f; // Bir sonraki kontrol zaman�
    
    public AiStateId GetId()
    {
        return AiStateId.FindWeapon;

    }

    public void Enter(AiAgent agent)
    {

        nextCheckTime = Time.time; // Zamanlay�c�y� s�f�rla
    }

    public void Exit(AiAgent agent)
    {

    }

    public void Update(AiAgent agent)
    {
        if (Time.time >= nextCheckTime)
        {
            nextCheckTime = Time.time + checkInterval; // Bir sonraki kontrol zaman�n� ayarla

            float distance = Vector3.Distance(agent.transform.position, agent.playerTransform.position);

            // Mesafeyi �iz
            //Debug.DrawLine(agent.transform.position, agent.playerTransform.position, Color.red);
            // Belirli mesafeden az ise ba�ka bir duruma ge�
            if (distance < agent.detectionRange)
            {
                // Ba�ka bir duruma ge�i� yap
                agent.stateMachine.ChangeState(AiStateId.Idle);
            }
        }
    }
}
