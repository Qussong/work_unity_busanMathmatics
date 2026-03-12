using System;
using UnityEngine;
using UnityEngine.UI;

namespace BusanMath.Views
{
    public class WriteView : BaseView
    {
        [Header("=== Year/Month/Day Board ===")]
        public Button _btnYear;
        public Button _btnMonth;
        public Button _btnDay;

        public GameObject _objYearContainer;
        public GameObject _objMonthContainer;
        public GameObject _objDayContainer;
        public GameObject _prefabDateBtn;

        public GameObject _objSelectYearPanel;
        public GameObject _objSelectMonthPanel;
        public GameObject _objSelectDayPanel;

        public event Action _OnYearButtonClicked;
        public event Action _OnMonthButtonClicked;
        public event Action _OnDayButtonClicked;

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
        }
    }
}
