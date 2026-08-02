using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class UIDebug : MonoBehaviour
    {
        public static UIDebug Instance;

        [SerializeField] private TMP_Text mainText;
        [SerializeField] private CanvasGroup DebugCanvas;
        public InputActionAsset inputAsset;
        private InputAction debugToggle;
        
        //Enemy
        public float ENEMY_DIST_TO_PLAYER;
        public float ENEMY_SPEED;
        public float ENEMY_IDLE_TIME;
        public string ENEMY_STATE;
        
        //Player
        public float MOVESPEED;
        public string IN_CROUCH;
        public float PING;
        public string IS_SPRINTING;
        public float SPRINT_VAL;
        
        private void Awake()
        {
            if (Instance != null) { Destroy(gameObject); return;}
            debugToggle = inputAsset.FindActionMap("Player").FindAction("Debug");
            Instance = this;
            DontDestroyOnLoad(gameObject);

            debugToggle.started += ToggleUI;
        }

        private void Start()
        {
             DebugCanvas.alpha = 0.0f;
        }

        private void ToggleUI(InputAction.CallbackContext context)
        {
            if (DebugCanvas.alpha < 1.0)
            {
                DebugCanvas.DOFade(1.0f, 0.1f);
            }
            else
            {
                DebugCanvas.DOFade(0.0f, 0.1f);
            }
        }
        
        private void Update()
        {
            if (DebugCanvas.alpha == 0.0f)
            {
                return;
            }
            mainText.text = $"Debug Menu [F1]\n\nEnemy:\nDistance to ply: {ENEMY_DIST_TO_PLAYER.ToString("F")}\nMovespeed: {ENEMY_SPEED.ToString("F1")}\nIdle Timer: {ENEMY_IDLE_TIME.ToString("F1")}\nCurrent state: {ENEMY_STATE}\n\nPlayer:\nSpeed: {MOVESPEED.ToString("F1")}\nStamina: {SPRINT_VAL.ToString("F1")}\nIn Crouch?: {IN_CROUCH}\nIs Sprint?: {IS_SPRINTING}";
        }  
      
        
    }
}