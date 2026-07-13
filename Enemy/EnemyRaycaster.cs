using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace Enemy
{
    public class EnemyRaycaster : MonoBehaviour
    {
        [SerializeField] private LayerMask wallLayerMask; 
        private Vector3 minPlayerPos;
        public Collider NearestPlayer;
        public bool playerInSight;
        private List<Collider> playerCollider;

        private void Awake()
        {
            playerCollider = new List<Collider>();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")) 
            {
                if (!playerCollider.Contains(other))
                {
                    Debug.Log("Player added to List<Collision>");
                    playerCollider.Add(other);
                    NearestPlayer = other;
                    minPlayerPos = other.transform.position;
                }
                
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerCollider.Remove(other);
            }
        }

        private void FixedUpdate()
        {
            if (playerCollider.Count > 1)
            {
                float minDist = float.MaxValue;

                foreach (Collider player in playerCollider)
                {
                    float dist = Vector3.Distance(player.transform.position, transform.position);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        minPlayerPos = player.transform.position;
                        NearestPlayer = player;
                    }
                }
            }

            if (NearestPlayer != null) RaycastPlayerPos();
        }

        public void RaycastPlayerPos()
        {
            Vector3 origin = transform.position + Vector3.up * 0.5f;
            Vector3 direction = NearestPlayer.transform.position - origin;
            if (Physics.Raycast(origin, direction, out RaycastHit hit, Vector3.Distance(origin, NearestPlayer.transform.position), wallLayerMask))
            {
                Debug.DrawLine(origin, hit.point, Color.red);
                playerInSight = false;
                NearestPlayer = null;
                
            }
            else
            {
                Debug.DrawLine(origin, NearestPlayer.transform.position, Color.green);
                playerInSight = true;
            }
        }
    }
}