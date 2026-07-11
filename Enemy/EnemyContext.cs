using UnityEngine;

namespace Enemy
{
    public class EnemyContext : MonoBehaviour
    {
        private EnemyContext Instance;
        public EnemyWaypointNav EnemyWaypointNav { get; private set; }
        public EnemyMovement EnemyMovement  { get; private set; }
        
        public EnemyRaycaster EnemyRaycaster {get; private set;}
        
        
        private void Awake()
        {
            Instance = this;
            EnemyWaypointNav = GetComponent<EnemyWaypointNav>();
            EnemyMovement = GetComponent<EnemyMovement>();
            EnemyRaycaster = GetComponent<EnemyRaycaster>();
            Debug.Log("[NPC] EnemyContext created!");
        }
    }
}