using UnityEngine;

public class NumGameState : BaseState
{
    private NumGameView _numGameView;
    private ECountry _country;

    public ECountry Country
    {
        set
        {
            _country = value;
        }
    }

    public NumGameState(NumGameView view)
    {
        _numGameView = view;

        // 이벤트 등록
        _numGameView._OnHomeButtonClicked += () => { NavigationController.Instance.GoToHome(); };
        _numGameView._OnMoveNextButtonClicked += () => { NavigationController.Instance.GoToCardGameDescription(); };
        _numGameView._OnOtherCountryButtonClicked += () => { NavigationController.Instance.GoToSelect(); };
    }

    public override void Enter()
    {
        Debug.Log("[NumGameState] Enter");
        _numGameView.Show();
    }

    public override void Update()
    {
        //
    }

    public override void Exit()
    {
        Debug.Log("[NumGameState] Eixt");

        // 국가 정보 초기화
        _country = ECountry.None;

        _numGameView.Hide();
    }
}
