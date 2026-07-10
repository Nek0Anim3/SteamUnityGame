using System;
using Unity.Collections;
using Unity.Netcode;

namespace Network.GameControl
{
    public struct LobbyPlayerNames : INetworkSerializable, IEquatable<LobbyPlayerNames>
    {
        public ulong ClientId;
        public FixedString64Bytes Name;
        
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref ClientId);
            serializer.SerializeValue(ref Name);
            
        }

        public bool Equals(LobbyPlayerNames other) => ClientId == other.ClientId;
    }
}