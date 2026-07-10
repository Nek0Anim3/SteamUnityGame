using TMPro.EditorUtilities;
using UnityEngine;

namespace Enemy.States
{
    public class E_RoamingState : IEnemyStates
    {
        private EnemyContext _context;
        private EnemyState stateMachine;
        private bool isActive = false;
        private bool isMoving = false;
        private Vector3 lastPos;
        //конструктор
        public E_RoamingState(EnemyState machine, EnemyContext ctx)
        {
            UnityEngine.Debug.Log("[INIT] E_RoamingState created!");
            stateMachine = machine;
            _context = ctx;
            lastPos = machine.transform.position;
        }
        
        
        public void Enter()
        {
            isActive = true;

            _context.EnemyWaypointNav.SetNewDestination(lastPos);
            lastPos = _context.EnemyWaypointNav.GetCurrentWaypoint();
            isMoving = true;
        }

        public void Exit()
        {
            isActive = false;   
        }

        
        
        public void Update()
        {
            if (!isActive) return;
            if (isMoving)
            {
                if (Vector3.Distance(_context.EnemyMovement.transform.position, lastPos) < 1.0f)
                {
                    stateMachine.ChangeState(stateMachine.idleState);
                }
            }
        }
    }
}