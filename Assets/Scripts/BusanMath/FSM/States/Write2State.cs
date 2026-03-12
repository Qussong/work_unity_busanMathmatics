using System;
using TMPro;
using UnityEngine;

public class Write2State : BaseState<Write2State, Write2View>
{
    // 선택된 년,월,일
    private int _selectedYear;
    private int _selectedMonth;
    private int _selectedDay;

    public Write2State(Write2View view) : base(view)
    {
    }

    #region Override (BaseState)

    /// <summary>
    /// 최초 1회 초기화 (View 이벤트 구독 등)
    /// StateMachine이 HashSet으로 추적하여 중복 호출 방지
    /// </summary>
    public override void Init()
    {
        base.Init();

        CreateDateButtons(_view._objYearContainer, 51, v => _selectedYear = v, 1980);   // 년도: 51개 (1980~2030)
        CreateDateButtons(_view._objMonthContainer, 12, v => _selectedMonth = v);    // 월: 12개 (1~12)
        CreateDateButtons(_view._objDayContainer, 31, v => _selectedDay = v);        // 일: 31개 (1~31)

        _view._OnYearButtonClicked += () => ShowContainer(_view._objSelectYearPanel);
        _view._OnMonthButtonClicked += () => ShowContainer(_view._objSelectMonthPanel);
        _view._OnDayButtonClicked += () => ShowContainer(_view._objSelectDayPanel);
    }

    public override void Enter()
    {
        // Debug.Log($"[{typeof(TState).Name}] Enter");
        base.Enter();

        _selectedYear = 0;
        _selectedMonth = 0;
        _selectedDay = 0;
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        // Debug.Log($"[{typeof(TState).Name}] Exit");
        base.Exit();
    }

    /// <summary>
    /// 이벤트 구독 해제 (프로그램 종료 시)
    /// </summary>
    public override void Dispose()
    {
        base.Dispose();
    }

    #endregion

    #region Private

    private void ShowContainer(GameObject selectPanel)
    {
        _view._objSelectYearPanel.SetActive(false);
        _view._objSelectMonthPanel.SetActive(false);
        _view._objSelectDayPanel.SetActive(false);

        selectPanel.SetActive(true);
    }

    private void CreateDateButtons(GameObject container, int count, Action<int> onSelect, int startValue = 1)
    {
        for (int i = 0; i < count; i++)
        {
            var btn = UnityEngine.Object.Instantiate(_view._prefabDateBtn, container.transform);
            var tmp = btn.GetComponentInChildren<TMP_Text>();
            int value = startValue + i;
            if (tmp != null)
            {
                tmp.text = value.ToString();
            }

            var button = btn.GetComponentInChildren<UnityEngine.UI.Button>();
            if (button != null)
            {
                button.onClick.AddListener(() => onSelect?.Invoke(value));
            }
        }
    }

    #endregion

    #region Public

    public void SelectYear(int year)
    {
        _selectedYear = year;
    }

    public void SelectMonth(int month)
    {
        _selectedMonth = month;
    }

    public void SelectDay(int day)
    {
        _selectedDay = day;
    }

    #endregion

}
