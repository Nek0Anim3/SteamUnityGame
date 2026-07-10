using System.Collections.Generic;
using Enemy.EnemyData;
using Enemy.Structs;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;


public class EnemyWaypointNav : MonoBehaviour
{

    private EnemyMovement NPC_Movement;
    [SerializeField] private WaypointData NAV_waypointData;
    public WaypointData Data => NAV_waypointData;
    private Dictionary<string, NavWaypoint> VisitedWaypoints;
    private Vector3 currentWaypoint;

    
    private void Awake()
    {
        NPC_Movement = GetComponent<EnemyMovement>();
        currentWaypoint = NAV_waypointData.SelectNextRandomPoint(NPC_Movement.transform.position, 3.0f).position;
        VisitedWaypoints = new Dictionary<string, NavWaypoint>();
    }

    public void SetNewDestination(Vector3 searchPosition)
    {
        NavWaypoint waypoint = NAV_waypointData.SelectNextRandomPoint(searchPosition, 6.0f);
        NPC_Movement.SetNewWaypoint(waypoint.position);
        currentWaypoint = waypoint.position;
    }
    
    public Vector3 GetCurrentWaypoint()
    {
        return currentWaypoint;
    }
    
    
    
}
