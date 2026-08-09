using System;
using Player;
using UnityEngine;

namespace DevTools
{
    public class OOBTrigger : MonoBehaviour
    { 
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Debug.Log("[OOB TRIGGER] Player collided!!");
                var script = collision.gameObject.GetComponent<PlayerOOBTeleport>();
                script.RespawnPlayer();
            }
        }
    }
}