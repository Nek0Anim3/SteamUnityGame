using UnityEngine;

namespace Enemy.States
{
    public class E_ChaseState : IEnemyStates
    {
        private EnemyState stateMachine;
        private EnemyContext _context;
        
        //brbrbrpatapim
        public E_ChaseState(EnemyState machine, EnemyContext ctx)
        {
            stateMachine = machine;
            _context = ctx;
        }

        public void Enter()
        {
            Debug.Log("[NPC] ENTERED CHASE STATE!");
        }

        public void Exit()
        {
            
        }

        public void Update()
        {
            
        }
    }
}