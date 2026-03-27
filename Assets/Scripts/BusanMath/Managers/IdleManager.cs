using BusanMath.Core;
using BusanMath.Controllers;
using System;
using UnityEngine;

namespace BusanMath.Managers
{
    /// <summary>
    /// 사용자 입력이 없을 때 타임아웃을 감지하여 홈 화면으로 복귀
    /// </summary>
    public class IdleManager : MonoSingleton<IdleManager>
    {
        [Header("=== Idle Settings ===")]
        [SerializeField] private float idleTimeout = 60f;
        [SerializeField] private bool isEnabled = true;
        [SerializeField] private bool showDebugLog = true;

        private float idleTimer;
        private bool isPaused;

        public event Action OnIdleTimeout;
        public event Action OnIdleReset;

        public float IdleTime => idleTimer;
        public float RemainingTime => Mathf.Max(0, idleTimeout - idleTimer);
        public bool IsIdle => idleTimer >= idleTimeout;
        public bool IsEnabled => isEnabled;

        protected override void OnSingletonAwake()
        {
            ResetTimer();
        }

        private void Start()
        {
            OnIdleTimeout += () =>
            {
                if (NavigationController.Instance.IsHomeState()
                    || NavigationController.Instance.IsSelectState()) return;
                NavigationController.Instance.GoToHome();
            };
        }

        private void Update()
        {
            if (!isEnabled || isPaused) return;

            if (HasAnyInput())
            {
                ResetTimer();
                return;
            }

            idleTimer += Time.deltaTime;
            CheckTimeout();
        }

        private bool HasAnyInput()
        {
            if (Mathf.Abs(Input.GetAxis("Mouse X")) > 0.01f ||
                Mathf.Abs(Input.GetAxis("Mouse Y")) > 0.01f)
            {
                return true;
            }

            if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2))
            {
                return true;
            }

            if (Input.touchCount > 0)
            {
                return true;
            }

            if (Input.anyKeyDown)
            {
                return true;
            }

            return false;
        }

        private void CheckTimeout()
        {
            if (idleTimer >= idleTimeout)
            {
                Log("Idle timeout triggered!");
                OnIdleTimeout?.Invoke();
                ResetTimer();
            }
        }

        #region Public API

        public void ResetTimer()
        {
            bool wasActive = idleTimer > 0;
            idleTimer = 0f;

            if (wasActive)
            {
                OnIdleReset?.Invoke();
                Log("Timer reset");
            }
        }

        public void SetTimeout(float seconds)
        {
            idleTimeout = Mathf.Max(1f, seconds);
            Log($"Timeout set to {idleTimeout}s");
        }

        public void SetEnabled(bool enabled)
        {
            isEnabled = enabled;

            if (enabled)
            {
                ResetTimer();
            }

            Log($"IdleManager {(enabled ? "enabled" : "disabled")}");
        }

        public void Pause()
        {
            isPaused = true;
            Log("Paused");
        }

        public void Resume()
        {
            isPaused = false;
            ResetTimer();
            Log("Resumed");
        }

        #endregion

        private void Log(string message)
        {
            if (showDebugLog)
            {
                Debug.Log($"[IdleManager] {message}");
            }
        }
    }
}
