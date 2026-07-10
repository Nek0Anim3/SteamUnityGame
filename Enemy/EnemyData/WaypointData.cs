using System.Collections.Generic;
using Enemy.Structs;
using UnityEngine;

namespace Enemy.EnemyData
{
    [CreateAssetMenu(fileName = "WaypointData", menuName = "NPC/Waypoint Data")]
    public class WaypointData : ScriptableObject
    {
        [SerializeField] public List<NavWaypoint> NAV_waypoints = new List<NavWaypoint>();
        private List<NavWaypoint> VisitedWaypoints = new List<NavWaypoint>();
        //Editor visibility
        private EnemyMovement NPC_Movement;
        public float gizmoRadius = 0.5f;
        public Color WaypointColor = Color.darkCyan;
        public bool ShowConnections = true;
        //-----------------
        public NavWaypoint SelectNextRandomPoint(Vector3 scanPos, float radius)
        {
            List<NavWaypoint> filteredWaypoints = new List<NavWaypoint>();
            for (int i = 0; i < NAV_waypoints.Count; i++)
            {
                if (VisitedWaypoints.Contains(NAV_waypoints[i])) continue;

                float distance = Vector3.Distance(scanPos, NAV_waypoints[i].position);
                if (distance < radius)
                {
                    filteredWaypoints.Add(NAV_waypoints[i]);
                }
            }
            /*Debug.Log($"[NAV] Waypoints count = {NAV_waypoints.Count}");
            Debug.Log($"[NAV] Filtered count = {filteredWaypoints.Count}");*/
            NavWaypoint selectedPoint = filteredWaypoints[Random.Range(0, filteredWaypoints.Count)];
            /*Debug.Log($"[NAV] Adding to visited wpt {selectedPoint.position}");*/
            VisitedWaypoints.Add(selectedPoint);
            return selectedPoint;
        }


    }
}