using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NumGameView : BaseView
{
    [Header("=== NumGameDescription View Settings ===")]
    public Image _background;
    public Button _homeButton;
    public Image _hint;
    public TMP_Text _hintText;
    public Button _hintButton;
    public Image _title;
    public Image _quizBoard;
    public Image _rndNumImage;      // 랜덤값에 해당하는 이미지 (이집트, 로마)
    public TMP_Text _rndNumText;    // 랜덤값에 해당하는 텍스트 (한자)
    public GameObject _answerTileContainer;
    public Image _numPadTitle;

    [Header("=== NumPad Settings ===")]
    public List<Button> _numButtons;
    public Button _initButton;
    public Button _compareButton;

    [Header("=== Hint Popup Settings ===")]
    public GameObject _hintContainer;
    public Image _popupHint;
    public Button _popupCloseButton;

    [Header("=== Result Popup Settings ====")]
    public GameObject _resultContainer;
    public Image _resultBoard;
    public Image _infoBoard;
    public TMP_Text _infoText;
    public Button _resultRetryButton;   // 다시하기
    public Button _resultMoveNext;      // 다음으로, numGame -> carGameDescription
    public Button _resultOtherCountry;  // 다른 숫자 체험하기, numGame -> select

    [Header("Sprites")]
    public List<Sprite> _backGroundList;
    public List<Sprite> _titleList;
    public List<Sprite> _hintList;

    [Header("Prefab")]
    public GameObject _answerTilePrefab;

    public event Action _OnHomeButtonClicked;
    public event Action _OnHintButtonClikced;
    public event Action _OnHintCloseButtonClicked;
    public event Action _OnRetryButtonClicked;
    public event Action _OnMoveNextButtonClicked;
    public event Action _OnOtherCountryButtonClicked;

    protected override void Awake()
    {
        base.Awake();

        // 랜덤값을 보여주는 이미지 객체 off (이집트, 로마)
        _rndNumImage.gameObject.SetActive(false);
        // 랜덤값을 보여주는 텍스트 객체 off (중국)
        _rndNumText.gameObject.SetActive(false);
    }

    protected override void Initialize()
    {
        // 힌트 팝업 패널 off
        _hintContainer.SetActive(false);
        // 결과 팝업 패널 off
        _resultContainer.SetActive(false);
    }

    protected override void BindUIEvent()
    {
        _homeButton.onClick.AddListener(() => _OnHomeButtonClicked?.Invoke());
        _hintButton.onClick.AddListener(() => _OnHintButtonClikced?.Invoke());
        _popupCloseButton.onClick.AddListener(() => _OnHintCloseButtonClicked?.Invoke());
        _resultRetryButton.onClick.AddListener(() => _OnRetryButtonClicked?.Invoke());
        _resultMoveNext.onClick.AddListener(() => _OnMoveNextButtonClicked?.Invoke());
        _resultOtherCountry.onClick.AddListener(() => _OnOtherCountryButtonClicked?.Invoke());
    }

}
