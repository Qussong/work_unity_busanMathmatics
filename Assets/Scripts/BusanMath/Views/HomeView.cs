using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BusanMath.Views
{
    public class HomeView : BaseView
    {
        // ── UI 요소 ───────────────────────────────────
        [Header("=== UI 요소 ===")]
        [SerializeField] public Image _backgroundImage;    // 배경 이미지
        [SerializeField] public Image _titleImage;         // 타이틀 이미지
        [SerializeField] public TMP_Text _subTitleText;    // 서브 타이틀 텍스트

        // ── 버튼 ──────────────────────────────────────
        [Header("=== 버튼 ===")]
        [SerializeField] public Button _leftButton;        // 왼쪽 버튼 (국가 선택으로 이동)
        [SerializeField] public Button _rightButton;       // 오른쪽 버튼 (숫자게임 설명으로 이동)

        // ── 이벤트 ────────────────────────────────────
        public event Action _OnLeftButtonClicked;          // 왼쪽 버튼 클릭 시 발생
        public event Action _OnRightButtonClicked;         // 오른쪽 버튼 클릭 시 발생

        protected override void Initialize()
        {
        }

        protected override void BindUIEvent()
        {
            _leftButton?.onClick.AddListener(() => _OnLeftButtonClicked?.Invoke());
            _rightButton?.onClick.AddListener(() => _OnRightButtonClicked?.Invoke());
        }
    }
}
