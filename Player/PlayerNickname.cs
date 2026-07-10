using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerNickname : NetworkBehaviour
{
    public NetworkVariable<FixedString128Bytes> Nickname = new NetworkVariable<FixedString128Bytes>(
        "Default PlayerName",
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );
    

    [SerializeField] private TMP_Text nicknameText;
    void Start()
    {
        Nickname.OnValueChanged += UpdateNickname;
    }

    
    public override void OnNetworkSpawn()
    {
       
        Nickname.OnValueChanged += UpdateNickname;
        UpdateNicknameUI(Nickname.Value.ToString());
        if (IsOwner)
        {
            string myName = PlayerPrefs.Nickname;
            SetNicknameServerRpc(myName);
        }
    }
    
    //net
    public override void OnNetworkDespawn()
    {
        Nickname.OnValueChanged -= UpdateNickname;
    }
    
    [ServerRpc]
    private void SetNicknameServerRpc(string newName)
    {
        Nickname.Value = newName;
    }
    
    private void UpdateNickname(FixedString128Bytes oldValue, FixedString128Bytes value)
    {
        UpdateNicknameUI(value.ToString());
    }
    private void UpdateNicknameUI(string nameToDisplay)
    {
        if (nicknameText != null)
        {
            nicknameText.text = nameToDisplay;
        }
    }
}
