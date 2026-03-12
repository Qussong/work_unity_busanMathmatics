using BusanMath.FSM;
using BusanMath.Views;
using BusanMath.Controllers;
using BusanMath.Managers;
using BusanMath.Models;

namespace BusanMath.FSM.States
{
    public class DrawingState : BaseState<DrawingState, DrawingView>
    {
        public DrawingState(DrawingView view) : base(view) { }

        public override void Init()
        {
            base.Init();

            _view._OnHomeButtonClicked += () => NavigationController.Instance.GoToHome();
            _view._OnMoveNextButtonClicked += () => NavigationController.Instance.GoToVoteResult();
        }

        public override void Enter()
        {
            base.Enter();
            _view.Show();
            _view._writeBoardContainer.SetActive(true);
        }

        public override void Exit()
        {
            base.Exit();
            _view._drawTextureUI.Clear();
            _view._writeBoardContainer.SetActive(false);
            _view.Hide();
        }
    }
}
