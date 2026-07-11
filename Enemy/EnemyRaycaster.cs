using System;
using UnityEngine;

namespace Enemy
{
    public class EnemyRaycaster : MonoBehaviour
    {
        public event Action OnPlayerRaycastVisible;
        public Vector3 TargetPosition { get; private set; }
        private bool isActive;
        
        private Collider playerCollider;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isActive = true;
                playerCollider = other;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isActive = false;
            }
        }

        private void FixedUpdate()
        {
            if (!isActive) return;
            TargetPosition = playerCollider.transform.position;
            
            if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.5f), TargetPosition - transform.position, out RaycastHit hit)) 
            {
                Debug.DrawLine(transform.position, hit.point, Color.green);
                if (hit.collider.CompareTag("Player"))
                {
                    Debug.Log("[NPC] Raycast has LOS with player!");
                    OnPlayerRaycastVisible?.Invoke();
                    isActive = false;
                }
            }

        }
    }
}