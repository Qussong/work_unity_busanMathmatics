using LS.DrawTexture.Runtime;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WriteView : BaseView
{
    [Header("=== NumGameDescription View Settings ===")]
    public Image _backgroundImage;
    public Button _homeButton;
    public Image _titleImage;
    public TMP_Text _subTitleText;
    public Image _yearBoardImage;
    public Image _monthBoardImage;
    public Image _dayBoardImage;
    public TMP_Text _yearUnitText;
    public TMP_Text _monthUnitText;
    public TMP_Text _dayUnitText;
    public Button _okayButton;
    public Image _arrowImage;
    //public Image _writingBoardImage;
    public Button _moveNextButton;
    public GameObject _writeBoardContainer;
    public SwipeUI.SwipeUI _yearUI;
    public SwipeUI.SwipeUI _monthUI;
    public SwipeUI.SwipeUI _dayUI;

    [Header("=== DrawTextureUI Settings===")]
    public DrawTextureUI _drawTextureUI;

    [Header("Date Select Settings")]
    public List<GameObject> _years;
    public List<GameObject> _months;
    public List<GameObject> _days;

    [Header("=== Writingboard Settings ===")]
    public Image _yearPreview;
    public Image _monthPreview;
    public Image _dayPreview;

    [Header("=== Sprites ===")]
    public List<Sprite> _titleSpriteList = new List<Sprite>();
    public List<Sprite> _backgroundSpriteList = new List<Sprite>();

    [Header("=== Egypt Sprites ===")]
    public List<Sprite> _yearEgyptList = new List<Sprite>();
    public List<Sprite> _monthEgyptList = new List<Sprite>();
    public List<Sprite> _dayEgyptList = new List<Sprite>();

    [Header("=== China Sprites ===")]
    public List<Sprite> _yearChinaList = new List<Sprite>();
    public List<Sprite> _monthChinaList = new List<Sprite>();
    public List<Sprite> _dayChinaList = new List<Sprite>();

    [Header("=== Roma Sprites ===")]
    public List<Sprite> _yearRomaList = new List<Sprite>();
    public List<Sprite> _monthRomaList = new List<Sprite>();
    public List<Sprite> _dayRomaList = new List<Sprite>();

    public event Action _OnHomeButtonClicked;
    public event Action _OnOkayButtonClicked;
    public event Action _OnMoveNextButtonClicked;

    protected override void Initialize()
    {
        _writeBoardContainer.SetActive(false);
    }

    protected override void BindUIEvent()
    {
        _homeButton.onClick.AddListener(() => _OnHomeButtonClicked?.Invoke());
        _okayButton.onClick.AddListener(() => _OnOkayButtonClicked?.Invoke());
        _moveNextButton.onClick.AddListener(() => _OnMoveNextButtonClicked?.Invoke());
    }
}
