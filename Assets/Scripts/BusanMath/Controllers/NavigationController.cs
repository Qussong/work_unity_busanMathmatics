using BusanMath.Core;
using System.Collections;
using UnityEngine;

public class NavigationController : MonoSingleton<NavigationController>
{
    [Header("Views")]
    [SerializeField] private HomeView _homeView;
    [SerializeField] private SelectView _selectView;
    [SerializeField] private VideoView _videoView;
    [SerializeField] private NumGameDescriptionView _numGameDescriptionView;
    [SerializeField] private NumGameView _numGameView;
    [SerializeField] private CardGameDescriptionView _cardGameDescriptionView;
    [SerializeField] private CardGameView _cardGameView;
    [SerializeField] private VoteView _voteView;
    [SerializeField] private WriteView _writeView;
    [SerializeField] private VoteResultView _voteResultView;

    public StateMachine StateMachine { get; private set; }

    /// <summary>
    /// 싱글톤 초기화 시 호출되는 가상 메서드
    /// 서브클래스에서 오버라이드하여 초기화 로직 구현 
    /// </summary>
    protected override void OnSingletonAwake()
    {
        InitializeStateMachine();
    }

    private void InitializeStateMachine()
    {
        // StateMachine 컴포넌트 추가
        StateMachine = gameObject.AddComponent<StateMachine>();

        // 상태 등록
        StateMachine.AddState(new HomeState(_homeView));
        StateMachine.AddState(new SelectState(_selectView));
        StateMachine.AddState(new VideoState(_videoView));
        StateMachine.AddState(new NumGameDescriptionState(_numGameDescriptionView));
        StateMachine.AddState(new NumGameState(_numGameView));
        StateMachine.AddState(new CardGameDescriptionState(_cardGameDescriptionView));
        StateMachine.AddState(new CardGameState(_cardGameView));
        StateMachine.AddState(new VoteState(_voteView));
        StateMachine.AddState(new WriteState(_writeView));
        StateMachine.AddState(new VoteResultState(_voteResultView));

        // 상태 변경 이벤트 구독
        StateMachine.OnStateChanged += HandleStateChanged;

        // 초기 상태: Home
        StateMachine.ChangeState<HomeState>();
    }

    private void HandleStateChanged(IState oldState, IState newState)
    {
        // 상태 변경 시 추가 처리

        // IdleManager 타이머 리셋
        IdleManager.Instance?.ResetTimer();
    }

    //=== 외부에서 호출하는 네비게이션 메서드 ===

    public void GoToHome()
    {
        StateMachine.ChangeState<HomeState>();
    }

    public void GoToSelect()
    {
        StateMachine.ChangeState<SelectState>();
    }

    public void GoToVideo(ECountry country)
    {
        var videoState = StateMachine.GetState<VideoState>();
        videoState.Country = country;
        StateMachine.ChangeState<VideoState>();
    }

    public void GoToNumGameDescription(ECountry country)
    {
        var numGameDescriptionState = StateMachine.GetState<NumGameDescriptionState>();
        numGameDescriptionState.Country = country;
        StateMachine.ChangeState<NumGameDescriptionState>();
    }

    public void GoToNumGame(ECountry country)
    {
        var numGameState = StateMachine.GetState<NumGameState>();
        numGameState.Country = country;
        StateMachine.ChangeState<NumGameState>();
    }

    public void GoToCardGameDescription()
    {
        StateMachine.ChangeState<CardGameDescriptionState>();
    }

    public void GoToCardGame()
    {
        StateMachine.ChangeState<CardGameState>();
    }

    public void GoToVote()
    {
        StateMachine.ChangeState<VoteState>();
    }

    public void GoToWrite(ECountry country)
    {
        var writeState = StateMachine.GetState<WriteState>();
        writeState.Country = country;
        StateMachine.ChangeState<WriteState>();
    }

    public void GoToVoteResult()
    {
        StateMachine.ChangeState<VoteResultState>();
    }

    //현재 상태 확인
    public bool IsHome() => StateMachine.IsCurrentState<HomeState>();

    protected override void OnDestroy()
    {
        if (StateMachine != null)
        {
            StateMachine.OnStateChanged -= HandleStateChanged;
        }
    }

}