using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// AiStateId enum'u, AI'nýn alabileceði farklý durumlarý temsil eder. Her durum bir sayýsal deðerle iliþkilendirilir.

public enum AiStateId
{   
    Idle,
    ChasePlayer,
    Death,
    FindWeapon,
    AttackPlayer
}
// AiState arayüzü, bir AI durumunun temel yapýsýný tanýmlar. Bu arayüzü uygulayan sýnýflar, belirli bir durumun davranýþlarýný tanýmlar:
public interface AiState
{
    AiStateId GetId();
    void Enter(AiAgent agent);
    void Update(AiAgent agent);
    void Exit(AiAgent agent);
}
