using UnityEngine;

public class HomeState : BaseState<HomeState, HomeView>
{
    public HomeState(HomeView view) : base(view) { }

    public override void Init()
    {
        base.Init();

        // 이벤트 구독 (최초 1회)
        _view._OnLeftButtonClicked += () => { NavigationController.Instance.GoToSelect(); };
        _view._OnRightButtonClicked += () => { NavigationController.Instance.GoToNumGameDescription(ECountry.Egypt); };
    }

    public override void Enter()
    {
        base.Enter();
        _view.Show();
    }

    public override void Exit()
    {
        base.Exit();
        _view.Hide();
    }
}
