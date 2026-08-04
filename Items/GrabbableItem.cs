using UnityEngine;

public class GrabbableItem : MonoBehaviour
{
    public float OBJECT_MASS;
    public float OBJECT_GRAB_DISTANCE;
    [SerializeField] private bool FREEZE_ROTATION;
    public Rigidbody objectRb;
    public bool IsGrabbed {get; private set;}

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

    public void OnGrab()
    {
        IsGrabbed = true;
        objectRb.useGravity = false;
        objectRb.linearDamping = 10.0f;
        objectRb.angularDamping = 10.0f;
        if (FREEZE_ROTATION)
        {
            objectRb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    public void OnRelease(Vector3 throwVel)
    {
        IsGrabbed = false;
        objectRb.useGravity = true;
        objectRb.linearDamping = _DRAG;
        objectRb.angularDamping = _ANGDRAG;
        objectRb.constraints = _RBCONSTR;
        objectRb.linearVelocity = throwVel;
    }
   
}
