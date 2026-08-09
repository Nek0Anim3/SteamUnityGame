using Unity.Netcode;
using UnityEngine;

public class GrabbableItem : NetworkBehaviour
{
    public float OBJECT_MASS;
    public float OBJECT_GRAB_DISTANCE;
    [SerializeField] private bool FREEZE_ROTATION;
    public Rigidbody objectRb;

    public NetworkVariable<bool> IsGrabbed = new NetworkVariable<bool>(
        false,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner
        );

    private float _DRAG;
    private float _ANGDRAG;
    private bool _USEGRAVITY;
    private RigidbodyConstraints _RBCONSTR;
    
    
    private void Awake()
    {
        _DRAG = objectRb.linearDamping;
        _ANGDRAG = objectRb.angularDamping;
        _USEGRAVITY = objectRb.useGravity;
        _RBCONSTR = objectRb.constraints;
    }
    
    public override void OnNetworkSpawn()
    {
        objectRb.isKinematic = !IsOwner;
    }
    public void OnGrab()
    {
        objectRb.isKinematic = false;
        objectRb.useGravity = false;
        objectRb.linearDamping = 10.0f;
        objectRb.angularDamping = 10.0f;
        if (FREEZE_ROTATION)
        {
            objectRb.constraints = RigidbodyConstraints.FreezeRotation;
        }
        IsGrabbed.Value = true;
    }

    public void OnRelease(Vector3 throwVel)
    {
        IsGrabbed.Value = false;
        objectRb.useGravity = true;
        objectRb.linearDamping = _DRAG;
        objectRb.angularDamping = _ANGDRAG;
        objectRb.constraints = _RBCONSTR;
        objectRb.linearVelocity = throwVel;
        if (IsOwner) ReturnOwnershipRpc();
    }

    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    private void ReturnOwnershipRpc()
    {
        NetworkObject.RemoveOwnership();
    }
   
}
