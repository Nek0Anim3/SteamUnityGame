using System;
using Unity.Collections;

namespace Network.GameControl
{
    public struct LobbyPlayerNames : IEquatable<LobbyPlayerNames>
    {
        public int ClientId;
        public FixedString64Bytes Name;
        public bool Equals(LobbyPlayerNames other) => ClientId == other.ClientId;
    }
}