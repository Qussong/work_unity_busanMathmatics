using System;
using UnityEngine;

public abstract class BaseView : MonoBehaviour
{
    [Header("=== Base View Settings ===")]
    [SerializeField] protected GameObject _rootPanel;    // View의 루트 패널 (활성화/비활성화 대상)
    [SerializeField] protected bool showOnAwake = false;// Awake 시 자동 표시 여부

    // 현재 View의 표시 상태
    public bool IsVisible { get; private set; }

    // View가 표시될 때 발생하는 이벤트
    public event Action OnShow;
    // View가 숨겨질 때 발생하는 이벤트
    public event Action OnHide;

    protected virtual void Awake()
    {
        // rootPanel 자동 할당 (미설정 시)
        if (_rootPanel == null)
        {
            _rootPanel = gameObject;
        }

        // 초기 상태 설정
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
        Initialize();
        BindUIEvent();
    }

    /// <summary>
    /// 초기화 (자식 클래스에서 재정의)
    /// </summary>
    protected virtual void Initialize() { }

    /// <summary>
    /// UI 동작관련 리스너 등록 (자식 클래스에서 재정의)
    /// </summary>
    protected virtual void BindUIEvent() { }

    /// <summary>
    /// 화면 표시
    /// </summary>
    public virtual void Show()
    {
        if (_rootPanel != null)
        {
            _rootPanel.SetActive(true);
        }

        IsVisible = true;
        OnShow?.Invoke();
    }

    /// <summary>
    /// 화면 숨김
    /// </summary>
    public virtual void Hide()
    {
        if (_rootPanel != null)
        {
            _rootPanel.SetActive(false);
        }

        IsVisible = false;
        OnHide?.Invoke();
    }

    /// <summary>
    /// 표시 상태 토글 (On/Off)
    /// </summary>
    public void Toggle()
    {
        if (IsVisible)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    /// <summary>
    /// UI 초기화 (데이터 클리어)
    /// </summary>
    public virtual void ResetView() { }

}