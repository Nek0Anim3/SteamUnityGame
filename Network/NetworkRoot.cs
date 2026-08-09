
using FishNet.Managing;
using FishNet.Transporting;
using UnityEngine;

namespace Network
{
    public class NetworkRoot : MonoBehaviour
    {
        public NetworkManager NetworkManager;
        public Transport Transport;
        public UINetBus UI;
        public FishySteamworks.FishySteamworks SteamTransport;
    }
}