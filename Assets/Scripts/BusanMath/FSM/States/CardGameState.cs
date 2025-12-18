using UnityEngine;

public class CardGameState : BaseState
{
    private CardGameView _cardGameView;

    public CardGameState(CardGameView view)
    {
        _cardGameView = view;

        // 이벤트 등록
        _cardGameView._OnHomeButtonClicked += () => { NavigationController.Instance.GoToHome(); };
        _cardGameView._OnMoveNextClicked += () => { NavigationController.Instance.GoToVote(); };
    }

    public override void Enter()
    {
        Debug.Log("[CardGameState] Enter");
        _cardGameView.Show();
    }

    public override void Update()
    {
        //
    }

    public override void Exit()
    {
        Debug.Log("[CardGameState] Eixt");
        _cardGameView.Hide();
    }

}
