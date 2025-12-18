

public abstract class BaseState : IState
{
    public virtual void Enter()
    {
        // 화면 진입시 필요한 로직
    }

    public virtual void Update()
    {
        // 화면에서 필요한 매 프레임 로직
    }

    public virtual void Exit()
    {
        // 화면 탈출시 필요한 로직
    }

}
