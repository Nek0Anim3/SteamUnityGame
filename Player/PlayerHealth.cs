using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PlayerHealth : NetworkBehaviour
{
    public NetworkVariable<float> playerHealth = new NetworkVariable<float>(100.0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    
    [SerializeField] private TMP_Text healthText;
    
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
        healthText.text = newVal.ToString("F0");
    }

    public void TakeDamage(float amount)
    {
        if (IsServer) 
        {
            playerHealth.Value -= amount;
            return;
        }
        TakeDamageServerRpc(amount); 
    }
    
    [ServerRpc]
    public void TakeDamageServerRpc(float amount)
    {
        playerHealth.Value -= amount;
    }
}
