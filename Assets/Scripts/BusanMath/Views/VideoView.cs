using System;
using UnityEngine;
using UnityEngine.UI;

public class VideoView : BaseView
{
    [Header("=== Video View Settings ===")]
    [SerializeField] public string _fileNameEgypt;
    [SerializeField] public string _fileNameChina;
    [SerializeField] public string _fileNameRoma;
    [SerializeField] public RawImage _displayImage;
    [SerializeField] public Button _homeButton;
    [SerializeField] public Image _skipImage;
    [SerializeField] public Button _skipButton;
    [SerializeField] public Slider _progressbar;

    public event Action _OnHomeButtonClicked;
    public event Action _OnSkipButtonClicked;

    protected override void Initialize()
    {
    }

    protected override void BindUIEvent()
    {
        _homeButton?.onClick.AddListener(() => _OnHomeButtonClicked?.Invoke());
        _skipButton?.onClick.AddListener(() => _OnSkipButtonClicked?.Invoke());
    }
}
