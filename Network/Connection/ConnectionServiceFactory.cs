namespace Network
{
    public class ConnectionServiceFactory
    {
        public static INetConnectionService Create(NetworkMode mode, NetworkRoot root) => mode switch
        {
            NetworkMode.DirectIP => new DirectConnectionService(root),
            NetworkMode.Steam => new SteamConnectionService(root),
        };
    }
}