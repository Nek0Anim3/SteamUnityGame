using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

namespace Enemy.States
{
    public class E_RoamingState : IEnemyStates
    {
        private EnemyContext _context;
        private EnemyState stateMachine;
        private bool isActive;
        private bool isMoving;
        public Vector3 lastPos;

        private Vector3 currentWpt;
        //DEBUG AREA
        public List<Vector3> visitedPoints_DEBUG = new List<Vector3>();
        
        //конструктор
        public E_RoamingState(EnemyState machine, EnemyContext ctx)
        {
            UnityEngine.Debug.Log("[INIT] E_RoamingState created!");
            stateMachine = machine;
            _context = ctx;
            lastPos = machine.transform.position;
            
            
            
        }
        
        
        public void Enter()
        {
            isActive = true;

            _context.EnemyWaypointNav.SetNewDestination(lastPos);
            currentWpt = _context.EnemyWaypointNav.GetCurrentWaypoint();
            lastPos = _context.EnemyMovement.transform.position;
            //DEBUG AREA
            visitedPoints_DEBUG.Add(lastPos);
            isMoving = true;
        }

        public void Exit()
        {
            isActive = false;   
        }

        
        
        public void Update()
        {
            if (!isActive) return;
            if (isMoving)
            {
                if (Vector3.Distance(_context.EnemyMovement.transform.position, currentWpt) < 1.0f)
                {
                    isMoving = false;
                    stateMachine.ChangeState(stateMachine.idleState);
                }
            }
        }
    }
}