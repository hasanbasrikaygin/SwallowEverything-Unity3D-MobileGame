using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// AiStateMachine sýnýfý, AI'nýn durum makinesini oluþturur. Bu sýnýf, farklý AI durumlarýný yönetir,
// mevcut durumu günceller ve durumlar arasýnda geçiþ yapar.
public class AiStateMachine
{
    // Tüm durumlar
    public AiState[] states;
    public AiAgent agent;
    public AiStateId currentState;
    public AiStateMachine(AiAgent agent)
    {
        this.agent = agent;
        // Durum sayýsýný al ve durumlar dizisini oluþtur
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
    // Durumu güncelle
    public void Update()
    {
        GetState(currentState)?.Update(agent);
    }
    // Durumu deðiþtir
    public void ChangeState(AiStateId newState)
    {
        // Mevcut durumdan çýk
        GetState(currentState)?.Exit(agent);
        // Yeni duruma geç
        currentState = newState;
        // Yeni duruma giriþ yap
        GetState(currentState).Enter(agent);
    }
}
