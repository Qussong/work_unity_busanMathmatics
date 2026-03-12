using UnityEngine;

public class VoteState : BaseState<VoteState, VoteView>
{
    public VoteState(VoteView view) : base(view) { }

    public override void Init()
    {
        base.Init();

        // 이벤트 구독 (최초 1회)
        _view._OnHomeButtonClicked += () => { NavigationController.Instance.GoToHome(); };
        _view._OnEgyptButtonClicked += () => { VoteCountry(ECountry.Egypt); };
        _view._OnChinaButtonClicked += () => { VoteCountry(ECountry.China); };
        _view._OnRomaButtonClicked += () => { VoteCountry(ECountry.Roma); };
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

    private void VoteCountry(ECountry choice)
    {
        Debug.Log($"vote : {choice.ToString()}");
        VoteManager.Instance.Vote(choice);
        NavigationController.Instance.GoToWrite(choice);
    }
}
