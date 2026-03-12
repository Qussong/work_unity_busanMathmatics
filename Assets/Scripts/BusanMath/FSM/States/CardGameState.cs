using UnityEngine;
using UnityEngine.UI;

public class CardGameState : BaseState<CardGameState, CardGameView>
{
    private float _totalTime = 60f;
    private float _remainingTime = 0f;
    private bool _isRunning = false;

    private float _timer = 0f;
    private bool _hasExecuted = false;

    public CardGameState(CardGameView view) : base(view) { }

    public override void Init()
    {
        base.Init();

        // 이벤트 구독 (최초 1회 - View 버튼)
        _view._OnHomeButtonClicked += () => { NavigationController.Instance.GoToHome(); };
        _view._OnRetryButtonClicked += () => { HandleRetryButton(); };
        _view._OnNextButtonClicked += () => { NavigationController.Instance.GoToVote(); };
    }

    public override void Enter()
    {
        base.Enter();
        _view.Show();

        // 이벤트 등록 (CardGameManager - 매 진입 시)
        CardGameManager.Instance._OnMatchSuccess += HandleMatchSuccess;
        CardGameManager.Instance._OnMatchFail += HandleMatchFail;
        CardGameManager.Instance._OnGameClear += HandleGameClear;

        // 카드 게임 시작
        CardGameManager.Instance.StartGame();

        // 카드 세팅
        CardSet();

        // 카드 프리뷰 (뒷면 -> 앞면)
        foreach (Image cardImage in _view._cardList)
        {
            cardImage.gameObject.GetComponent<CardFlip>().Flip();
        }

        // 2초 후 카드 프리뷰 (앞면 -> 뒷면)
        foreach (Image cardImage in _view._cardList)
        {
            cardImage.gameObject.GetComponent<CardFlip>().LateFlip(2f);
        }
    }

    public override void Update()
    {
        if (!_hasExecuted)
        {
            _timer += Time.deltaTime;

            if (_timer >= 2.5f)
            {
                // 타이틀 이미지 변경
                _view._titleImage.sprite = _view._titleImageList[1];
                _view._titleImage.SetNativeSize();
                _view._titleImage.rectTransform.sizeDelta /= 4f;

                // 타이머 시작
                StartTimer();

                _hasExecuted = true;
            }
        }

        if (false == _isRunning) return;

        // 타이머 감소
        _remainingTime -= Time.deltaTime;

        // 강제 종료
        if (Input.GetKey(KeyCode.Space))
        {
            _remainingTime = 0f;
        }

        // 화면 업데이트
        UpdateTimerUI();

        // 시간 초과 시 게임 종료
        if (_remainingTime <= 0)
        {
            HandleGameClear();
            _remainingTime = 0f;
            _isRunning = false;
        }
    }

    public override void Exit()
    {
        base.Exit();

        // 이벤트 해제 (CardGameManager)
        CardGameManager.Instance._OnMatchSuccess -= HandleMatchSuccess;
        CardGameManager.Instance._OnMatchFail -= HandleMatchFail;
        CardGameManager.Instance._OnGameClear -= HandleGameClear;

        // 게임 초기화
        CardGameManager.Instance.ResetGame();

        // 카드 상태 초기화
        CardInit();

        // 팝업창 닫기
        _view._popupContainerObj.SetActive(false);

        // 타이머 초기화
        ResetTimer();

        // 타이틀 이미지 복원
        _view._titleImage.sprite = _view._titleImageList[0];
        _view._titleImage.SetNativeSize();
        _view._titleImage.rectTransform.sizeDelta /= 4f;

        _timer = 0f;
        _hasExecuted = false;

        _view.Hide();
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
        _view._timerText.text = $"{minutes:00}:{seconds:00}";
    }

    private void CardSet()
    {
        int idx = 0;
        foreach (Image card in _view._cardList)
        {
            if (null == card) continue;

            if (!card.TryGetComponent(out CardFlip flipComp))
            {
                flipComp = card.gameObject.AddComponent<CardFlip>();
            }
            flipComp._backSprite = _view._cardBackSprite;
            flipComp._cardIdx = idx;

            // 카드 클릭 콜백 등록
            flipComp._OnClickCard += HandleClickCard;

            ++idx;
        }

        // 카드 앞면 이미지 할당
        var deck = CardGameManager.Instance.GetCurrentDeck();
        for (int i = 0; i < deck.Count; i++)
        {
            CardFlip flip = _view._cardList[i].gameObject.GetComponent<CardFlip>();
            flip._frontSprite = deck[i]._cardSprite;
        }
    }

    private void CardInit()
    {
        foreach (Image card in _view._cardList)
        {
            if (null == card) continue;
            CardFlip flipComp = card.GetComponent<CardFlip>();

            flipComp.Restore();
            flipComp._OnClickCard -= HandleClickCard;
        }
    }

    public void HandleClickCard(int index)
    {
        CardGameManager.Instance.SelectCard(index);
    }

    void HandleMatchSuccess(int index1, int index2)
    {
        Debug.Log($"매칭 성공! {index1}, {index2}");

        _view._cardList[index1].raycastTarget = false;
        _view._cardList[index1].color = new Color(0.5f, 0.5f, 0.5f, 1f);
        _view._cardList[index2].raycastTarget = false;
        _view._cardList[index2].color = new Color(0.5f, 0.5f, 0.5f, 1f);
    }

    void HandleMatchFail(int index1, int index2)
    {
        Debug.Log($"매칭 실패! {index1}, {index2}");

        _view._cardList[index1].gameObject.GetComponent<CardFlip>().Flip();
        _view._cardList[index2].gameObject.GetComponent<CardFlip>().Flip();
    }

    void HandleGameClear()
    {
        Debug.Log("게임 클리어!");

        _view._popupContainerObj.SetActive(true);

        if (false == _isRunning)
        {
            CardGameManager.Instance._isSuccess = false;
        }

        _view._infoText.text =
        CardGameManager.Instance._isSuccess ?
        "주어진 시간안에\n문제를 해결했습니다!" :
        "주어진 시간안에\n문제를 해결하지 못했습니다!";

        int minutes = Mathf.FloorToInt(_remainingTime / 60);
        int seconds = Mathf.FloorToInt(_remainingTime % 60);
        _view._recordText.text =
            CardGameManager.Instance._isSuccess ?
            $"기록 : {minutes:00}:{seconds:00}" :
            "기록 : x";

        StopTimer();
    }

    public void HandleRetryButton()
    {
        CardGameManager.Instance.ResetGame();
        CardInit();
        _view._popupContainerObj.SetActive(false);
        ResetTimer();
        _view._titleImage.sprite = _view._titleImageList[0];
        _view._titleImage.SetNativeSize();
        _view._titleImage.rectTransform.sizeDelta /= 4f;
        _timer = 0f;
        _hasExecuted = false;

        CardGameManager.Instance.StartGame();
        CardSet();
        foreach (Image cardImage in _view._cardList)
        {
            cardImage.gameObject.GetComponent<CardFlip>().Flip();
        }
        foreach (Image cardImage in _view._cardList)
        {
            cardImage.gameObject.GetComponent<CardFlip>().LateFlip(2f);
        }
    }
}
