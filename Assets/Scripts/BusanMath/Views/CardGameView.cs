using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardGameView : BaseView
{
    [Header("=== CardGameDescription View Settings ===")]
    public Image _backgroundImage;
    public Button _homeButton;
    public TMP_Text _timerTitleText;
    public Image _clockImage;
    public TMP_Text _timerText;
    public Image _titleImage;
    public List<Image> _cardList = new List<Image>();

    [Header("=== Popup Settings ===")]
    public GameObject _popupContainerObj;
    public Image _popupBoardImage;
    public Image _infoBoardImage;
    public TMP_Text _infoText;
    public TMP_Text _recordText;
    public Button _retryButton;
    public Button _nextButton;   // cardGame -> vote

    [Header("=== Sprites ===")]
    public List<Sprite> _titleImageList = new List<Sprite>();
    public Sprite _cardBackSprite;

    public event Action _OnHomeButtonClicked;
    public event Action _OnRetryButtonClicked;
    public event Action _OnNextButtonClicked;

    protected override void Initialize()
    {
    }

    protected override void BindUIEvent()
    {
        _homeButton.onClick.AddListener(() => _OnHomeButtonClicked?.Invoke());
        _retryButton.onClick.AddListener(() => _OnRetryButtonClicked?.Invoke());
        _nextButton.onClick.AddListener(() => _OnNextButtonClicked?.Invoke());
    }
}
