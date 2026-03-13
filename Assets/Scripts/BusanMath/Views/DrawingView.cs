using LS.DrawTexture.Runtime;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BusanMath.Views
{
    public class DrawingView : BaseView
    {
        // ── 드로잉 보드 ──────────────────────────────
        [Header("=== 드로잉 보드 ===")]
        public GameObject _writeBoardContainer;  // 드로잉 영역 컨테이너
        public DrawTextureUI _drawTextureUI;     // 터치 드로잉 컴포넌트

        // ── 버튼 ────────────────────────────────────
        [Header("=== 버튼 ===")]
        public Button _reselectButton;   // 날짜 다시 선택 버튼
        public Button _moveNextButton;   // 다음 화면(투표 결과) 이동 버튼

        // ── 날짜 미리보기 ────────────────────────────
        [Header("=== 날짜 미리보기 ===")]
        public Image _yearPreviewImage;   // 선택된 연도 숫자 이미지
        public Image _monthPreviewImage;  // 선택된 월 숫자 이미지
        public Image _dayPreviewImage;    // 선택된 일 숫자 이미지

        // ── 이집트 숫자 스프라이트 ───────────────────
        [Header("=== 이집트 숫자 스프라이트 ===")]
        public List<Sprite> _egyptYearSprites;   // 연도 이미지 (index = year - 1980)
        public List<Sprite> _egyptMonthSprites;  // 월 이미지 (index = month - 1)
        public List<Sprite> _egyptDaySprites;    // 일 이미지 (index = day - 1)

        // ── 중국 숫자 스프라이트 ─────────────────────
        [Header("=== 중국 숫자 스프라이트 ===")]
        public List<Sprite> _chinaYearSprites;   // 연도 이미지 (index = year - 1980)
        public List<Sprite> _chinaMonthSprites;  // 월 이미지 (index = month - 1)
        public List<Sprite> _chinaDaySprites;    // 일 이미지 (index = day - 1)

        // ── 로마 숫자 스프라이트 ─────────────────────
        [Header("=== 로마 숫자 스프라이트 ===")]
        public List<Sprite> _romaYearSprites;    // 연도 이미지 (index = year - 1980)
        public List<Sprite> _romaMonthSprites;   // 월 이미지 (index = month - 1)
        public List<Sprite> _romaDaySprites;     // 일 이미지 (index = day - 1)

        // ── 이벤트 ──────────────────────────────────
        public event Action _OnReselectButtonClicked;    // 다시 선택 버튼 클릭 시 발생
        public event Action _OnMoveNextButtonClicked;    // 다음 이동 버튼 클릭 시 발생

        protected override void Initialize()
        {
            _writeBoardContainer.SetActive(false);
        }

        protected override void BindUIEvent()
        {
            _reselectButton.onClick.AddListener(() => _OnReselectButtonClicked?.Invoke());
            _moveNextButton.onClick.AddListener(() => _OnMoveNextButtonClicked?.Invoke());
        }
    }
}
