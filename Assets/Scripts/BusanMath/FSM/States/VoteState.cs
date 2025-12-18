
using System.Collections;
using UnityEngine;

public class VoteState : BaseState
{
    private VoteView _voteView;

    public VoteState(VoteView view)
    {
        _voteView = view;

        // 이벤트 등록
        _voteView._OnHomeButtonClicked += () => { NavigationController.Instance.GoToHome(); };
        _voteView._OnEgyptButtonClicked += () => { VoteCountry(ECountry.Egypt); };
        _voteView._OnChinaButtonClicked += () => { VoteCountry(ECountry.China); };
        _voteView._OnRomaButtonClicked += () => { VoteCountry(ECountry.Roma); };
    }

    public override void Enter()
    {
        Debug.Log("[VoteState] Enter");
        _voteView.Show();
    }

    public override void Update()
    {
        //
    }

    public override void Exit()
    {
        Debug.Log("[VoteState] Eixt");
        _voteView.Hide();
    }

    private void VoteCountry(ECountry voteCountry)
    {
        Debug.Log($"vote : {voteCountry.ToString()}");

        // 외부 파일에 데이터 누적

        // WriteView 로 이동
        NavigationController.Instance.GoToWrite(voteCountry);
    }

}
