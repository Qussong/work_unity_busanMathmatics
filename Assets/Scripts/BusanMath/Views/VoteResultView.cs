using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BusanMath.Views
{
    public class VoteResultView : BaseView
    {
        // ── 상단 UI ───────────────────────────────────
        [Header("=== 상단 UI ===")]
        [SerializeField] public Image _titleImage;         // 타이틀 이미지
        [SerializeField] public Image _boardImage;         // 결과 보드 배경 이미지
        [SerializeField] public Button _homeButton;        // 홈 이동 버튼

        // ── 투표 결과 표시 ────────────────────────────
        [Header("=== 투표 결과 표시 ===")]
        [SerializeField] public List<TMP_Text> _rankCountryList = new List<TMP_Text>();    // 순위별 국가명 텍스트
        [SerializeField] public List<Image> _countryViewList = new List<Image>();           // 순위별 국가 이미지
        [SerializeField] public List<TMP_Text> _votePercentList = new List<TMP_Text>();    // 순위별 투표 비율 텍스트
        [SerializeField] public List<Slider> _voteRateBarList = new List<Slider>();        // 순위별 투표 비율 바
        [SerializeField] public List<TMP_Text> _voteCountList = new List<TMP_Text>();      // 순위별 투표 수 텍스트

        // ── 국가 스프라이트 ───────────────────────────
        [Header("=== 국가 스프라이트 ===")]
        [SerializeField] public List<Sprite> _countryViewSpriteList = new List<Sprite>();  // 국가별 이미지 스프라이트

        // ── 이벤트 ────────────────────────────────────
        public event Action _OnHomeButtonClicked;          // 홈 버튼 클릭 시 발생

        protected override void Initialize()
        {
        }

        protected override void BindUIEvent()
        {
            _homeButton.onClick.AddListener(() => _OnHomeButtonClicked?.Invoke());
        }
    }
}
