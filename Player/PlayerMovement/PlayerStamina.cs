using System;
using Player.PlayerMovement;

using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStamina : NetworkBehaviour
{
    private PlayerController _playerController;
    
    private int Stamina = 2000;
    private int StaminaMax = 2000;
    private int staminaDrain = 3;
    private int staminaRegen = 2;
    private InputActionAsset inputActionAsset;
    private InputAction shiftAction;
    public event Action<float> OnStaminaChange;
    private float StaminaNormalized;
    private bool isSprinting;
    private bool isExhausted;
    private bool isFull;
    public event Action OnSprintStart;
    public event Action OnSprintStop;
    
    
    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        inputActionAsset = _playerController.inputAsset;
        shiftAction = inputActionAsset.FindActionMap("Player").FindAction("Sprint");
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) { return; }
        shiftAction.Enable();
        shiftAction.started += StartSprinting;
        shiftAction.canceled += StopSprinting;
    }

    private void StartSprinting(InputAction.CallbackContext obj)
    {
        if (!isExhausted && _playerController.isMoving)
        {
            isFull = false;
            OnSprintStart?.Invoke();

            _playerController.moveMultiplier = 1.5f;
            isSprinting = true;    
        } 
    }

    private void StopSprinting(InputAction.CallbackContext obj)
    {
        _playerController.moveMultiplier = 1.0f;
        isSprinting = false;
    }

    private void Update()
    {
        if (isSprinting && !isExhausted)
        {
            Stamina -= Mathf.RoundToInt(staminaDrain * 100f * Time.deltaTime);
            
            if (Stamina <= 0)
            {
                Stamina = 0;
                isExhausted = true;
                isSprinting = false;
                _playerController.moveMultiplier = 1f;
            }

            if (_playerController.isMoving == false) 
            {
                isSprinting = false;
                _playerController.moveMultiplier = 1f;
            }
        }
        else
        {
            Stamina += Mathf.RoundToInt(staminaRegen * 100f * Time.deltaTime);

            if (Stamina >= StaminaMax)
            {
                if (!isFull)
                {
                    OnSprintStop?.Invoke();
                }
                Stamina = StaminaMax;
                isFull = true;
            }

            if (isExhausted && Stamina >= 900)
            {
                isExhausted = false;
            }
                
        }
        OnStaminaChange?.Invoke((float)Stamina / StaminaMax);
    }
}
