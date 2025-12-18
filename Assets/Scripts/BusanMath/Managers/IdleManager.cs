using BusanMath.Core;
using System;
using UnityEngine;

public class IdleManager : MonoSingleton<IdleManager>
{
    [Header("=== Idle Settings ===")]
    [SerializeField] private float idleTimeout = 60f;
    [SerializeField] private bool isEnabled = true;
    [SerializeField] private bool showDebugLog = true;

    // 타이머 상태
    private float idleTimer;
    private bool isPaused;

    // 이벤트
    public event Action OnIdleTimeout;          // 타임아웃 발생
    public event Action OnIdleReset;            // 타이머 리셋됨

    // 프로퍼티
    public float IdleTime => idleTimer;
    public float RemainingTime => Mathf.Max(0, idleTimeout - idleTimer);
    public bool IsIdle => idleTimer >= idleTimeout;
    public bool IsEnabled => isEnabled;


    /// <summary>
    /// 싱글톤 초기화 시 호출되는 가상 메서드
    /// 서브클래스에서 오버라이드하여 초기화 로직 구현 
    /// </summary>
    protected override void OnSingletonAwake()
    {
        ResetTimer();
    }

    private void Start()
    {
        OnIdleTimeout += () => NavigationController.Instance.GoToHome();
    }

    private void Update()
    {
        if (!isEnabled || isPaused) return;

        // 입력 감지
        if (HasAnyInput())
        {
            ResetTimer();
            return;
        }

        // 타이머 증가
        idleTimer += Time.deltaTime;

        // 타임아웃 체크
        CheckTimeout();
    }

    /// <summary>
    /// 모든 종류의 입력 감지
    /// </summary>
    private bool HasAnyInput()
    {
        // 마우스 이동
        if (Mathf.Abs(Input.GetAxis("Mouse X")) > 0.01f ||
            Mathf.Abs(Input.GetAxis("Mouse Y")) > 0.01f)
        {
            return true;
        }

        // 마우스 클릭
        if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2))
        {
            return true;
        }

        // 터치 입력
        if (Input.touchCount > 0)
        {
            return true;
        }

        // 키보드 입력
        if (Input.anyKeyDown)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// 타임아웃 체크
    /// </summary>
    private void CheckTimeout()
    {
        if (idleTimer >= idleTimeout)
        {
            Log("Idle timeout triggered!");
            OnIdleTimeout?.Invoke();

            // 타임아웃 후 자동 리셋 (연속 발생 방지)
            ResetTimer();
        }
    }

    // === Public 메서드 ===

    /// <summary>
    /// 타이머 리셋 (사용자 액션 시 호출)
    /// </summary>
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

    /// <summary>
    /// 타임아웃 시간 설정
    /// </summary>
    public void SetTimeout(float seconds)
    {
        idleTimeout = Mathf.Max(1f, seconds);
        Log($"Timeout set to {idleTimeout}s");
    }

    /// <summary>
    /// 활성화/비활성화
    /// </summary>
    public void SetEnabled(bool enabled)
    {
        isEnabled = enabled;

        if (enabled)
        {
            ResetTimer();
        }

        Log($"IdleManager {(enabled ? "enabled" : "disabled")}");
    }

    /// <summary>
    /// 일시정지 (팝업 등에서 사용)
    /// </summary>
    public void Pause()
    {
        isPaused = true;
        Log("Paused");
    }

    /// <summary>
    /// 재개
    /// </summary>
    public void Resume()
    {
        isPaused = false;
        ResetTimer(); // 재개 시 리셋
        Log("Resumed");
    }

    private void Log(string message)
    {
        if (showDebugLog)
        {
            Debug.Log($"[IdleManager] {message}");
        }
    }
}