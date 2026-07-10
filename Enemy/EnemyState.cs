using System;
using Enemy;
using Enemy.States;
using Unity.Netcode;
using UnityEngine;

public enum EnemyStates
{
    Idle,
    Roaming,
    Chasing,
    Attack
}

public class EnemyState : NetworkBehaviour
{
    public event Action<Vector3> OnChangeWaypoint;

    private EnemyStates _currentState; 
    private IEnemyStates _enemyState;

    private EnemyContext context;
    
    public E_IdleState idleState { get; private set; }
    public E_RoamingState roamingState { get; private set; }


    private float timerSeconds;
    private bool isTimerRunning;
    
    private void Awake()
    {
        context = GetComponent<EnemyContext>();
        
        idleState = new E_IdleState(this, context);
        roamingState = new E_RoamingState(this, context);
        
        _enemyState = idleState;
    }

    public override void OnNetworkSpawn()
    {
        if (!IsServer) { enabled = false; }
        _enemyState.Enter();
    }


    public void ChangeState(IEnemyStates newState)
    {
        
        UnityEngine.Debug.Log($"[NPC] Change state to {newState}");
        _enemyState.Exit();
        _enemyState = newState;
        _enemyState.Enter();
    }

    public void SetStateFlag(EnemyStates newState)
    {
        UnityEngine.Debug.Log($"[NPC] State flag set to {newState}");
        _currentState = newState;
    }

    private void Update()
    {
        _enemyState.Update();
    }




    

}


