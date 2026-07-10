using Network;
using Player;
using Steamworks;
using UnityEngine;

namespace GameManagement
{
    public class GameBootstrap : MonoBehaviour
    {
        public static GameBootstrap Instance { get; private set; }
        
        public INetConnectionService Connection { get; private set; }
        private INetPolling _polling;
        public IPlayerData PlayerDataManager;
        
        [SerializeField] private NetworkRoot root;
        [SerializeField] private NetworkMode networkMode;
        [SerializeField] private string debugNickName;
        
        private void Awake()
        {
            if (Instance != null) {Destroy(gameObject); return;}
            
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Connection = ConnectionServiceFactory.Create(networkMode, root);
            PlayerDataManager = PlayerDataFactory.Create(networkMode);
            _polling = Connection as INetPolling; 
            
            

            PlayerDataManager.SetNickname(debugNickName);
            PlayerDataManager.SetUID(0);    
        }

        
        
        private void Update()
        {
            _polling?.Tick();
        }
    }
}