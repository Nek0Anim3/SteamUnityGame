// Out of bounds trigger script that snaps backs to reality (tp player to spawn if OOB)

using FishNet.Object;
using UnityEngine;

namespace Player
{
    public class PlayerOOBTeleport : NetworkBehaviour
    {
        private BoxCollider _collider;
        [SerializeField] private Vector3 _SpawnPoint;

        public void RespawnPlayer()
        {
            if (IsOwner)
            {
                SnapBackToRealityRpc();
            }
        }
        
        [ServerRpc(RequireOwnership = false)]
        private void SnapBackToRealityRpc()
        {
            transform.position = _SpawnPoint;
        }
    }
}