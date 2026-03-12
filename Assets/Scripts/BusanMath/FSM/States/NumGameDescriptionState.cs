using System.Collections.Generic;
using TMPro;
using UnityEngine;
using BusanMath.FSM;
using BusanMath.Views;
using BusanMath.Controllers;
using BusanMath.Managers;
using BusanMath.Models;

namespace BusanMath.FSM.States
{
    public class NumGameDescriptionState : BaseState<NumGameDescriptionState, NumGameDescriptionView>
    {
    private ECountry _country;
    private SwipeUI.SwipeUI _swipeUI;
    private bool textColorChangeFlag = false;

    public ECountry Country
    {
        set { _country = value; }
    }

    public NumGameDescriptionState(NumGameDescriptionView view) : base(view)
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
        _view._OnStartButtonClicked += () => { NavigationController.Instance.GoToNumGame(_country); };
    }

    public override void Enter()
    {
        base.Enter();
        _view.Show();

        // swipe 후처리를 위한 이벤트 등록 (매 진입 시)
        _swipeUI._OnSwipeCompleted += TextColorChange;

        // 선택된 국가에 맞는 예제 이미지 세팅
        SetExampleView();

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

        EmptyExampleView();
        _country = ECountry.None;

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

    private void SetExampleView()
    {
        List<Sprite> spriteList = null;
        switch (_country)
        {
            case ECountry.Egypt:
                spriteList = _view._egyptExampleViewSpriteList;
                break;
            case ECountry.China:
                spriteList = _view._chinaExampleViewSpriteList;
                break;
            case ECountry.Roma:
                spriteList = _view._romaExampleViewSpriteList;
                break;
        }

        int swipePageTotalCnt = 3;
        for (int i = 0; i < swipePageTotalCnt; ++i)
        {
            _view._swipeImageList[i].sprite = spriteList[i];
        }
    }

    private void EmptyExampleView()
    {
        int swipePageTotalCnt = 3;
        for (int i = 0; i < swipePageTotalCnt; ++i)
        {
            _view._swipeImageList[i].sprite = null;
        }
    }
    }
}
