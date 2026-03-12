using System;
using UnityEngine;
using UnityEngine.UI;

public class Write2View : BaseView
{
    [Header("=== Year/Month/Day Board ===")]
    public Button _btnYear;
    public Button _btnMonth;
    public Button _btnDay;

    public GameObject _objYearContainer; // 년도 버튼 컨테이너
    public GameObject _objMonthContainer;// 달 버튼 컨테이너
    public GameObject _objDayContainer;  // 일 버튼 컨테이너
    public GameObject _prefabDateBtn;  // 날짜 버튼 프리펩

    public GameObject _objSelectYearPanel;  // 년도 선택 패널
    public GameObject _objSelectMonthPanel; // 달 선택 패널
    public GameObject _objSelectDayPanel;   // 일 선택 패널

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
