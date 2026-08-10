using FishNet.Object;
using UnityEngine;
using UnityEngine.InputSystem;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public enum CrouchState
{
    Standing,
    Crouching
}

namespace Player.PlayerMovement
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : NetworkBehaviour
    {
        private Transform playerTransform;
        [SerializeField] private Transform orientation;   
        [SerializeField] private Transform cameraHolder;
        [SerializeField] private Camera CameraPrefab;
        [SerializeField] private Camera playerCam;
        [SerializeField] private Transform playerHead;
        [SerializeField] private PlayerStamina playerStamina;
        private Collider playerCollider;
        [SerializeField] private CapsuleCollider capsule;


        public InputActionAsset inputAsset;
        private InputAction moveAction;
        private InputAction jumpAction;
        private InputAction crouchAction;
        
        [Header("Player settings")]
        [SerializeField] [Range(0.01f, 0.2f)] private float mouseSensitivity;
        [SerializeField] private float accelerationSpeed;
        
        [Header("Movement settings (Debug)")]
        private Rigidbody rb;
        [SerializeField] [Range(1.0f, 20.0f)] private float maxSpeed;
        [SerializeField] private float moveSpeed = 4.6f;
        public float moveMultiplier = 1.0f;
        [SerializeField] private float jumpHeight = 1f;
        [SerializeField] private float gravity = -12.81f;
        [SerializeField] private float accelerationTime = 0.1f; 
        [SerializeField] private float decelerationTime = 0.15f; 

        private Vector3 _currentVelocity; 
        private Vector3 _smoothedMoveDirection;
        private Vector3 jumpForwardVec;
        private Vector3 jumpRightVec;

        private float CROUCH_HEIGHT;
        private float STANDING_HEIGHT;
        private float CAM_CROUCH_HEIGHT;
        private float CAM_STAND_HEIGHT;
        public CrouchState crouchState {get; private set;}
        private bool crouchInputHeld;
        private RaycastHit[] hitBuff = new RaycastHit[1];
        private Vector2 moveInput;
        private Vector2 lookInput;
        private float verticalVelocity;
        
        private bool isGrounded;
        private bool jumpRequested;
        public bool isMoving { get; private set; }
        
        [Header("Ground Check")]
        [SerializeField] private Transform groundCheck;
        [SerializeField] private float groundDistance = 0.2f;
        [SerializeField] private LayerMask groundMask;
        
        private float verticalRotation;
    
        private void Awake()
        {
            inputAsset = Instantiate(inputAsset);
            playerTransform = GetComponent<Transform>();
            rb = GetComponent<Rigidbody>();
            playerCollider = GetComponent<Collider>();

            rb.freezeRotation = true;
            rb.interpolation = RigidbodyInterpolation.Interpolate; 

            moveAction = inputAsset.FindActionMap("Player").FindAction("Move");
            jumpAction = inputAsset.FindActionMap("Player").FindAction("Jump");
            crouchAction = inputAsset.FindActionMap("Player").FindAction("Crouch");

            CROUCH_HEIGHT = capsule.height - 1.0f;
            STANDING_HEIGHT = capsule.height;
            CAM_STAND_HEIGHT = cameraHolder.transform.position.y;
            CAM_CROUCH_HEIGHT = CAM_STAND_HEIGHT - 0.5f;
        }


        public override void OnStartClient()
        {
            base.OnStartClient();
            if (IsOwner)
            {
                
                rb.isKinematic = false; 
                moveAction.Enable();
                jumpAction.Enable();
                crouchAction.Enable();

                jumpAction.performed += OnJump;
                crouchAction.started += OnStartCrouch;
                crouchAction.canceled += OnEndCrouch;

                playerCam = Instantiate(CameraPrefab, cameraHolder);
                playerCam.transform.localPosition = Vector3.zero;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Debug.Log(cameraHolder.localPosition);
                return;
            }
    
            rb.isKinematic = true; 
            moveAction.Disable();
            jumpAction.Disable();
        }
        
        public override void OnStopClient()
        {
            if (IsOwner) moveAction.Disable();
            base.OnStopClient();
        }

        private void OnJump(InputAction.CallbackContext ctx)
        {
            if (jumpRequested) return;
            jumpForwardVec = orientation.forward;
            jumpRightVec = orientation.right;
            jumpRequested = true;
        }

        private void OnStartCrouch(InputAction.CallbackContext ctx) => crouchInputHeld = true;
        private void OnEndCrouch(InputAction.CallbackContext ctx) => crouchInputHeld = false;
        
        private void RotateCamera()
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();
            float mouseX = mouseDelta.x * mouseSensitivity;
            float mouseY = mouseDelta.y * mouseSensitivity;

            verticalRotation -= mouseY;
            verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
            playerCam.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
            orientation.Rotate(Vector3.up * mouseX);
            cameraHolder.Rotate(Vector3.up * mouseX);                
        }

        private void UpdateCrouchState()
        {
            if (crouchInputHeld)
            {
                if (crouchState != CrouchState.Crouching)
                {
                    crouchState = CrouchState.Crouching;
                    playerStamina.ForceStopSprint();
                }
            }
            else if (crouchState == CrouchState.Crouching && CanStandup())
            {
                crouchState = CrouchState.Standing;
            }
            moveMultiplier = crouchState == CrouchState.Crouching ? 0.6f : (playerStamina.isSprinting ? 1.5f : 1.0f);
            ApplyCrouch();
        }

        private void ApplyCrouch()
        {
            float targetHeight = crouchState == CrouchState.Crouching ? CROUCH_HEIGHT : STANDING_HEIGHT;
            capsule.height = Mathf.Lerp(capsule.height, targetHeight, Time.deltaTime * 10.0f);
        }
        
        private bool CanStandup()
        {
            Ray ray = new Ray(playerHead.transform.position, Vector3.up);
            int hitCount = Physics.SphereCastNonAlloc(ray, 0.51f, hitBuff, 0.5f, groundMask);
            if (hitCount > 0)
            {
                return false;
            }
            return true;
        }
        
        void Update()
        {
            if (!IsOwner) return;
            RotateCamera();
            UpdateCrouchState();
        }
      
        private void FixedUpdate()
        {
            if (!IsOwner) return;
            HandleGravityAndGround();
            Vector3 targetDirection;
            if (isGrounded)
            {
                isMoving = moveInput.magnitude > 0.0f;
                moveInput = moveAction.ReadValue<Vector2>();
                targetDirection = (orientation.forward * moveInput.y + orientation.right * moveInput.x).normalized * (moveSpeed * moveMultiplier);
            }
            else
            {
                isMoving = false;
                targetDirection = (jumpForwardVec * moveInput.y + jumpRightVec * moveInput.x).normalized * moveSpeed;
            }

            float smoothTime = moveInput.magnitude > 0.01f ? accelerationTime : decelerationTime;
            _smoothedMoveDirection = Vector3.SmoothDamp(_smoothedMoveDirection, targetDirection, ref _currentVelocity, smoothTime);

            rb.linearVelocity = new Vector3(_smoothedMoveDirection.x, verticalVelocity, _smoothedMoveDirection.z);
            //========
            // UI DEBUG
            /*UIDebug.Instance.MOVESPEED = rb.linearVelocity.magnitude - 2.2f;*/
            //========
        }

        private void HandleGravityAndGround()
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if (isGrounded && verticalVelocity < 0)
            {
                verticalVelocity = -2f; 
            }

            if (jumpRequested && isGrounded)
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        
            jumpRequested = false; 
            verticalVelocity += gravity * Time.fixedDeltaTime;
        }
    }
}
