using UnityEngine;

public class HomeState : BaseState
{
    private HomeView _homeView;

    public HomeState(HomeView view)
    {
        _homeView = view;

        // 이벤트 등록
        _homeView._OnLeftButtonClicked += () => { NavigationController.Instance.GoToSelect(); };
        _homeView._OnRightButtonClicked += () => { NavigationController.Instance.GoToNumGameDescription(ECountry.Egypt); };
    }

    public override void Enter()
    {
        Debug.Log("[HomeState] Enter");
        _homeView.Show();
    }

    public override void Update()
    {
        //
    }

    public override void Exit()
    {
        Debug.Log("[HomeState] Eixt");
        _homeView.Hide();
    }

}
