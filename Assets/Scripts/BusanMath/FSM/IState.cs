namespace BusanMath.FSM
{
    public interface IState
    {
        void Init();    // 최초 1회 초기화 (이벤트 구독 등)
        void Enter();   // 상태 진입 시 매번 호출
        void Update();
        void Exit();    // 상태 이탈 시 매번 호출
        void Dispose(); // 이벤트 해제 (프로그램 종료 시)
    }
}
