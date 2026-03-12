using TMPro;
using UnityEngine;
using BusanMath.FSM;
using BusanMath.Views;
using BusanMath.Controllers;
using BusanMath.Managers;
using BusanMath.Models;

namespace BusanMath.FSM.States
{
    public class CardGameDescriptionState : BaseState<CardGameDescriptionState, CardGameDescriptionView>
    {
    private SwipeUI.SwipeUI _swipeUI;
    private bool textColorChangeFlag = false;

    public CardGameDescriptionState(CardGameDescriptionView view) : base(view)
    {
        // SwipeUI 참조 (1회 캐싱)
        _swipeUI = _view._swipeUIObj.GetComponentInChildren<SwipeUI.SwipeUI>();
    }

    public override void Init()
    {
        base.Init();

        // 이벤트 구독 (최초 1회)
        _view._OnHomeButtonClicked += () => { NavigationController.Instance.GoToHome(); };
        _view._OnPrevButtonClicked += () => { _swipeUI.AutoSwipe(true); };
        _view._OnNextButtonClicked += () => { _swipeUI.AutoSwipe(false); };
        _view._OnStartButtonClicked += () => { NavigationController.Instance.GoToCardGame(); };
    }

    public override void Enter()
    {
        base.Enter();
        _view.Show();

        // swipe 후처리를 위한 이벤트 등록 (매 진입 시)
        _swipeUI._OnSwipeCompleted += TextColorChange;

        // 현재 페이지를 0번 페이지로 돌리기
        int backCnt = _swipeUI.CurrentPage;
        for (int i = 0; i < backCnt; ++i)
        {
            _swipeUI.AutoSwipe(true);
        }
    }

    public override void Update()
    {
        if (true == textColorChangeFlag)
        {
            ChangeDescriptionColor();
            textColorChangeFlag = false;
        }
    }

    public override void Exit()
    {
        base.Exit();

        // swipe 이벤트 해제
        _swipeUI._OnSwipeCompleted -= TextColorChange;

        _view.Hide();
    }

    private void ChangeDescriptionColor()
    {
        int curPageIdx = _swipeUI.CurrentPage;
        foreach (TMP_Text targetText in _view._descriptionTextList)
        {
            targetText.color = Color.white;
        }
        _view._descriptionTextList[curPageIdx].color = new Color(1f, 0.87f, 0.39f);
    }

    public void TextColorChange()
    {
        textColorChangeFlag = true;
    }
    }
}
