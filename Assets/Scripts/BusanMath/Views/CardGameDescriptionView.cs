using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BusanMath.Views
{
    public class CardGameDescriptionView : BaseView
    {
        // ── 상단 UI ───────────────────────────────────
        [Header("=== 상단 UI ===")]
        [SerializeField] public Image _backgroundImage;    // 배경 이미지
        [SerializeField] public Image _titleImage;         // 타이틀 이미지
        [SerializeField] public Button _homeButton;        // 홈 이동 버튼

        // ── 스와이프 설명 ─────────────────────────────
        [Header("=== 스와이프 설명 ===")]
        [SerializeField] public Image _exampleViewBackgroundImage;   // 예시 영역 배경 이미지
        [SerializeField] public Image _exampleViewBoundaryImage;     // 예시 영역 경계 이미지
        [SerializeField] public GameObject _swipeUIObj;              // 스와이프 UI 오브젝트
        [SerializeField] public List<TMP_Text> _descriptionTextList = new List<TMP_Text>(); // 설명 텍스트 리스트

        // ── 네비게이션 버튼 ───────────────────────────
        [Header("=== 네비게이션 버튼 ===")]
        [SerializeField] public Button _prevButton;        // 이전 페이지 버튼
        [SerializeField] public Button _nextButton;        // 다음 페이지 버튼
        [SerializeField] public Button _startButton;       // 게임 시작 버튼

        // ── 이벤트 ────────────────────────────────────
        public event Action _OnHomeButtonClicked;          // 홈 버튼 클릭 시 발생
        public event Action _OnPrevButtonClicked;          // 이전 버튼 클릭 시 발생
        public event Action _OnNextButtonClicked;          // 다음 버튼 클릭 시 발생
        public event Action _OnStartButtonClicked;         // 시작 버튼 클릭 시 발생

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
}
