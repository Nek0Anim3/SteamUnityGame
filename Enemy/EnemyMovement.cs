using Enemy;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : NetworkBehaviour
{
    public static EnemyMovement Instance;
    private NavMeshAgent navAgent;
    //debug waypoint
    private Vector3 currentWpt;
    public float DistanceToPoint { get; private set; }
    public bool isMoving = false;
    private void Awake()
    {
        Instance = this;

    }
    
    public override void OnNetworkSpawn()
    {
        if (!IsServer) { enabled = false; }
        navAgent = GetComponent<NavMeshAgent>();
        DistanceToPoint = 5.0f;

    }

    
    public void SetNewWaypoint(Vector3 destination)
    {
        if (navAgent.SetDestination(destination))
        {
            DistanceToPoint = Vector3.Distance(transform.position, destination);
            currentWpt = destination;
            if (!isMoving)
            {
                isMoving = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, currentWpt) < 2.0f)
        {
            isMoving = false;
        }
    }
}
