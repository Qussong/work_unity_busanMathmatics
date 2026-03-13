using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BusanMath.Views
{
    public class NumGameView : BaseView
    {
        // ── 상단 UI ────────────────────────────────────
        [Header("=== 상단 UI ===")]
        public Image _background;          // 배경 이미지 (국가별 변경)
        public Image _title;               // 타이틀 이미지 (국가별 변경)
        public Button _homeButton;         // 홈 이동 버튼
        public Button _hintButton;         // 힌트 팝업 열기 버튼

        // ── 퀴즈 보드 ─────────────────────────────────
        [Header("=== 퀴즈 보드 ===")]
        public Image _quizBoard;           // 퀴즈 보드 배경 이미지
        public Image _rndNumImage;         // 랜덤 숫자 이미지 (이집트/로마)
        public TMP_Text _rndNumText;       // 랜덤 숫자 텍스트 (중국 한자)
        public GameObject _answerTileContainer;  // 정답 타일 컨테이너

        // ── 넘패드 ────────────────────────────────────
        [Header("=== 넘패드 ===")]
        public Image _numPadTitle;         // 넘패드 타이틀 이미지
        public List<Button> _numButtons;   // 숫자 입력 버튼 리스트 (0~9)
        public Button _initButton;         // 정답 초기화 버튼
        public Button _compareButton;      // 정답 비교(완료) 버튼

        // ── 힌트 팝업 ─────────────────────────────────
        [Header("=== 힌트 팝업 ===")]
        public GameObject _hintContainer;  // 힌트 팝업 컨테이너
        public Image _popupHint;           // 힌트 이미지 (국가별 숫자 표)
        public Button _popupCloseButton;   // 힌트 팝업 닫기 버튼

        // ── 결과 팝업 ─────────────────────────────────
        [Header("=== 결과 팝업 ===")]
        public GameObject _resultContainer;     // 결과 팝업 컨테이너
        public Image _resultBoard;              // 결과 보드 배경 이미지
        public Image _infoBoard;                // 정보 보드 배경 이미지
        public TMP_Text _infoText;              // 정답/오답 안내 텍스트
        public Button _resultRetryButton;       // 다시하기 버튼
        public Button _resultMoveNext;          // 다음 게임(카드게임) 이동 버튼
        public Button _resultOtherCountry;      // 다른 나라 선택 버튼

        // ── 국가별 스프라이트 ──────────────────────────
        [Header("=== 국가별 스프라이트 ===")]
        public List<Sprite> _backGroundList;    // 국가별 배경 스프라이트 (index = ECountry)
        public List<Sprite> _titleList;         // 국가별 타이틀 스프라이트 (index = ECountry)
        public List<Sprite> _hintList;          // 국가별 힌트 스프라이트 (index = ECountry)

        // ── 프리팹 ────────────────────────────────────
        [Header("=== 프리팹 ===")]
        public GameObject _answerTilePrefab;    // 정답 타일 프리팹

        // ── 이벤트 ────────────────────────────────────
        public event Action _OnHomeButtonClicked;          // 홈 버튼 클릭 시 발생
        public event Action _OnHintButtonClikced;          // 힌트 버튼 클릭 시 발생
        public event Action _OnHintCloseButtonClicked;     // 힌트 닫기 버튼 클릭 시 발생
        public event Action _OnRetryButtonClicked;         // 다시하기 버튼 클릭 시 발생
        public event Action _OnMoveNextButtonClicked;      // 다음 이동 버튼 클릭 시 발생
        public event Action _OnOtherCountryButtonClicked;  // 다른 나라 버튼 클릭 시 발생

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
