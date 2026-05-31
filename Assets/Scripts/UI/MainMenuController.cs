using UnityEngine;
using TargetStrike.Core;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TargetStrike.UI
{
    public class MainMenuController : MonoBehaviour
    {
        [Header("Main Panel")]
        [SerializeField] private GameObject mainPanel;
        [SerializeField] private Button startButton;
        [SerializeField] private Button levelSelectButton;
        [SerializeField] private Button exitButton;

        [Header("Level Select Panel")]
        [SerializeField] private GameObject levelSelectPanel;
        [SerializeField] private Button backButton;
        [SerializeField] private Button lvl1Button;
        [SerializeField] private Button lvl2Button;
        [SerializeField] private Button lvl3Button;

        private void Start()
        {
            // Auto-bind buttons if they exist
            if (startButton != null) startButton.onClick.AddListener(StartGame);
            if (levelSelectButton != null) levelSelectButton.onClick.AddListener(ShowLevelSelect);
            if (exitButton != null) exitButton.onClick.AddListener(ExitGame);
            
            if (backButton != null) backButton.onClick.AddListener(ShowMainPanel);
            if (lvl1Button != null) lvl1Button.onClick.AddListener(() => LoadLevel("lvl 1"));
            if (lvl2Button != null) lvl2Button.onClick.AddListener(() => LoadLevel("lvl 2"));
            if (lvl3Button != null) lvl3Button.onClick.AddListener(() => LoadLevel("lvl 3"));

            ShowMainPanel();

            if (GameManager.Instance != null)
                GameManager.Instance.UpdateState(GameState.MainMenu);
        }

        public void StartGame()
        {
            Debug.Log("[MAIN MENU] Starting Level 1...");
            LoadLevel("lvl 1");
        }

        public void ShowLevelSelect()
        {
            if (mainPanel != null) mainPanel.SetActive(false);
            if (levelSelectPanel != null) levelSelectPanel.SetActive(true);
        }

        public void ShowMainPanel()
        {
            if (mainPanel != null) mainPanel.SetActive(true);
            if (levelSelectPanel != null) levelSelectPanel.SetActive(false);
        }

        private void LoadLevel(string sceneName)
        {
            // Try 'lvl X' first, then 'Level_0X'
            try {
                SceneManager.LoadScene(sceneName);
            } catch {
                string alternative = sceneName.Replace("lvl ", "Level_0");
                SceneManager.LoadScene(alternative);
            }
        }

        public void ExitGame()
        {
            Debug.Log("[MAIN MENU] Exiting...");
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}
