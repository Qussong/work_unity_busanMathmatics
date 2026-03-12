using UnityEngine;
using BusanMath.FSM;
using BusanMath.Views;
using BusanMath.Controllers;
using BusanMath.Managers;
using BusanMath.Models;

namespace BusanMath.FSM.States
{
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
}
