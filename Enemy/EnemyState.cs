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

    private EnemyStates _currentState; 
    private IEnemyStates _enemyState;

    private EnemyContext _context;
    
    
    public E_IdleState idleState { get; private set; }
    public E_RoamingState roamingState { get; private set; }
    public E_ChaseState chaseState { get; private set; }

    private float timerSeconds;
    private bool isTimerRunning;
    
    private void Awake()
    {
        _context = GetComponent<EnemyContext>();
        
        //States
        idleState = new E_IdleState(this, _context);
        roamingState = new E_RoamingState(this, _context);
        chaseState = new E_ChaseState(this, _context);
        
        _enemyState = idleState;
        
    }

    public override void OnNetworkSpawn()
    {
        if (!IsServer) { enabled = false; }
        _enemyState.Enter();
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

    private void FixedUpdate()
    {
        if (_context.EnemyRaycaster.playerInSight && _enemyState != chaseState)
        {
            ForceChaseState();
        }
        _enemyState.Update();
    }

    
    //CHASE STATE WRAP
    public void ForceChaseState()
    {
        ChangeState(chaseState);
    }

    public void ChaseStartUpdate()
    {
        InvokeRepeating(nameof(UpdateChasePos), 0, 0.2f);
    }

    public void StopChaseUpdate()
    {
        CancelInvoke(nameof(UpdateChasePos));
    }


    private void UpdateChasePos()
    {
        if (_context.EnemyRaycaster.playerInSight)
        {
            /*Debug.Log($"[NPC] Chase at {_context.transform.position}");*/
            
            _context.EnemyMovement.SetNewWaypoint(_context.EnemyRaycaster.NearestPlayer.transform.position);
        }
        else
        {
            if (_context.EnemyMovement.isMoving) return;
            if (chaseState.SearchTime > 0.0f)
            {
                Debug.Log("Search time left: " + chaseState.SearchTime);
                chaseState.SearchTime -= 0.2f;
            }
            else
            {
                ChangeState(roamingState);
                chaseState.SearchTime = chaseState.SEARCH_BASE_TIME; 
            }
        }
    }
    

}


