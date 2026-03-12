using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BusanMath.Views
{
    public class NumGameView : BaseView
    {
        [Header("=== NumGame View Settings ===")]
        public Image _background;
        public Button _homeButton;
        public Image _hint;
        public TMP_Text _hintText;
        public Button _hintButton;
        public Image _title;
        public Image _quizBoard;
        public Image _rndNumImage;
        public TMP_Text _rndNumText;
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
        public Button _resultRetryButton;
        public Button _resultMoveNext;
        public Button _resultOtherCountry;

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
            _rndNumImage.gameObject.SetActive(false);
            _rndNumText.gameObject.SetActive(false);
        }

        protected override void Initialize()
        {
            _hintContainer.SetActive(false);
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
}
