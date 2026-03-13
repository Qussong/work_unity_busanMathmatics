using System;
using UnityEngine;
using UnityEngine.UI;

namespace BusanMath.Views
{
    public class SelectView : BaseView
    {
        // ── 영상 ────────────────────────────────────
        [Header("=== 영상 ===")]
        public string _fileName;          // 인트로 영상 파일명 (StreamingAssets 기준)
        public RawImage _displayImage;    // 영상 출력 대상 이미지

        // ── 상단 UI ─────────────────────────────────
        [Header("=== 상단 UI ===")]
        public Image _titleImage;         // 타이틀 이미지
        public Button _homeButton;        // 홈 이동 버튼
        public Image _skipImage;          // 스킵 버튼 이미지
        public Button _skipButton;        // 영상 스킵 버튼

        // ── 국가 선택 버튼 ──────────────────────────
        [Header("=== 국가 선택 버튼 ===")]
        public GameObject _buttonContainer;  // 국가 버튼 컨테이너 (CanvasGroup 페이드 대상)
        public Button _egyptButton;          // 이집트 선택 버튼
        public Button _chinaButton;          // 중국 선택 버튼
        public Button _romaButton;           // 로마 선택 버튼

        // ── 진행바 ──────────────────────────────────
        [Header("=== 진행바 ===")]
        public Slider _progressbar;       // 영상 재생 진행바 (드래그로 탐색 가능)

        // ── 이벤트 ──────────────────────────────────
        public event Action _OnHomeButtonClicked;    // 홈 버튼 클릭 시 발생
        public event Action _OnSkipButtonClicked;    // 스킵 버튼 클릭 시 발생
        public event Action _OnEgyptButtonClicked;   // 이집트 버튼 클릭 시 발생
        public event Action _OnChinaButtonClicked;   // 중국 버튼 클릭 시 발생
        public event Action _OnRomaButtonClicked;    // 로마 버튼 클릭 시 발생

        protected override void Initialize()
        {
        }

        protected override void BindUIEvent()
        {
            _homeButton?.onClick.AddListener(() => _OnHomeButtonClicked?.Invoke());
            _skipButton?.onClick.AddListener(() => _OnSkipButtonClicked?.Invoke());
            _egyptButton?.onClick.AddListener(() => _OnEgyptButtonClicked?.Invoke());
            _chinaButton?.onClick.AddListener(() => _OnChinaButtonClicked?.Invoke());
            _romaButton?.onClick.AddListener(() => _OnRomaButtonClicked?.Invoke());
        }
    }
}
