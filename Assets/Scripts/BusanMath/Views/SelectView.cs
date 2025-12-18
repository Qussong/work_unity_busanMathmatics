using System;
using UnityEngine;
using UnityEngine.UI;

public class SelectView : BaseView
{
    [Header("=== Select View Settings ===")]
    [SerializeField] public string _fileName;
    [SerializeField] public RawImage _displayImage;
    [SerializeField] public Image _titleImage;
    [SerializeField] public Button _homeButton;
    [SerializeField] public Image _skipImage;
    [SerializeField] public Button _skipButton;
    [SerializeField] public Button _egyptButton;
    [SerializeField] public Button _chinaButton;
    [SerializeField] public Button _romaButton;
    [SerializeField] public Slider _progressbar;

    public event Action _OnHomeButtonClicked;
    public event Action _OnSkipButtonClicked;
    public event Action _OnEgyptButtonClicked;
    public event Action _OnChinaButtonClicked;
    public event Action _OnRomaButtonClicked;

    protected override void Initialize()
    {

    }

    protected override void BindUIEvent()
    {
        _homeButton?.onClick.AddListener(() => _OnHomeButtonClicked?.Invoke());
        _skipButton?.onClick.AddListener(() => _OnSkipButtonClicked?.Invoke());
        _egyptButton?.onClick.AddListener(() => _OnEgyptButtonClicked?.Invoke());
        _chinaButton?.onClick.AddListener(() => _OnChinaButtonClicked?.Invoke());
        _romaButton?.onClick.AddListener(() => _OnRomaButtonClicked?.Invoke());
    }
}
