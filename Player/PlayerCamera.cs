using System;
using FishNet.Object;
using Player.PlayerMovement;
using UnityEngine;

public class PlayerCamera : NetworkBehaviour
{
    [SerializeField] private Camera CameraPrefab;
    [SerializeField] private Transform cameraHolder;
    private Camera playerCam;
    public event Action<Camera, Transform> OnCameraReady;
    public bool Ready = false;
    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!IsOwner) return; 
        playerCam = Instantiate(CameraPrefab, cameraHolder);
        playerCam.transform.localPosition = Vector3.zero;
        
        OnCameraReady?.Invoke(playerCam, cameraHolder);
        Ready = true;
    }

    public Camera GetCam()
    {
        return playerCam;
    }

    public Transform GetCameraHolder()
    {
        return cameraHolder;
    }
}
