using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WriteView : BaseView
{
    [Header("=== NumGameDescription View Settings ===")]
    [SerializeField] public Image _backgroundImage;
    [SerializeField] public Button _homeButton;
    [SerializeField] public Image _titleImage;
    [SerializeField] public TMP_Text _subTitleText;
    [SerializeField] public Image _yearBoardImage;
    [SerializeField] public Image _monthBoardImage;
    [SerializeField] public Image _dayBoardImage;
    [SerializeField] public TMP_Text _yearUnitText;
    [SerializeField] public TMP_Text _monthUnitText;
    [SerializeField] public TMP_Text _dayUnitText;
    [SerializeField] public Button _okayButton;
    [SerializeField] public Image _arrowImage;
    [SerializeField] public Image _writingBoardImage;
    [SerializeField] public Button _moveNextButton;

    [Header("=== Localized Sprites ===")]
    [SerializeField] public List<Sprite> _titleSpriteList = new List<Sprite>();
    [SerializeField] public List<Sprite> _backgroundSpriteList = new List<Sprite>();

    public event Action _OnHomeButtonClicked;
    public event Action _OnOkayButtonClicked;
    public event Action _OnMoveNextButtonClicked;

    protected override void Initialize()
    {
    }

    protected override void BindUIEvent()
    {
        _homeButton.onClick.AddListener(() => _OnHomeButtonClicked?.Invoke());
        _okayButton.onClick.AddListener(() => _OnOkayButtonClicked?.Invoke());
        _moveNextButton.onClick.AddListener(() => _OnMoveNextButtonClicked?.Invoke());
    }
}
