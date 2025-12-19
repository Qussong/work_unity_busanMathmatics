using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class CardGameState : BaseState
{
    private CardGameView _cardGameView;
    private float _totalTime = 60f;
    private float _remainingTime = 0f;
    private bool _isRunning = false;

    public CardGameState(CardGameView view)
    {
        _cardGameView = view;

        // 이벤트 등록 (CardGameView)
        _cardGameView._OnHomeButtonClicked += () => { NavigationController.Instance.GoToHome(); };
        _cardGameView._OnRetryButtonClicked += () => { HandleRetryButton(); };
        _cardGameView._OnNextButtonClicked += () => { NavigationController.Instance.GoToVote(); };

        // 이벤트 등록 (CardGameManager)
        CardGameManager.Instance._OnMatchSuccess += HandleMatchSuccess;
        CardGameManager.Instance._OnMatchFail += HandleMatchFail;
        CardGameManager.Instance._OnGameClear += HandleGameClear;

    }

    public override void Enter()
    {
        Debug.Log("[CardGameState] Enter");
        _cardGameView.Show();

        // 카드 게임 시작
        CardGameManager.Instance.StartGame();

        // 카드 세팅
        CardSet();

        // 타이머시작
        StartTimer();
    }

    public override void Update()
    {
        if (false == _isRunning) return;

        _remainingTime -= Time.deltaTime;
        UpdateTimerUI();

        if (_remainingTime <= 0)
        {
            HandleGameClear();
            _remainingTime = 0f;
            _isRunning = false;
        }
    }

    public override void Exit()
    {
        Debug.Log("[CardGameState] Eixt");

        // 게임 초기화
        CardGameManager.Instance.ResetGame();

        // 카드 세팅 초기화
        CardInit();

        // 팝업창 숨김
        _cardGameView._popupContainerObj.SetActive(false);

        // 타이머 초기화
        ResetTimer();

        _cardGameView.Hide();
    }

    private void StartTimer()
    {
        _remainingTime = _totalTime;
        _isRunning = true;
    }

    private void StopTimer()
    {
        _isRunning = false;
    }

    public void ResetTimer()
    {
        _remainingTime = _totalTime;
        _isRunning = false;
        UpdateTimerUI();
    }

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(_remainingTime / 60);
        int seconds = Mathf.FloorToInt(_remainingTime % 60);
        _cardGameView._timerText.text = $"{minutes:00}:{seconds:00}";  // "01:00" 형식
    }

    private void CardSet()
    {
        // card 에 CardFlip 컴포넌트 추가
        int idx = 0;
        foreach (Image card in _cardGameView._cardList)
        {
            if (null == card) continue;

            if (!card.TryGetComponent(out CardFlip flipComp))
            {
                flipComp = card.gameObject.AddComponent<CardFlip>();
            }
            // 카드 뒷면 이미지 할당
            flipComp._backSprite = _cardGameView._cardBackSprite;
            flipComp._cardIdx = idx;

            // 카드 클릭시 호출될 콜백함수 등록
            flipComp._OnClickCard += HandleClickCard;

            ++idx;
        }

        // 카드 앞면 이미지 할당
        var deck = CardGameManager.Instance.GetCurrentDeck();
        for (int i = 0; i < deck.Count; i++)
        {
            CardFlip flip = _cardGameView._cardList[i].gameObject.GetComponent<CardFlip>();
            flip._frontSprite = deck[i]._cardSprite;
        }
    }

    private void CardInit()
    {
        foreach (Image card in _cardGameView._cardList)
        {
            if (null == card) continue;
            CardFlip flipComp = card.GetComponent<CardFlip>();

            // 카드의 각종 값 초기화
            flipComp.Restore();

            // 카드 클릭시 호출될 콜백함수 해제
            flipComp._OnClickCard -= HandleClickCard;
        }

    }

    // 카드 클릭 시
    public void HandleClickCard(int index)
    {
        CardGameManager.Instance.SelectCard(index);
    }

    void HandleMatchSuccess(int index1, int index2)
    {
        Debug.Log($"매칭 성공! {index1}, {index2}");

        // 카드 비활성화 처리
        _cardGameView._cardList[index1].raycastTarget = false;
        _cardGameView._cardList[index1].color = new Color(0.5f, 0.5f, 0.5f, 1f);
        _cardGameView._cardList[index2].raycastTarget = false;
        _cardGameView._cardList[index2].color = new Color(0.5f, 0.5f, 0.5f, 1f);
    }

    void HandleMatchFail(int index1, int index2)
    {
        Debug.Log($"매칭 실패! {index1}, {index2}");

        // 카드 뒤집기 처리
        _cardGameView._cardList[index1].gameObject.GetComponent<CardFlip>().Flip();
        _cardGameView._cardList[index2].gameObject.GetComponent<CardFlip>().Flip();
    }

    void HandleGameClear()
    {
        Debug.Log("게임 클리어!");

        // 팝업창 등장
        _cardGameView._popupContainerObj.SetActive(true);

        if (false == _isRunning)
        {
            CardGameManager.Instance._isSuccess = false;
        }

        // 팝업창 문구 설정
        _cardGameView._infoText.text =
        CardGameManager.Instance._isSuccess ?
        "제한 시간내에\n문제를 해결했습니다!" :
        "제한 시간내에\n문제를 해결하지 못했습니다!";

        // 팝업창 기록 설정
        int minutes = Mathf.FloorToInt(_remainingTime / 60);
        int seconds = Mathf.FloorToInt(_remainingTime % 60);
        _cardGameView._recordText.text =
            CardGameManager.Instance._isSuccess ?
            $"기록 : {minutes:00}:{seconds:00}" :
            "기록 : x";

        // 타이머 정지
        StopTimer();
    }

    public void HandleRetryButton()
    {
        // 게임 초기화
        CardGameManager.Instance.ResetGame();
        // 카드 세팅 초기화
        CardInit();
        // 팝업창 숨김
        _cardGameView._popupContainerObj.SetActive(false);
        // 타이머 초기화
        ResetTimer();


        // 카드 게임 시작
        CardGameManager.Instance.StartGame();
        // 카드 세팅
        CardSet();
        // 타이머시작
        StartTimer();
    }

}
