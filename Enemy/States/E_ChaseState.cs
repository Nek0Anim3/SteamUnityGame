using UnityEngine;

namespace Enemy.States
{
    public class E_ChaseState : IEnemyStates
    {
        private EnemyState stateMachine;
        private EnemyContext _context;
        public readonly float SEARCH_BASE_TIME = 5.0f;
        public float SearchTime;

        public E_ChaseState(EnemyState machine, EnemyContext ctx)
        {
            stateMachine = machine;
            _context = ctx;
            SearchTime = SEARCH_BASE_TIME;
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