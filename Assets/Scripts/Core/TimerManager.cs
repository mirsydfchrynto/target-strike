using UnityEngine;
using System;

namespace TargetStrike.Core
{
    public class TimerManager : MonoBehaviour
    {
        public static TimerManager Instance { get; private set; }
        
        public event Action OnTimerExpired;
        private float currentTime;
        private bool isRunning;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void StartTimer(float duration)
        {
            currentTime = duration;
            isRunning = true;
        }

        public void StopTimer()
        {
            isRunning = false;
        }

        private void Update()
        {
            if (!isRunning) return;

            if (currentTime > 0)
            {
                currentTime -= Time.deltaTime;
            }
            else
            {
                currentTime = 0;
                isRunning = false;
                OnTimerExpired?.Invoke();
            }
        }

        public string GetFormattedTime()
        {
            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);
            return string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        
        public float GetRemainingTime() => currentTime;
    }
}
