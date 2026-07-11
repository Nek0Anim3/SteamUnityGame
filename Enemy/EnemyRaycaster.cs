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
            //FIX THIS PLS ...
            if (!isActive) return;
            TargetPosition = playerCollider.transform.position;
            if (Physics.Raycast(transform.position, TargetPosition, out RaycastHit hit, 20.0f))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    Debug.Log("PLAYER HITS RAYCAST");
                    OnPlayerRaycastVisible?.Invoke();
                    isActive = false;
                }
            } 
            
        }
    }
}