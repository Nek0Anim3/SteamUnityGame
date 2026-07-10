using Network;

namespace Player
{
    public class PlayerDataFactory
    {
        public static IPlayerData Create(NetworkMode mode) => mode switch
        {
            NetworkMode.DirectIP => new LocalData(),
            NetworkMode.Steam => new SteamData()
        };
    }
}