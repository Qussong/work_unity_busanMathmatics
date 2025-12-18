using TMPro;
using UnityEngine;

public class CardGameDescriptionState : BaseState
{
    private CardGameDescriptionView _cardGameDescriptionView;
    private SwipeUI.SwipeUI _swipeUI;
    private bool textColorChangeFlag = false;

    public CardGameDescriptionState(CardGameDescriptionView view)
    {
        _cardGameDescriptionView = view;

        // SwipeUI 클래스 참조
        _swipeUI = _cardGameDescriptionView._swipeUIObj.GetComponentInChildren<SwipeUI.SwipeUI>();

        // 이벤트 등록
        _cardGameDescriptionView._OnHomeButtonClicked += () => { NavigationController.Instance.GoToHome(); };
        _cardGameDescriptionView._OnPrevButtonClicked += () => { _swipeUI.AutoSwipe(true); };
        _cardGameDescriptionView._OnNextButtonClicked += () => { _swipeUI.AutoSwipe(false); };
        _cardGameDescriptionView._OnStartButtonClicked += () => { NavigationController.Instance.GoToCardGame(); };
    }

    public override void Enter()
    {
        Debug.Log("[CardGameDescriptionState] Enter");
        _cardGameDescriptionView.Show();

        // swipe 후속처리를 위한 이벤트 등록
        _swipeUI._OnSwipeCompleted += TextColorChange;

        // 최초 시작할 때 0번 페이지를 볼 수 있도록 설정
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
        Debug.Log("[CardGameDescriptionState] Eixt");

        // swipe 후속처리를 위한 이벤트 등록 해제
        _swipeUI._OnSwipeCompleted += TextColorChange;

        _cardGameDescriptionView.Hide();
    }

    private void ChangeDescriptionColor()
    {
        // Description Text 객체의 폰트 색상 전부 white 로 변경
        int curPageIdx = _swipeUI.CurrentPage;
        foreach (TMP_Text targetText in _cardGameDescriptionView._descriptionTextList)
        {
            targetText.color = Color.white;
        }

        // 현재 인덱스에 해당하는 Text 객체의 폰트 색상만 변경
        _cardGameDescriptionView._descriptionTextList[curPageIdx].color = new Color(1f, 0.87f, 0.39f);
    }

    public void TextColorChange()
    {
        textColorChangeFlag = true;
    }
}
