using System;
using UnityEngine;
using UnityEngine.UI;

namespace BusanMath.Views
{
    public class WriteView : BaseView
    {
        // ── 날짜 선택 버튼 ──────────────────────────────
        [Header("=== 날짜 선택 버튼 ===")]
        public Button _btnYear;   // 연도 선택 버튼
        public Button _btnMonth;  // 월 선택 버튼
        public Button _btnDay;    // 일 선택 버튼
        public Button _btnWriteDate;    // 날짜 선택 왼료 버튼

        // ── 날짜 항목 컨테이너 ──────────────────────────
        [Header("=== 날짜 항목 컨테이너 ===")]
        public GameObject _objYearContainer;   // 연도 버튼들이 생성될 부모 오브젝트
        public GameObject _objMonthContainer;  // 월 버튼들이 생성될 부모 오브젝트
        public GameObject _objDayContainer;    // 일 버튼들이 생성될 부모 오브젝트
        public GameObject _prefabDateBtn;      // 날짜 버튼 프리팹

        // ── 선택 패널 ────────────────────────────────
        [Header("=== 선택 패널 ===")]
        public GameObject _objSelectYearPanel;   // 연도 선택 드롭다운 패널
        public GameObject _objSelectMonthPanel;  // 월 선택 드롭다운 패널
        public GameObject _objSelectDayPanel;    // 일 선택 드롭다운 패널

        // ── 이벤트 ──────────────────────────────────
        public event Action _OnYearButtonClicked;   // 연도 버튼 클릭 시 발생
        public event Action _OnMonthButtonClicked;  // 월 버튼 클릭 시 발생
        public event Action _OnDayButtonClicked;    // 일 버튼 클릭 시 발생
        public event Action _OnWriteDateButtonClicked;  // 날짜 선택 완료 버튼 클릭 시 발생

        protected override void Initialize()
        {
            base.Initialize();

            _objSelectYearPanel.SetActive(false);
            _objSelectMonthPanel.SetActive(false);
            _objSelectDayPanel.SetActive(false);
        }

        protected override void BindUIEvent()
        {
            base.BindUIEvent();

            _btnYear.onClick.AddListener(() => _OnYearButtonClicked?.Invoke());
            _btnMonth.onClick.AddListener(() => _OnMonthButtonClicked?.Invoke());
            _btnDay.onClick.AddListener(() => _OnDayButtonClicked?.Invoke());
            _btnWriteDate.onClick.AddListener(() => _OnWriteDateButtonClicked?.Invoke());
        }
    }
}
