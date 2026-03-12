using System;
using UnityEngine;

public abstract class BaseView : MonoBehaviour
{
    [Header("=== Base View Settings ===")]
    [SerializeField] protected GameObject _rootPanel;
    [SerializeField] protected bool showOnAwake = false;

    // 초기화 여부 플래그
    private bool _isInitialized = false;

    public bool IsVisible { get; private set; }

    public event Action OnShow;
    public event Action OnHide;

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
    /// GameObject가 비활성화 상태여도 안전하게 Initialize/BindUIEvent 실행
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
        if (IsVisible)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    public virtual void ResetView() { }

}
