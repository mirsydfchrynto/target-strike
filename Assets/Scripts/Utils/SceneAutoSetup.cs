using UnityEngine;
using UnityEngine.InputSystem;
using TargetStrike.Player;
using TargetStrike.Core;
using TargetStrike.UI;

namespace TargetStrike.Utils
{
    public class SceneAutoSetup : MonoBehaviour
    {
        [Header("Core Prefabs")]
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private GameObject uiPrefab;
        [SerializeField] private InputActionAsset inputActions;
        
        [Header("Level Settings")]
        [SerializeField] private bool isLevel3 = false;

        private void Awake()
        {
            SetupManagers();
            SetupUI();
            SetupPlayer();
        }

        private void SetupManagers()
        {
            if (GameManager.Instance == null)
            {
                GameObject gm = new GameObject("GameManager");
                gm.AddComponent<GameManager>();
            }
            if (TimerManager.Instance == null)
            {
                GameObject tm = new GameObject("TimerManager");
                tm.AddComponent<TimerManager>();
            }
            if (LevelManager.Instance == null)
            {
                GameObject lm = new GameObject("LevelManager");
                lm.AddComponent<LevelManager>();
            }
        }

        private void SetupUI()
        {
            UIManager ui = Object.FindFirstObjectByType<UIManager>();
            if (ui == null && uiPrefab != null)
            {
                GameObject canvas = Instantiate(uiPrefab);
                canvas.name = "MainCanvas_Auto";
                ui = canvas.GetComponentInChildren<UIManager>();
            }

            if (ui == null)
            {
                Debug.LogWarning("[AUTO SETUP] UIManager/UI Prefab missing! HUD will not function.");
            }
        }

        private void SetupPlayer()
        {
            GameObject player = GameObject.FindWithTag("Player");
            
            if (player == null && playerPrefab != null)
            {
                player = Instantiate(playerPrefab, Vector3.up, Quaternion.identity);
                player.tag = "Player";
                player.name = "Player_Auto";
            }

            if (player != null)
            {
                // 1. Input System
                var playerInput = player.GetComponent<PlayerInput>();
                if (playerInput == null) playerInput = player.AddComponent<PlayerInput>();
                if (inputActions != null) playerInput.actions = inputActions;

                var inputHandler = player.GetComponent<PlayerInputHandler>();
                if (inputHandler == null) inputHandler = player.AddComponent<PlayerInputHandler>();

                // 2. Camera Setup
                Camera cam = player.GetComponentInChildren<Camera>();
                if (cam == null)
                {
                    GameObject camObj = new GameObject("FPS Camera");
                    camObj.transform.SetParent(player.transform);
                    camObj.transform.localPosition = new Vector3(0, 1.6f, 0);
                    cam = camObj.AddComponent<Camera>();
                    camObj.AddComponent<AudioListener>();
                    cam.tag = "MainCamera";
                }

                if (cam.gameObject.GetComponent<PlayerCamera>() == null)
                    cam.gameObject.AddComponent<PlayerCamera>();

                // 3. Movement
                var move = player.GetComponent<PlayerMovement>();
                if (move == null) move = player.AddComponent<PlayerMovement>();
                if (isLevel3) move.ConfigureAutoMovement(true, 4f, 8f);

                // 4. Weapons & Shooting
                var shooting = player.GetComponent<PlayerShooting>();
                if (shooting == null) shooting = player.AddComponent<PlayerShooting>();
                
                if (player.GetComponent<PlayerAmmo>() == null) player.AddComponent<PlayerAmmo>();

                // Ensure Weapon View is setup
                var weaponSetup = player.GetComponent<WeaponSetup>();
                if (weaponSetup == null) weaponSetup = player.AddComponent<WeaponSetup>();
                
                // Find or Create Weapon Container under Camera
                Transform weaponContainer = cam.transform.Find("WeaponContainer");
                if (weaponContainer == null)
                {
                    GameObject container = new GameObject("WeaponContainer");
                    container.transform.SetParent(cam.transform);
                    container.transform.localPosition = new Vector3(0.2f, -0.3f, 0.4f);
                    container.transform.localRotation = Quaternion.identity;
                    weaponContainer = container.transform;
                }

                weaponSetup.Setup(shooting);

                Debug.Log($"[AUTO SETUP] SUCCESS: Level {(isLevel3 ? "3" : "1/2")} is now fully playable.");
            }
        }
    }
}
