using System;
using UnityEngine;
using UnityEngine.UI;

public class SelectView : BaseView
{
    [Header("=== Select View Settings ===")]
    public string _fileName;
    public RawImage _displayImage;
    public Image _titleImage;
    public Button _homeButton;
    public Image _skipImage;
    public Button _skipButton;
    public GameObject _buttonContainer;
    public Button _egyptButton;
    public Button _chinaButton;
    public Button _romaButton;
    public Slider _progressbar;

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
