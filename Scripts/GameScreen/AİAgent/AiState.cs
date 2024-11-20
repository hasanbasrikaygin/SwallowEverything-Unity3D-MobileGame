using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// AiStateId enum'u, AI'n�n alabilece�i farkl� durumlar� temsil eder. Her durum bir say�sal de�erle ili�kilendirilir.

public enum AiStateId
{   
    Idle,
    ChasePlayer,
    Death,
    FindWeapon,
    AttackPlayer
}
// AiState aray�z�, bir AI durumunun temel yap�s�n� tan�mlar. Bu aray�z� uygulayan s�n�flar, belirli bir durumun davran��lar�n� tan�mlar:
public interface AiState
{
    AiStateId GetId();
    void Enter(AiAgent agent);
    void Update(AiAgent agent);
    void Exit(AiAgent agent);
}
