using System;
using UnityEngine;

namespace BusanMath.Views
{
    public abstract class BaseView : MonoBehaviour
    {
        // ── 기본 설정 ──────────────────────────────────
        [Header("=== 기본 설정 ===")]
        [SerializeField] protected GameObject _rootPanel;   // 루트 패널 (Show/Hide 대상)
        [SerializeField] protected bool showOnAwake = false; // Awake 시 자동 표시 여부

        private bool _isInitialized = false;  // 초기화 완료 여부

        public bool IsVisible { get; private set; }  // 현재 표시 상태

        // ── 이벤트 ────────────────────────────────────
        public event Action OnShow;   // Show 호출 시 발생
        public event Action OnHide;   // Hide 호출 시 발생

        protected virtual void Awake()
        {
            if (_rootPanel == null)
            {
                _rootPanel = gameObject;
            }

            if (showOnAwake)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }

        protected virtual void Start()
        {
            EnsureInitialized();
        }

        /// <summary>
        /// 비활성화된 View도 초기화할 수 있도록 외부에서 호출 가능
        /// </summary>
        public void EnsureInitialized()
        {
            if (_isInitialized) return;
            _isInitialized = true;

            if (_rootPanel == null)
            {
                _rootPanel = gameObject;
            }

            Initialize();
            BindUIEvent();
        }

        protected virtual void Initialize() { }

        protected virtual void BindUIEvent() { }

        public virtual void Show()
        {
            if (_rootPanel != null)
            {
                _rootPanel.SetActive(true);
            }

            IsVisible = true;
            OnShow?.Invoke();
        }

        public virtual void Hide()
        {
            if (_rootPanel != null)
            {
                _rootPanel.SetActive(false);
            }

            IsVisible = false;
            OnHide?.Invoke();
        }

        public void Toggle()
        {
            if (IsVisible) Hide();
            else Show();
        }

        public virtual void ResetView() { }
    }
}
