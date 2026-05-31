using UnityEngine;

namespace TargetStrike.Core
{
    public enum GameState
    {
        MainMenu,
        Playing,
        Paused,
        GameOver,
        Victory
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public GameState CurrentState { get; private set; } = GameState.MainMenu;

        private void Awake()
        {
            if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
            else { Destroy(gameObject); }
        }

        private void Start()
        {
            UpdateState(GameState.MainMenu);
        }

        public void UpdateState(GameState newState)
        {
            CurrentState = newState;
            Debug.Log($"[GAME MANAGER] State Updated to: {newState}");

            switch (newState)
            {
                case GameState.MainMenu:
                    Time.timeScale = 1f;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    break;
                case GameState.Playing:
                    Time.timeScale = 1f;
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    break;
                case GameState.Paused:
                case GameState.GameOver:
                case GameState.Victory:
                    Time.timeScale = 0f;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    break;
            }
        }
    }
}
