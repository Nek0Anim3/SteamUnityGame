using UnityEngine;

namespace Enemy.States
{
    public class E_ChaseState : IEnemyStates
    {
        private EnemyState stateMachine;
        private EnemyContext _context;
        public float SearchTime = 10.0f;

        public E_ChaseState(EnemyState machine, EnemyContext ctx)
        {
            stateMachine = machine;
            _context = ctx;
        }

        public void Enter()
        {
            stateMachine.ChaseStartUpdate();
        }
        
        public void Exit()
        {
            stateMachine.roamingState.lastPos = _context.EnemyMovement.transform.position;
            stateMachine.StopChaseUpdate();
        }
        
        public void Update()
        {
            
        }
    }
}