using System;
using UnityEngine;
using UnityEngine.UI;

public class VoteView : BaseView
{
    [Header("=== Vote View Settings ===")]
    [SerializeField] public Image _backgroundImage;
    [SerializeField] public Button _homeButton;
    [SerializeField] public Image _titleImage;
    [SerializeField] public Button _egyptButton;
    [SerializeField] public Button _chinaButton;
    [SerializeField] public Button _romaButton;

    public event Action _OnHomeButtonClicked;
    public event Action _OnEgyptButtonClicked;
    public event Action _OnChinaButtonClicked;
    public event Action _OnRomaButtonClicked;

    protected override void Initialize()
    {
    }

    protected override void BindUIEvent()
    {
        _homeButton.onClick.AddListener(() => _OnHomeButtonClicked?.Invoke());
        _egyptButton.onClick.AddListener(() => _OnEgyptButtonClicked?.Invoke());
        _chinaButton.onClick.AddListener(() => _OnChinaButtonClicked?.Invoke());
        _romaButton.onClick.AddListener(() => _OnRomaButtonClicked?.Invoke());
    }
}
