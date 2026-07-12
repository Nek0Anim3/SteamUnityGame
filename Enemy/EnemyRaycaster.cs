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
            foreach (Collider player in playerCollider)
            {
                if (Vector3.Distance(player.transform.position, transform.position) < Vector3.Distance(minPlayerPos, transform.position))
                {
                    minPlayerPos = player.transform.position;
                    NearestPlayer = player;    
                }
            }

            if (NearestPlayer != null)
            {
                RaycastPlayerPos();
            }
        }

        public void RaycastPlayerPos()
        {
            //TODO Fix raycast hit, doesnt detect player
            if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), NearestPlayer.transform.position - transform.position, out RaycastHit hit, 20.0f,wallLayerMask)) 
            {
                Debug.DrawLine(transform.position, hit.point, Color.green);
                if (hit.collider.CompareTag("Player"))
                {
                    if (playerInSight == false) { playerInSight = true; }
                    
                }
            }
            else
            {
                Debug.DrawLine(new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), NearestPlayer.transform.position - transform.position, Color.red);
                playerInSight = false;
            }
        }
    }
}