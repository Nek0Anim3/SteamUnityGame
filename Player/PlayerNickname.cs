using FishNet.Object;
using FishNet.Object.Synchronizing;
using TMPro;
using Unity.Collections;
using UnityEngine;

public class PlayerNickname : NetworkBehaviour
{
    private readonly SyncVar<string> Nickname = new SyncVar<string>("Name",
        new SyncTypeSettings(WritePermission.ServerOnly, ReadPermission.Observers));
    
    

    [SerializeField] private TMP_Text nicknameText;
    void Start()
    {
        Nickname.OnChange += UpdateNickname;
    }


    public override void OnStartClient()
    {
        Nickname.OnChange += UpdateNickname;
        UpdateNicknameUI(Nickname.Value.ToString());
        if (IsOwner)
        {
            string myName = PlayerPrefs.Nickname;
            SetNicknameServerRpc(myName);
        }
    }
    
    //net
    public override void OnStopClient()
    {
        Nickname.OnChange -= UpdateNickname;
    }

    
    [ServerRpc]
    private void SetNicknameServerRpc(string newName)
    {
        Nickname.Value = newName;
    }
    
    private void UpdateNickname(string oldValue, string value, bool asServer)
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
