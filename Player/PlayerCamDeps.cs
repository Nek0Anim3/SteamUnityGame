using FishNet.Object;
using Player.PlayerMovement;
using UnityEngine;

namespace Player
{
    public class PlayerCamDeps : NetworkBehaviour
    {
        [SerializeField] private PlayerCamera PlayerCamScript;
        
        //All components that need camera
        [SerializeField] private HUDItemRaycaster HUDItemRaycaster;
        [SerializeField] private PlayerController PlayerController;
        [SerializeField] private PlayerItemGrabber PlayerItemGrabber;
        
        public override void OnStartClient()
        {
            base.OnStartClient();
            if (!IsOwner) return;
            PlayerCamScript.OnCameraReady += Initialize;
            if (PlayerCamScript.Ready)
            {
                Camera cam = PlayerCamScript.GetCam();
                Transform camHold = PlayerCamScript.GetCameraHolder();
                Initialize(cam, camHold);
            }
        }

        private void Initialize(Camera cam, Transform camHold)
        {
            Debug.Log("Initializing player camera");
            HUDItemRaycaster.InitCamera(cam, camHold);
            PlayerController.InitCamera(cam, camHold);
            PlayerItemGrabber.InitCamera(cam, camHold);
        }
    }
}