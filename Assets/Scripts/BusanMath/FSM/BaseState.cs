using UnityEngine;
using BusanMath.Views;

namespace BusanMath.FSM
{
    /// <summary>
    /// 제네릭 상태 베이스 클래스
    /// TState: 자기 자신 타입 (로그용), TView: 대응하는 View 타입
    /// </summary>
    public abstract class BaseState<TState, TView> : IState where TView : BaseView
    {
        protected TView _view;

        public BaseState(TView view)
        {
            _view = view;
        }

        /// <summary>
        /// 최초 1회 초기화 (View 이벤트 구독 등)
        /// StateMachine이 HashSet으로 추적하여 중복 호출 방지
        /// </summary>
        public virtual void Init()
        {
            Debug.Log($"[{typeof(TState).Name}] Init");
        }

        public virtual void Enter()
        {
            Debug.Log($"[{typeof(TState).Name}] Enter");
        }

        public virtual void Update()
        {
        }

        public virtual void Exit()
        {
            Debug.Log($"[{typeof(TState).Name}] Exit");
        }

        /// <summary>
        /// 이벤트 구독 해제 (프로그램 종료 시)
        /// </summary>
        public virtual void Dispose()
        {
        }
    }
}
