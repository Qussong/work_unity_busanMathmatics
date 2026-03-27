using BusanMath.Core;
using BusanMath.FSM;
using BusanMath.FSM.States;
using BusanMath.Views;
using BusanMath.Managers;
using BusanMath.Models;
using UnityEngine;

namespace BusanMath.Controllers
{
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
        [SerializeField] private WriteView _writeView;
        [SerializeField] private DrawingView _drawingView;
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
            StateMachine.AddState(new WriteState(_writeView));
            StateMachine.AddState(new DrawingState(_drawingView));
            StateMachine.AddState(new VoteResultState(_voteResultView));

            StateMachine.InitializeAllStates();

            StateMachine.OnStateChanged += HandleStateChanged;
            StateMachine.ChangeState<HomeState>();
        }

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
            _drawingView.EnsureInitialized();
            _voteResultView.EnsureInitialized();
        }

        #endregion

        #region Navigation

        public void GoToHome()
        {
            StateMachine.ChangeState<HomeState>();
        }

        public void GoToSelect(bool skipToButtons = false)
        {
            StateMachine.GetState<SelectState>().SkipToButtons = skipToButtons;
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
            StateMachine.GetState<WriteState>().Country = country;
            StateMachine.ChangeState<WriteState>();
        }

        public void GoToDrawing(ECountry country, int year, int month, int day)
        {
            var state = StateMachine.GetState<DrawingState>();
            state.Country = country;
            state.SelectedYear = year;
            state.SelectedMonth = month;
            state.SelectedDay = day;
            StateMachine.ChangeState<DrawingState>();
        }

        public void GoToVoteResult()
        {
            StateMachine.ChangeState<VoteResultState>();
        }

        #endregion

        #region Query

        public bool IsHomeState() => StateMachine.IsCurrentState<HomeState>();
        public bool IsSelectState() => StateMachine.IsCurrentState<SelectState>();

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
}
