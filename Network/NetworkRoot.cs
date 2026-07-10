using Netcode.Transports.Facepunch;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

namespace Network
{
    public class NetworkRoot : MonoBehaviour
    {
        public NetworkManager NetworkManager;
        public UnityTransport UnityTransport;
        public FacepunchTransport FacepunchTransport;
        public UINetBus UI;
    }
}