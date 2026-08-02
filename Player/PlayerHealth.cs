using Player;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PlayerHealth : NetworkBehaviour, IDamageable
{
    public NetworkVariable<float> playerHealth = new NetworkVariable<float>(100.0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
     
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        playerHealth.OnValueChanged += OnHealthChanged;
    }

    public override void OnNetworkDespawn()
    {
        playerHealth.OnValueChanged -= OnHealthChanged;
        base.OnNetworkDespawn();
    }

    private void OnHealthChanged(float oldVal, float newVal)
    {
        Debug.Log($"Client hp changed to -> {newVal}");
    }

    public void TakeDamage(float damage)
    {
        if (IsServer) 
        {
            playerHealth.Value -= damage;
            return;
        }
        TakeDamageServerRpc(damage); 
    }
    
    [ServerRpc]
    private void TakeDamageServerRpc(float amount)
    {
        Debug.Log($"Player taken damage -> IN AMOUNT: {amount}");
        playerHealth.Value -= amount;
    }
}
