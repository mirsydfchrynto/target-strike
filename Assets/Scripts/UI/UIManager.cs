using UnityEngine;
using TMPro;
using TargetStrike.Core;
using TargetStrike.Player;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace TargetStrike.UI
{
    public class UIManager : MonoBehaviour
    {
        [Header("HUD Elements")]
        public TextMeshProUGUI scoreText;
        public TextMeshProUGUI timerText;
        public TextMeshProUGUI ammoText;
        public TextMeshProUGUI targetCountText;
        public GameObject hitMarker;

        [Header("Screens")]
        public GameObject winScreen;
        public GameObject loseScreen;
        public GameObject pauseScreen;

        [Header("Action Buttons")]
        public Button nextLevelButton;
        public Button restartButtonWin;
        public Button restartButtonLose;
        public Button menuButtonWin;
        public Button menuButtonLose;

        private PlayerAmmo playerAmmo;
        private float hitMarkerDuration = 0.1f;
        private float hitMarkerTimer;

        private void Start()
        {
            playerAmmo = FindFirstObjectByType<PlayerAmmo>();
            if (playerAmmo != null) playerAmmo.OnAmmoChanged += UpdateAmmoUI;

            PlayerShooting playerShooting = FindFirstObjectByType<PlayerShooting>();
            if (playerShooting != null) playerShooting.OnHitTarget += ShowHitMarker;

            // Bind Screen Buttons programmatically
            SetupButtons();
            
            UpdateAmmoUI();
            if (hitMarker != null) hitMarker.SetActive(false);
        }

        private void SetupButtons()
        {
            if (nextLevelButton != null) nextLevelButton.onClick.AddListener(LoadNextLevel);
            if (restartButtonWin != null) restartButtonWin.onClick.AddListener(RestartLevel);
            if (restartButtonLose != null) restartButtonLose.onClick.AddListener(RestartLevel);
            if (menuButtonWin != null) menuButtonWin.onClick.AddListener(LoadMainMenu);
            if (menuButtonLose != null) menuButtonLose.onClick.AddListener(LoadMainMenu);
        }

        private void Update()
        {
            UpdateHUD();
            HandleScreenVisibility();
            HandleHitMarker();
        }

        private void HandleHitMarker()
        {
            if (hitMarkerTimer > 0)
            {
                hitMarkerTimer -= Time.deltaTime;
                if (hitMarkerTimer <= 0 && hitMarker != null)
                {
                    hitMarker.SetActive(false);
                }
            }
        }

        public void ShowHitMarker()
        {
            if (hitMarker != null)
            {
                hitMarker.SetActive(true);
                hitMarkerTimer = hitMarkerDuration;
            }
        }

        private void UpdateHUD()
        {
            if (LevelManager.Instance != null)
            {
                if (scoreText != null) scoreText.text = "SCORE: " + LevelManager.Instance.CurrentScore;
                if (targetCountText != null) targetCountText.text = "TARGETS: " + LevelManager.Instance.DestroyedTargetsCount + "/" + LevelManager.Instance.TotalTargets;
            }

            if (TimerManager.Instance != null && timerText != null)
            {
                timerText.text = TimerManager.Instance.GetFormattedTime();
            }
        }

        private void UpdateAmmoUI()
        {
            if (playerAmmo != null && ammoText != null)
            {
                ammoText.text = playerAmmo.CurrentMagazineAmmo + " / " + playerAmmo.TotalReserveAmmo;
            }
        }

        private void HandleScreenVisibility()
        {
            if (GameManager.Instance == null) return;

            var state = GameManager.Instance.CurrentState;
            if (winScreen != null) winScreen.SetActive(state == GameState.Victory);
            if (loseScreen != null) loseScreen.SetActive(state == GameState.GameOver);
            if (pauseScreen != null) pauseScreen.SetActive(state == GameState.Paused);
        }

        public void RestartLevel()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void LoadNextLevel()
        {
            Time.timeScale = 1f;
            string currentScene = SceneManager.GetActiveScene().name;
            
            // Progression logic based on GDD scene names in 'uts'
            if (currentScene.Contains("lvl 1")) SceneManager.LoadScene("lvl 2");
            else if (currentScene.Contains("lvl 2")) SceneManager.LoadScene("lvl 3");
            else if (currentScene.Contains("Level_01")) SceneManager.LoadScene("Level_02");
            else if (currentScene.Contains("Level_02")) SceneManager.LoadScene("Level_03");
            else LoadMainMenu();
        }

        public void LoadMainMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("MainMenu");
        }
    }
}
