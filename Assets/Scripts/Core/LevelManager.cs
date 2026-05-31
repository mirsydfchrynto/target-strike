using UnityEngine;
using TargetStrike.Core;
using TargetStrike.Targets;

namespace TargetStrike.Core
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance { get; private set; }

        [Header("Level Settings")]
        [SerializeField] private float levelTimeLimit = 60f;
        
        private int totalTargets;
        private int destroyedTargetsCount;
        private int currentScore;

        public int CurrentScore => currentScore;
        public int DestroyedTargetsCount => destroyedTargetsCount;
        public int TotalTargets => totalTargets;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        private void Start()
        {
            // Auto-count targets in the current scene
            totalTargets = FindObjectsByType<Target>(FindObjectsSortMode.None).Length;
            destroyedTargetsCount = 0;
            currentScore = 0;

            Target.OnTargetDestroyed += HandleTargetDestroyed;
            
            if (TimerManager.Instance != null)
            {
                TimerManager.Instance.StartTimer(levelTimeLimit);
                TimerManager.Instance.OnTimerExpired += HandleTimeExpired;
            }

            if (GameManager.Instance != null)
                GameManager.Instance.UpdateState(GameState.Playing);
        }

        private void OnDestroy()
        {
            Target.OnTargetDestroyed -= HandleTargetDestroyed;
            if (TimerManager.Instance != null)
                TimerManager.Instance.OnTimerExpired -= HandleTimeExpired;
        }

        private void HandleTargetDestroyed(int scoreValue)
        {
            destroyedTargetsCount++;
            currentScore += scoreValue;
            Debug.Log($"[LEVEL MANAGER] Target Destroyed! Score: {currentScore}, Remaining: {totalTargets - destroyedTargetsCount}");

            if (destroyedTargetsCount >= totalTargets && totalTargets > 0)
            {
                 Victory();
            }
        }

        private void HandleTimeExpired()
        {
            GameOver();
        }

        private void Victory()
        {
            if (TimerManager.Instance != null) TimerManager.Instance.StopTimer();
            if (GameManager.Instance != null) GameManager.Instance.UpdateState(GameState.Victory);
        }

        private void GameOver()
        {
            if (GameManager.Instance != null) GameManager.Instance.UpdateState(GameState.GameOver);
        }
    }
}
