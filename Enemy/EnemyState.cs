using Enemy;
using Enemy.States;
using FishNet.Object;
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
    
    //States
    public E_IdleState idleState { get; private set; }
    public E_RoamingState roamingState { get; private set; }
    public E_ChaseState chaseState { get; private set; }
    public E_AttackState attackState { get; private set; }
    
    private float timerSeconds;
    private bool isTimerRunning;
    private float distanceToPlayer;
    public readonly float ATTACK_DISTANCE = 1.8f;
    private void Awake()
    {
        _context = GetComponent<EnemyContext>();
        
        //STATES
        //==========================
        idleState = new E_IdleState(this, _context);
        roamingState = new E_RoamingState(this, _context);
        chaseState = new E_ChaseState(this, _context);
        attackState = new E_AttackState(this, _context);
        //==========================
        _enemyState = idleState;
        
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        if (!IsServer) { enabled = false; }
        _enemyState.Enter();
    }

    public void ChangeState(IEnemyStates newState)
    {
        
        Debug.Log($"[NPC] Change state to {newState}");
        _enemyState.Exit();
        _enemyState = newState;
        //=========
        // UI DEBUG
        /*UIDebug.Instance.ENEMY_STATE = newState.ToString()[15..];*/
        //=========
        _enemyState.Enter();
    }

    private void FixedUpdate()
    {
        if (_context.EnemyRaycaster.playerInSight && _enemyState != chaseState && _enemyState != attackState)
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
            if (_context.EnemyRaycaster.NearestPlayer != null)
            {
                distanceToPlayer = Vector3.Distance(_context.transform.position, _context.EnemyRaycaster.NearestPlayer.transform.position);
                //===========
                // UI DEBUG
                /*UIDebug.Instance.ENEMY_DIST_TO_PLAYER = distanceToPlayer;*/
                //===========
            }
            _context.EnemyMovement.SetNewWaypoint(_context.EnemyRaycaster.NearestPlayer.transform.position);
            if (distanceToPlayer <= ATTACK_DISTANCE && _enemyState != attackState)
            {
                ChangeState(attackState);
            }
        }
        else
        {
            if (_context.EnemyMovement.isMoving) return;
            if (chaseState.SearchTime > 0.0f)
            {
                Debug.Log("Search time left: " + chaseState.SearchTime);
                //======
                // UI DEBUG
                /*UIDebug.Instance.ENEMY_IDLE_TIME = chaseState.SearchTime;*/
                //======
                chaseState.SearchTime -= 0.2f;
            }
            else
            {
                ChangeState(roamingState);
                /*UIDebug.Instance.ENEMY_IDLE_TIME = 0.0f;*/
                chaseState.SearchTime = chaseState.SEARCH_BASE_TIME; 
            }
        }
    }
    

}


