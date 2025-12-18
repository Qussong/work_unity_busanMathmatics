using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HomeView : BaseView
{
    [Header("=== Home View Settings ===")]
    [SerializeField] public Image _backgroundImage;
    [SerializeField] public Image _titleImage;
    [SerializeField] public TMP_Text _subTitleText;
    [SerializeField] public Button _leftButton;
    [SerializeField] public Button _rightButton;

    public event Action _OnLeftButtonClicked;
    public event Action _OnRightButtonClicked;

    protected override void Initialize()
    {
        
    }

    protected override void BindUIEvent()
    {
        _leftButton?.onClick.AddListener(() => _OnLeftButtonClicked?.Invoke());
        _rightButton?.onClick.AddListener(() => _OnRightButtonClicked?.Invoke());
    }
}
