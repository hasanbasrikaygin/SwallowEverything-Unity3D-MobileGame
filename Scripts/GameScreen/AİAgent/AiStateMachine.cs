using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// AiStateMachine s�n�f�, AI'n�n durum makinesini olu�turur. Bu s�n�f, farkl� AI durumlar�n� y�netir,
// mevcut durumu g�nceller ve durumlar aras�nda ge�i� yapar.
public class AiStateMachine
{
    // T�m durumlar
    public AiState[] states;
    public AiAgent agent;
    public AiStateId currentState;
    public AiStateMachine(AiAgent agent)
    {
        this.agent = agent;
        // Durum say�s�n� al ve durumlar dizisini olu�tur
        int numStates = System.Enum.GetNames(typeof(AiStateId)).Length;
        states = new AiState[numStates];
    }
    // Durumu kaydet
    public void RegisterState(AiState state)
    {
        int index = (int)state.GetId();
        states[index] = state;
    }
    // Belirli bir durumu al
    public AiState GetState(AiStateId stateId)
    {
        int index = (int)stateId;
        return states[index];
    }
    // Durumu g�ncelle
    public void Update()
    {
        GetState(currentState)?.Update(agent);
    }
    // Durumu de�i�tir
    public void ChangeState(AiStateId newState)
    {
        // Mevcut durumdan ��k
        GetState(currentState)?.Exit(agent);
        // Yeni duruma ge�
        currentState = newState;
        // Yeni duruma giri� yap
        GetState(currentState).Enter(agent);
    }
}
