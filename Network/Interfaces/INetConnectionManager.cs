

using System;
using System.Threading.Tasks;
using UnityEngine.UI;

public interface INetConnectionService
{
    Task<bool> StartHosting();
    Task<bool> StartClientConnection();

    void Disconnect();

}
