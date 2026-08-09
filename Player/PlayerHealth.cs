using FishNet.CodeGenerating;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using Player;
using TMPro;

using UnityEngine;

public class PlayerHealth : NetworkBehaviour, IDamageable
{
    private readonly SyncVar<float> playerHealth = new SyncVar<float>(100.0f,new SyncTypeSettings(WritePermission.ServerOnly, ReadPermission.Observers));
    public float PlayerHp => playerHealth.Value;
    
    public override void OnStartClient()
    {
        base.OnStartClient();
        playerHealth.OnChange += OnHealthChanged;
    }

    public override void OnStopClient()
    {
        playerHealth.OnChange -= OnHealthChanged;
        base.OnStopClient();
    }

    private void OnHealthChanged(float oldVal, float newVal, bool asServer)
    {
        Debug.Log($"Client hp changed to -> {newVal}");
    }

    public void TakeDamage(float damage)
    {
        if (IsServerInitialized) 
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
