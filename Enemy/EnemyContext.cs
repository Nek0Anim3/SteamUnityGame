using UnityEngine;

namespace Enemy
{
    public class EnemyContext : MonoBehaviour
    {
        private EnemyContext Instance;
        public EnemyWaypointNav EnemyWaypointNav { get; private set; }
        public EnemyMovement EnemyMovement  { get; private set; }

        private void Awake()
        {
            Instance = this;
            EnemyWaypointNav = GetComponent<EnemyWaypointNav>();
            EnemyMovement = GetComponent<EnemyMovement>();
            Debug.Log("[NPC] EnemyContext created!");
        }
    }
}