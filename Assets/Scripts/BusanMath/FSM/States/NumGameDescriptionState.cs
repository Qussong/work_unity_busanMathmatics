using NUnit.Framework;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class NumGameDescriptionState : BaseState
{
    private NumGameDescriptionView _numGameDescriptionView;
    private ECountry _country;
    private SwipeUI.SwipeUI _swipeUI;
    private bool textColorChangeFlag = false;

    public ECountry Country
    {
        set
        {
            _country = value;
        }
    }

    public NumGameDescriptionState(NumGameDescriptionView view)
    {
        _numGameDescriptionView = view;

        // SwipeUI 클래스 참조
        _swipeUI = _numGameDescriptionView._swipeUIObj.GetComponentInChildren<SwipeUI.SwipeUI>();

        // 이벤트 등록
        _numGameDescriptionView._OnHomeButtonClicked += () => { NavigationController.Instance.GoToHome(); };
        _numGameDescriptionView._OnPrevButtonClicked += () => { _swipeUI.AutoSwipe(true); };
        _numGameDescriptionView._OnNextButtonClicked += () => { _swipeUI.AutoSwipe(false); };
        _numGameDescriptionView._OnStartButtonClicked += () => { NavigationController.Instance.GoToNumGame(_country); };
    }

    public override void Enter()
    {
        Debug.Log("[NumGameDescriptionState] Enter");
        _numGameDescriptionView.Show();

        // swipe 후속처리를 위한 이벤트 등록
        _swipeUI._OnSwipeCompleted += TextColorChange;

        // 선택된 국가에 맞는 예시 이미지 설정
        SetExampleView();

        // 최초 시작할 때 0번 페이지를 볼 수 있도록 설정
        int backCnt = _swipeUI.CurrentPage;
        for (int i = 0; i < backCnt; ++i)
        {
            _swipeUI.AutoSwipe(true);
        }
    }

    public override void Update()
    {
        if(true == textColorChangeFlag)
        {
            ChangeDescriptionColor();
            textColorChangeFlag = false;
        }
    }

    public override void Exit()
    {
        Debug.Log("[NumGameDescriptionState] Eixt");

        // 예시 이미지 초기화
        EmptyExampleView();

        // 국가 정보 초기화
        _country = ECountry.None;

        // swipe 후속처리를 위한 이벤트 등록 해제
        _swipeUI._OnSwipeCompleted += TextColorChange;

        _numGameDescriptionView.Hide();
    }

    private void ChangeDescriptionColor()
    {
        // Description Text 객체의 폰트 색상 전부 white 로 변경
        int curPageIdx = _swipeUI.CurrentPage;
        foreach(TMP_Text targetText in _numGameDescriptionView._descriptionTextList)
        {
            targetText.color = Color.white;
        }

        // 현재 인덱스에 해당하는 Text 객체의 폰트 색상만 변경
        _numGameDescriptionView._descriptionTextList[curPageIdx].color = new Color(1f, 0.87f, 0.39f);
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
                spriteList = _numGameDescriptionView._egyptExampleViewSpriteList;
                break;
            case ECountry.China:
                spriteList = _numGameDescriptionView._chinaExampleViewSpriteList;
                break;
            case ECountry.Roma:
                spriteList = _numGameDescriptionView._romaExampleViewSpriteList;
                break;
        }

        int swipePageTotalCnt = 3;
        for(int i = 0; i < swipePageTotalCnt; ++i)
        {
            _numGameDescriptionView._swipeImageList[i].sprite = spriteList[i];
        }
    }

    private void EmptyExampleView()
    {
        int swipePageTotalCnt = 3;
        for (int i = 0; i < swipePageTotalCnt; ++i)
        {
            _numGameDescriptionView._swipeImageList[i].sprite = null;
        }
    }

}
