using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerItemGrabber : NetworkBehaviour
    {
        [SerializeField] private HUDInitializer HUDController;
        private UICrosshair crosshair;
        
        [SerializeField] Camera playerCam;
        
        [SerializeField] private float _GRABRANGE;
        [SerializeField] private float _HOLDDIST;
        [SerializeField] private float _MIN_HOLDDIST;
        [SerializeField] private float _MAX_HOLDDIST;
        [SerializeField] private float _SCROLLSENS;
        [SerializeField] private LayerMask _interactableLayer;

        [SerializeField] private float _FOLLOWSPEED;
        [SerializeField] private float _MAXFOLLFORCE;
        [SerializeField] private float _VELOCITYDAMPING;
        [SerializeField] private float _THROWFORCE;
        [SerializeField] private int _THROWFRAMES = 5;

        private GrabbableItem _heldItem;
        [SerializeField] private InputActionAsset _inputActionAsset;
        private InputAction _holdAction;
        private InputAction _scrollAction;
        
        private Queue<Vector3> _cameraVelocityHistory = new Queue<Vector3>();
        private Vector3 _lastCameraPos;
        
        private void Awake()
        {
            _holdAction = _inputActionAsset.FindActionMap("Player").FindAction("Grab");
            _scrollAction = _inputActionAsset.FindActionMap("Player").FindAction("Scroll");
        }

        private void Start()
        {
            crosshair = HUDController.GetCrosshair();
            _holdAction.Enable();
            _scrollAction.Enable();
            _holdAction.started += OnGrabInput;
            _holdAction.canceled += OnReleaseInput;
        }

        private void OnDisable()
        {
            _holdAction.started -= OnGrabInput;
            _holdAction.canceled -= OnReleaseInput;
            _holdAction.Disable();
            _scrollAction.Disable();
        }

        private void Update()
        {
            TrackCameraVelocity();
            if (_heldItem == null) return;
            
            float scroll = _scrollAction.ReadValue<Vector2>().y;
            if (Mathf.Abs(scroll) > 0.01f)
            {
                _HOLDDIST = Mathf.Clamp(_HOLDDIST + scroll * _SCROLLSENS, _MIN_HOLDDIST, _MAX_HOLDDIST);
            }
            
            float distToItem = Vector3.Distance(playerCam.transform.position, _heldItem.transform.position);
            if (distToItem > _heldItem.OBJECT_GRAB_DISTANCE * 1.5f)
            {
                Release(drop: true);
            }
        }

        private void FixedUpdate()
        {
            if (_heldItem==null) return;
            MoveHeldObject();
        }
        
        private void OnGrabInput(InputAction.CallbackContext ctx)
        {
            if (_heldItem != null) return;

            TryGrab();
        }

        private void OnReleaseInput(InputAction.CallbackContext ctx)
        {
            if (_heldItem == null) return;
            crosshair.SetCrosshair(0);
            Release(drop: false);
        }

        private void TryGrab()
        {
            Ray ray = playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
            if (!Physics.Raycast(ray, out RaycastHit hit, _GRABRANGE, _interactableLayer)) return;
            GrabbableItem grabbedItem = hit.collider.GetComponent<GrabbableItem>();
            if (grabbedItem == null || grabbedItem.IsGrabbed.Value) return;
            
            _HOLDDIST = Mathf.Clamp(hit.distance, _MIN_HOLDDIST, _MAX_HOLDDIST);
            RequestGrabRpc(grabbedItem.NetworkObject.NetworkObjectId);
        }
        
        private void Release(bool drop)
        {
            if (_heldItem == null) return;
            Vector3 throwVel = drop ? Vector3.zero : GetAverageCamVel() * _THROWFORCE;
            
            _heldItem.OnRelease(throwVel);
            _heldItem = null;
        }

        private void MoveHeldObject()
        {
            Vector3 targetPos = playerCam.transform.position + playerCam.transform.forward * _HOLDDIST;
            Vector3 toTarget = targetPos - _heldItem.objectRb.position;
            float distance = toTarget.magnitude;
            float speedFactor = _FOLLOWSPEED / _heldItem.OBJECT_MASS;
            Vector3 targetVel = toTarget * speedFactor;
            targetVel = Vector3.ClampMagnitude(targetVel, _MAXFOLLFORCE);
            
            //moving item main lerp
            _heldItem.objectRb.linearVelocity = Vector3.Lerp(_heldItem.objectRb.linearVelocity, targetVel, Time.fixedDeltaTime * speedFactor);

            _heldItem.objectRb.angularVelocity *= _VELOCITYDAMPING;
        }
        
        private void TrackCameraVelocity()
        {
            Vector3 currentPos = playerCam.transform.position;
            Vector3 frameVel = (currentPos - _lastCameraPos) / Time.deltaTime;
            _lastCameraPos = currentPos;
            _cameraVelocityHistory.Enqueue(frameVel);
            if (_cameraVelocityHistory.Count > _THROWFRAMES)
            {
                _cameraVelocityHistory.Dequeue();
            }
        }

        private Vector3 GetAverageCamVel()
        {
            if (_cameraVelocityHistory.Count == 0) return Vector3.zero;
            Vector3 sum = Vector3.zero;
            foreach (var v in _cameraVelocityHistory)
            {
                sum += v;
            }
            return sum / _cameraVelocityHistory.Count;
        }
        
        
        //========================= 
        // NETWORK
        //======================
        [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
        private void RequestGrabRpc(ulong netObjId, RpcParams rpcParams = default)
        {
            if (!NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(netObjId, out var obj)) return;
            GrabbableItem grabbable = obj.GetComponent<GrabbableItem>();
            if (grabbable.IsGrabbed.Value) return; //if .Value == true

            ulong reqId = rpcParams.Receive.SenderClientId;
            obj.ChangeOwnership(reqId); // Gives 'Owner' to client
            ConfirmGrabRpc(netObjId, RpcTarget.Single(reqId, RpcTargetUse.Temp));
        }

        [Rpc(SendTo.SpecifiedInParams)]
        private void ConfirmGrabRpc(ulong netObjId, RpcParams rpcParams)
        {
            NetworkObject netObj = NetworkManager.Singleton.SpawnManager.SpawnedObjects[netObjId];
            GrabbableItem grabbable = netObj.GetComponent<GrabbableItem>();
            grabbable.OnGrab();
            _heldItem = grabbable;
            crosshair.SetCrosshair(2);
            _cameraVelocityHistory.Clear();
        }
    }
}