using UnityEngine;

namespace Enemy.States
{
    public class E_IdleState : IEnemyStates
    {
        private EnemyState stateMachine;
        private EnemyContext _context;
        private bool isActive = false;

        private float idleTimer = 5.0f;
        private float timer;
        public E_IdleState(EnemyState machine, EnemyContext ctx)
        {
            Debug.Log("[INIT] E_IdleState created!");
            stateMachine = machine;
            _context = ctx;
        }
        
        public void Enter()
        {
            isActive = true;
            timer = _context.EnemyMovement.DistanceToPoint / idleTimer;
            Debug.Log($"DISTANCE TO POINT = {_context.EnemyMovement.DistanceToPoint}");
            Debug.Log($"IDLE TIMER = {timer}");
            
        }

        public void Exit()
        {
            isActive = false;   
        }

        public void Update()
        {
            if (!isActive) return;

            if (timer > 0.0f)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                stateMachine.ChangeState(stateMachine.roamingState);
            }
        }
    }
}