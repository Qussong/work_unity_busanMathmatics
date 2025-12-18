using UnityEngine;

public class VoteResultState : BaseState
{
    private VoteResultView _voteResultView;

    public VoteResultState(VoteResultView view)
    {
        _voteResultView = view;

        // 이벤트 등록
        _voteResultView._OnHomeButtonClicked += () => { NavigationController.Instance.GoToHome(); };
    }

    public override void Enter()
    {
        Debug.Log("[VoteResultState] Enter");
        _voteResultView.Show();
    }

    public override void Update()
    {
        //
    }

    public override void Exit()
    {
        Debug.Log("[VoteResultState] Eixt");
        _voteResultView.Hide();
    }
}
