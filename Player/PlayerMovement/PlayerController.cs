using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.PlayerMovement
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : NetworkBehaviour
    {
        private Transform playerTransform;
        private Transform cameraTransform;
        [SerializeField] private Camera playerCam;
    
        public InputActionAsset inputAsset;

        private Rigidbody rb;
        private InputAction moveAction;
        private InputAction jumpAction;
        
        [Header("Player settings")]
        [SerializeField] [Range(0.01f, 0.2f)] private float mouseSensitivity;
        [SerializeField] private float accelerationSpeed;
        
        [Header("Movement settings (Debug)")]
        [SerializeField] [Range(1.0f, 20.0f)] private float maxSpeed;
        [SerializeField] private float moveSpeed = 4.6f;
        public float moveMultiplier = 1.0f;
        [SerializeField] private float jumpHeight = 2f;
        [SerializeField] private float gravity = -9.81f;
        [SerializeField] private float accelerationTime = 0.1f; 
        [SerializeField] private float decelerationTime = 0.15f; 

        private Vector3 _currentVelocity; 
        private Vector3 _smoothedMoveDirection;
        private Vector3 jumpForwardVec;
        private Vector3 jumpRightVec;

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
            cameraTransform = playerCam.transform;
            playerTransform = GetComponent<Transform>();
            rb = GetComponent<Rigidbody>();

            moveAction = inputAsset.FindActionMap("Player").FindAction("Move");
            jumpAction = inputAsset.FindActionMap("Player").FindAction("Jump");
        }


        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                rb.isKinematic = false; 
                moveAction.Enable();
                jumpAction.Enable();
                jumpAction.performed += OnJump;
                
                playerCam.gameObject.SetActive(true);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                return;
            }
    
            rb.isKinematic = true; 
            playerCam.gameObject.SetActive(false);
            moveAction.Disable();
            jumpAction.Disable();
        }
        
        public override void OnNetworkDespawn()
        {
            if (IsOwner) moveAction.Disable();
        }

        private void OnJump(InputAction.CallbackContext ctx)
        {
            if (jumpRequested) return;
            jumpForwardVec = transform.forward;
            jumpRightVec = transform.right;
            jumpRequested = true;
        }

        private void RotateCamera()
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();
            float mouseX = mouseDelta.x * mouseSensitivity;
            float mouseY = mouseDelta.y * mouseSensitivity;
        
            verticalRotation -= mouseY;
            verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
            cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);

            playerTransform.Rotate(Vector3.up * mouseX);                
        }
        
        
        
        void Update()
        {
            if (!IsOwner) return;
            RotateCamera();
        }
      
        private void FixedUpdate()
        {
            if (!IsOwner) return;
            HandleGravityAndGround();
            Vector3 targetDirection;
            if (isGrounded)
            {
                if (moveInput.magnitude > 0.0f) { isMoving = true; }
                else { isMoving = false; }
                moveInput = moveAction.ReadValue<Vector2>();
                targetDirection = (transform.forward * moveInput.y + transform.right * moveInput.x)
                    .normalized * (moveSpeed * moveMultiplier);
            }
            else
            {
                isMoving = false;
                targetDirection = (jumpForwardVec * moveInput.y + jumpRightVec * moveInput.x)
                    .normalized * moveSpeed;
            }
            
            float smoothTime = moveInput.magnitude > 0.01f ? accelerationTime : decelerationTime;
            _smoothedMoveDirection = Vector3.SmoothDamp(
                _smoothedMoveDirection,
                targetDirection,
                ref _currentVelocity,
                smoothTime
            );

            rb.linearVelocity = new Vector3(_smoothedMoveDirection.x, verticalVelocity, _smoothedMoveDirection.z);

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
