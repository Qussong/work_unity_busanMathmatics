using System;
using UnityEngine;
using UnityEngine.UI;

namespace BusanMath.Views
{
    public class VoteView : BaseView
    {
        // ── 상단 UI ───────────────────────────────────
        [Header("=== 상단 UI ===")]
        [SerializeField] public Image _backgroundImage;    // 배경 이미지
        [SerializeField] public Image _titleImage;         // 타이틀 이미지
        [SerializeField] public Button _homeButton;        // 홈 이동 버튼

        // ── 투표 버튼 ─────────────────────────────────
        [Header("=== 투표 버튼 ===")]
        [SerializeField] public Button _egyptButton;       // 이집트 투표 버튼
        [SerializeField] public Button _chinaButton;       // 중국 투표 버튼
        [SerializeField] public Button _romaButton;        // 로마 투표 버튼

        // ── 이벤트 ────────────────────────────────────
        public event Action _OnHomeButtonClicked;          // 홈 버튼 클릭 시 발생
        public event Action _OnEgyptButtonClicked;         // 이집트 투표 버튼 클릭 시 발생
        public event Action _OnChinaButtonClicked;         // 중국 투표 버튼 클릭 시 발생
        public event Action _OnRomaButtonClicked;          // 로마 투표 버튼 클릭 시 발생

        protected override void Initialize()
        {
        }

        protected override void BindUIEvent()
        {
            _homeButton.onClick.AddListener(() => _OnHomeButtonClicked?.Invoke());
            _egyptButton.onClick.AddListener(() => _OnEgyptButtonClicked?.Invoke());
            _chinaButton.onClick.AddListener(() => _OnChinaButtonClicked?.Invoke());
            _romaButton.onClick.AddListener(() => _OnRomaButtonClicked?.Invoke());
        }
    }
}
