using UnityEngine;
using UnityEngine.InputSystem;

namespace TargetStrike.Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        [Header("Input Action Asset")]
        [SerializeField] private InputActionAsset inputAsset;

        [Header("Action Map Names")]
        [SerializeField] private string playerMapName = "Player";

        private InputAction moveAction;
        private InputAction lookAction;
        private InputAction jumpAction;
        private InputAction sprintAction;
        private InputAction fireAction;
        private InputAction reloadAction;
        private InputAction pauseAction;

        public Vector2 MoveInput { get; private set; }
        public Vector2 LookInput { get; private set; }
        public bool JumpTriggered { get; private set; }
        public bool SprintValue { get; private set; }
        public bool FireTriggered { get; private set; }
        public bool ReloadTriggered { get; private set; }
        public bool PauseTriggered { get; private set; }

        private void Awake()
        {
            if (inputAsset == null)
            {
                Debug.LogError("Input Action Asset is missing on " + gameObject.name);
                enabled = false;
                return;
            }

            var playerMap = inputAsset.FindActionMap(playerMapName);
            if (playerMap == null)
            {
                Debug.LogError("Action Map '" + playerMapName + "' not found in " + inputAsset.name);
                enabled = false;
                return;
            }
            
            moveAction = playerMap.FindAction("Move");
            lookAction = playerMap.FindAction("Look");
            jumpAction = playerMap.FindAction("Jump");
            sprintAction = playerMap.FindAction("Sprint");
            fireAction = playerMap.FindAction("Fire");
            reloadAction = playerMap.FindAction("Reload");
            pauseAction = playerMap.FindAction("Pause");
        }

        private void OnEnable()
        {
            if (inputAsset != null) inputAsset.Enable();
        }

        private void OnDisable()
        {
            if (inputAsset != null) inputAsset.Disable();
        }

        private void Update()
        {
            if (inputAsset == null) return;

            MoveInput = moveAction != null ? moveAction.ReadValue<Vector2>() : Vector2.zero;
            LookInput = lookAction != null ? lookAction.ReadValue<Vector2>() : Vector2.zero;
            
            JumpTriggered = jumpAction != null && jumpAction.WasPressedThisFrame();
            SprintValue = sprintAction != null && sprintAction.IsPressed();
            FireTriggered = fireAction != null && fireAction.WasPressedThisFrame();
            ReloadTriggered = reloadAction != null && reloadAction.WasPressedThisFrame();
            PauseTriggered = pauseAction != null && pauseAction.WasPressedThisFrame();
        }
    }
}
