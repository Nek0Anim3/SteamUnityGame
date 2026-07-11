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

    private EnemyStates _currentState; 
    private IEnemyStates _enemyState;

    private EnemyContext context;
    
    
    public E_IdleState idleState { get; private set; }
    public E_RoamingState roamingState { get; private set; }
    public E_ChaseState chaseState { get; private set; }

    private float timerSeconds;
    private bool isTimerRunning;
    
    private void Awake()
    {
        context = GetComponent<EnemyContext>();
        
        //States
        idleState = new E_IdleState(this, context);
        roamingState = new E_RoamingState(this, context);
        chaseState = new E_ChaseState(this, context);
        
        _enemyState = idleState;
        
    }

    public override void OnNetworkSpawn()
    {
        if (!IsServer) { enabled = false; }
        _enemyState.Enter();
        context.EnemyRaycaster.OnPlayerRaycastVisible += ForceChaseState;
    }
    
    //DEBUG NPC ROAMING SPHERE DRAWS
    /*private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0f, 0f, 1f, 0.5f);
        Gizmos.DrawSphere(roamingState.lastPos, 6.0f);

        foreach (var wpt in roamingState.visitedPoints_DEBUG)
        {
            Gizmos.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);     
            Gizmos.DrawSphere(wpt, 0.5f);
        }
    }*/


    public void ChangeState(IEnemyStates newState)
    {
        
        Debug.Log($"[NPC] Change state to {newState}");
        _enemyState.Exit();
        _enemyState = newState;
        _enemyState.Enter();
    }

    private void Update()
    {
        _enemyState.Update();
    }

    public void ForceChaseState()
    {
        ChangeState(chaseState);
    }



    

}


