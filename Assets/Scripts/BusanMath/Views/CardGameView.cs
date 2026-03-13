using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BusanMath.Views
{
    public class CardGameView : BaseView
    {
        // ── 상단 UI ───────────────────────────────────
        [Header("=== 상단 UI ===")]
        public Image _backgroundImage;        // 배경 이미지 (국가별 변경)
        public Image _titleImage;             // 타이틀 이미지 (국가별 변경)
        public Button _homeButton;            // 홈 이동 버튼

        // ── 타이머 ────────────────────────────────────
        [Header("=== 타이머 ===")]
        public TMP_Text _timerTitleText;      // 타이머 제목 텍스트
        public Image _clockImage;             // 시계 아이콘 이미지
        public TMP_Text _timerText;           // 남은 시간 텍스트 (60초 카운트다운)

        // ── 카드 보드 ─────────────────────────────────
        [Header("=== 카드 보드 ===")]
        public List<Image> _cardList = new List<Image>();  // 카드 이미지 리스트 (12장)

        // ── 결과 팝업 ─────────────────────────────────
        [Header("=== 결과 팝업 ===")]
        public GameObject _popupContainerObj;  // 결과 팝업 컨테이너
        public Image _popupBoardImage;         // 팝업 보드 배경 이미지
        public Image _infoBoardImage;          // 정보 보드 배경 이미지
        public TMP_Text _infoText;             // 결과 안내 텍스트 (성공/실패)
        public TMP_Text _recordText;           // 기록 텍스트 (소요 시간)
        public Button _retryButton;            // 다시하기 버튼
        public Button _nextButton;             // 다음 화면(투표) 이동 버튼

        // ── 스프라이트 ────────────────────────────────
        [Header("=== 스프라이트 ===")]
        public List<Sprite> _titleImageList = new List<Sprite>();  // 국가별 타이틀 스프라이트 (index = ECountry)
        public Sprite _cardBackSprite;         // 카드 뒷면 스프라이트

        // ── 이벤트 ────────────────────────────────────
        public event Action _OnHomeButtonClicked;      // 홈 버튼 클릭 시 발생
        public event Action _OnRetryButtonClicked;     // 다시하기 버튼 클릭 시 발생
        public event Action _OnNextButtonClicked;      // 다음 이동 버튼 클릭 시 발생

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
}
