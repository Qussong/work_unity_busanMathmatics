using BusanMath.Core;
using UnityEngine;

/// <summary>
/// 화면 전환을 담당하는 싱글톤 컨트롤러
/// StateMachine을 통해 각 화면(State)의 생명주기를 관리한다.
/// </summary>
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
    // [SerializeField] private WriteView _writeView;
    [SerializeField] private Write2View _writeView;
    [SerializeField] private VoteResultView _voteResultView;

    public StateMachine StateMachine { get; private set; }

    #region Lifecycle

    protected override void OnSingletonAwake()
    {
        InitializeStateMachine();
    }

    protected override void OnDestroy()
    {
        if (StateMachine != null)
        {
            StateMachine.OnStateChanged -= HandleStateChanged;
        }

        base.OnDestroy();
    }

    #endregion

    #region Initialize

    private void InitializeStateMachine()
    {
        EnsureAllViewsInitialized();

        StateMachine = gameObject.AddComponent<StateMachine>();

        StateMachine.AddState(new HomeState(_homeView));
        StateMachine.AddState(new SelectState(_selectView));
        StateMachine.AddState(new VideoState(_videoView));
        StateMachine.AddState(new NumGameDescriptionState(_numGameDescriptionView));
        StateMachine.AddState(new NumGameState(_numGameView));
        StateMachine.AddState(new CardGameDescriptionState(_cardGameDescriptionView));
        StateMachine.AddState(new CardGameState(_cardGameView));
        StateMachine.AddState(new VoteState(_voteView));
        StateMachine.AddState(new Write2State(_writeView));
        StateMachine.AddState(new VoteResultState(_voteResultView));

        StateMachine.InitializeAllStates();

        StateMachine.OnStateChanged += HandleStateChanged;
        StateMachine.ChangeState<HomeState>();
    }

    /// <summary>
    /// 비활성화된 View의 Initialize/BindUIEvent가 누락되지 않도록
    /// StateMachine 초기화 전에 모든 View를 명시적으로 초기화한다.
    /// </summary>
    private void EnsureAllViewsInitialized()
    {
        _homeView.EnsureInitialized();
        _selectView.EnsureInitialized();
        _videoView.EnsureInitialized();
        _numGameDescriptionView.EnsureInitialized();
        _numGameView.EnsureInitialized();
        _cardGameDescriptionView.EnsureInitialized();
        _cardGameView.EnsureInitialized();
        _voteView.EnsureInitialized();
        _writeView.EnsureInitialized();
        _voteResultView.EnsureInitialized();
    }

    #endregion

    #region Navigation

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
        StateMachine.GetState<VideoState>().Country = country;
        StateMachine.ChangeState<VideoState>();
    }

    public void GoToNumGameDescription(ECountry country)
    {
        StateMachine.GetState<NumGameDescriptionState>().Country = country;
        StateMachine.ChangeState<NumGameDescriptionState>();
    }

    public void GoToNumGame(ECountry country)
    {
        StateMachine.GetState<NumGameState>().Country = country;
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
        StateMachine.GetState<Write2State>().Country = country;
        StateMachine.ChangeState<Write2State>();
    }

    public void GoToVoteResult()
    {
        StateMachine.ChangeState<VoteResultState>();
    }

    #endregion

    #region Query

    public bool IsHome() => StateMachine.IsCurrentState<HomeState>();

    #endregion

    #region Event Handlers

    private void HandleStateChanged(IState oldState, IState newState)
    {
        if (IdleManager.Instance != null)
        {
            IdleManager.Instance.ResetTimer();
        }
    }

    #endregion
}
