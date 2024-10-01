using RMC.MyProject.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace RMC.MyProject.Scenes
{
    /// <summary>
    /// Main entry point for the Scene.
    /// </summary>
    public class Scene01_Intro : MonoBehaviour
    {
        //  Properties ------------------------------------
        public HudUI HudUI { get { return _hudUI; } }


        //  Fields ----------------------------------------
        [Header("UI")]
        [SerializeField]
        private HudUI _hudUI;

        [Header("Game")]
        [SerializeField]
        private Player _player;

        // Input
        private InputAction _moveInputAction;
        private InputAction _jumpInputAction;
        private InputAction _resetInputAction;
        
        // UI
        private FloatField _moveSpeedField;
        private FloatField _jumpSpeedField;
        
        //  Unity Methods ---------------------------------
        protected void Start()
        {
            Debug.Log($"{GetType().Name}.Start()");
            
            // Input
            _moveInputAction = InputSystem.actions.FindAction("Move");
            _jumpInputAction = InputSystem.actions.FindAction("Jump");
            _resetInputAction = InputSystem.actions.FindAction("Reset");
            
            // UI
            HudUI.SetScore("Score: 000");
            HudUI.SetLives("Lives: 003");
            HudUI.SetInstructions("Instructions: Arrows, Spacebar, R");
            HudUI.SetTitle(SceneManager.GetActiveScene().name);
            
            // Bind PlayerData
            BindPlayerData();

        }


        protected void Update()
        {
            HandleUserInput();
            CheckPlayerFalling();

        }



        //  Methods ---------------------------------------
        private void BindPlayerData()
        {
            _moveSpeedField = HudUI.PlayerDataLayout.Q<FloatField>("MoveSpeed");
            _jumpSpeedField = HudUI.PlayerDataLayout.Q<FloatField>("JumpSpeed");

            _moveSpeedField.value = _player.PlayerData.MoveSpeed;
            _jumpSpeedField.value = _player.PlayerData.JumpSpeed;
            
            // Check for changes FROM THE RUNTIME
            _moveSpeedField.RegisterValueChangedCallback(evt =>
            {
                _player.PlayerData.MoveSpeed = evt.newValue;
            });

            _jumpSpeedField.RegisterValueChangedCallback(evt =>
            {
                _player.PlayerData.JumpSpeed = evt.newValue;
            });
        }
        

        private void HandleUserInput()
        {
            Vector2 moveInputVector2 = _moveInputAction.ReadValue<Vector2>();
            
            if (moveInputVector2.magnitude > 0.1f)
            {
                Vector3 moveInputVector3 = new Vector3
                (
                    moveInputVector2.x,
                    0,
                    moveInputVector2.y
                );
                
                // Move with arrow keys / WASD / gamepad
                _player.Rigidbody.AddForce(moveInputVector3 * _player.PlayerData.MoveSpeed, ForceMode.Acceleration);
            }

            if (_jumpInputAction.WasPerformedThisFrame())
            {
                // Jump with spacebar / gamepad
                _player.Rigidbody.AddForce(Vector3.up * _player.PlayerData.JumpSpeed, ForceMode.Impulse);
            }
            
            if (_resetInputAction.IsPressed())
            {
                // Reload the current scene with R key 
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }

            // Check for changes FROM THE EDITOR
            if (!Mathf.Approximately(_moveSpeedField.value, _player.PlayerData.MoveSpeed))
            {
                _moveSpeedField.value = _player.PlayerData.MoveSpeed;
            }
            
            if (!Mathf.Approximately(_jumpSpeedField.value, _player.PlayerData.JumpSpeed))
            {
                _jumpSpeedField.value = _player.PlayerData.JumpSpeed;
            }
        }

        
        private void CheckPlayerFalling()
        {
            if (_player.transform.position.y < -5)
            {
                // Reload the current scene if character falls off Floor
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        //  Event Handlers --------------------------------
    }
}