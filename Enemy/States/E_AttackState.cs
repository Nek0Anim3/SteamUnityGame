namespace Enemy.States
{
    public class E_AttackState : IEnemyStates
    {
        private EnemyState stateMachine;
        private EnemyContext _context;
        public float SearchTime;
        private const float DAMAGE_AMOUNT = 10.0f;
        public E_AttackState(EnemyState machine, EnemyContext ctx)
        {
            stateMachine = machine;
            _context = ctx;
        }
        public void Enter()
        {
            stateMachine.StopChaseUpdate();
            _context.EnemyRaycaster.NearestPlayer.gameObject.GetComponent<PlayerHealth>().TakeDamage(DAMAGE_AMOUNT);
            _context.EnemyMovement.SetNewWaypoint(_context.EnemyMovement.transform.position);
        }

        public void Exit()
        {
            //
        }

        public void Update()
        {
            //
        }
    }
}