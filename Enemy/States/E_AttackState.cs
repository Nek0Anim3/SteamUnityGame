using UnityEngine;

namespace Enemy.States
{
    public class E_AttackState : IEnemyStates
    {
        private bool isActive;
        
        private EnemyState stateMachine;
        private EnemyContext _context;
        
        //=========================
        // REMOVE LATER
        //=========================
        private const float DAMAGE_AMOUNT = 10.0f;
        
        public E_AttackState(EnemyState machine, EnemyContext ctx)
        {
            stateMachine = machine;
            _context = ctx;
        }
        public void Enter()
        {
            isActive = true;
            stateMachine.StopChaseUpdate();
            //========================================
            // CHANGE THIS LOGIC TO INSTANT-KILL LATER
            //========================================
            _context.EnemyRaycaster.NearestPlayer.gameObject.GetComponent<PlayerHealth>().TakeDamage(DAMAGE_AMOUNT);
            
            //=======================================
            _context.EnemyMovement.SetNewWaypoint(_context.EnemyMovement.transform.position);
        }

        public void Exit()
        {
            isActive = false;
        }

        public void Update()
        {
            if (!isActive) return;
            float distance = Vector3.Distance(_context.EnemyMovement.transform.position, _context.EnemyRaycaster.NearestPlayer.transform.position);
            if (distance > stateMachine.ATTACK_DISTANCE + 1.5f)
            {
                stateMachine.ChangeState(stateMachine.chaseState);
            }
        }
    }
}