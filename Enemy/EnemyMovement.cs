using Enemy;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : NetworkBehaviour
{
    public static EnemyMovement Instance;
    private NavMeshAgent navAgent;
    //debug waypoint

    
    private void Awake()
    {
        Instance = this;

    }
    
    public override void OnNetworkSpawn()
    {
        if (!IsServer) { enabled = false; }
        navAgent = GetComponent<NavMeshAgent>();

    }
    
    public void SetNewWaypoint(Vector3 destination)
    {
        if (navAgent.SetDestination(destination))
        {
            UnityEngine.Debug.Log("[NPC] Going to waypoint");
        }
    }
}
