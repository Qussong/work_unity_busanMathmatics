using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardGameDescriptionView : BaseView
{
    [Header("=== CardGameDescription View Settings ===")]
    [SerializeField] public Image _backgroundImage;
    [SerializeField] public Button _homeButton;
    [SerializeField] public Image _titleImage;
    [SerializeField] public Button _prevButton;
    [SerializeField] public Button _nextButton;
    [SerializeField] public Image _exampleViewBackgroundImage;
    [SerializeField] public Image _exampleViewBoundaryImage;
    [SerializeField] public GameObject _swipeUIObj;
    [SerializeField] public List<TMP_Text> _descriptionTextList = new List<TMP_Text>();
    [SerializeField] public Button _startButton;

    public event Action _OnHomeButtonClicked;
    public event Action _OnPrevButtonClicked;
    public event Action _OnNextButtonClicked;
    public event Action _OnStartButtonClicked;

    protected override void Initialize()
    {

    }

    protected override void BindUIEvent()
    {
        _homeButton?.onClick.AddListener(() => _OnHomeButtonClicked?.Invoke());
        _prevButton?.onClick.AddListener(() => _OnPrevButtonClicked?.Invoke());
        _nextButton?.onClick.AddListener(() => _OnNextButtonClicked?.Invoke());
        _startButton?.onClick.AddListener(() => _OnStartButtonClicked?.Invoke());
    }
}
