using System;
using System.Text;
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

        private readonly StringBuilder _sb = new StringBuilder(512);
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
            _sb.Clear();
            _sb.AppendLine("Debug Menu [F1]\n");
            _sb.AppendLine("Enemy:");
            _sb.Append("Distance to ply: ").AppendFormat("{0:F2}\n", ENEMY_DIST_TO_PLAYER);
            _sb.Append("Movespeed: ").AppendFormat("{0:F1}\n", ENEMY_SPEED);
            _sb.Append("Idle Timer: ").AppendFormat("{0:F1}\n", ENEMY_IDLE_TIME);
            _sb.Append("Current state: ").AppendLine(ENEMY_STATE.ToString());
            _sb.AppendLine("\nPlayer:");
            _sb.Append("Speed: ").AppendFormat("{0:F1}\n", MOVESPEED);
            _sb.Append("Stamina: ").AppendFormat("{0:F1}\n", SPRINT_VAL);
            _sb.Append("In Crouch?: ").AppendLine(IN_CROUCH.ToString());
            _sb.Append("Is Sprint?: ").AppendLine(IS_SPRINTING.ToString());

            mainText.SetText(_sb);
        }  
      
        
    }
}