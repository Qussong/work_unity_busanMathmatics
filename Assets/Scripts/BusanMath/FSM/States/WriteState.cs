using UnityEngine;

public class WriteState : BaseState
{
    private WriteView _writeView;
    private ECountry _country;

    public ECountry Country
    {
        set
        {
            _country = value;
        }
    }

    public WriteState(WriteView view)
    {
        _writeView = view;

        // 이벤트 등록
        _writeView._OnHomeButtonClicked += () => { NavigationController.Instance.GoToHome(); };
        _writeView._OnOkayButtonClicked += () => { 
            // Writing Board 등장
        };
        _writeView._OnMoveNextButtonClicked += () => { NavigationController.Instance.GoToVoteResult(); };
    }

    public override void Enter()
    {
        Debug.Log("[WriteState] Enter");

        // 선택된 국가에 맞는 배경 이미지 설정
        _writeView._backgroundImage.sprite = _writeView._backgroundSpriteList[(int)_country];

        // 선택된 국가에 맞는 타이틀 이미지 설정
        _writeView._titleImage.sprite = _writeView._titleSpriteList[(int)_country];

        _writeView.Show();
    }

    public override void Update()
    {
        //
    }

    public override void Exit()
    {
        Debug.Log("[WriteState] Eixt");

        // 국가 선택 기록 초기화
        _country = ECountry.None;

        // 배경 이미지 설정 해제
        _writeView._backgroundImage.sprite = null;

        // 타이틀 이미지 설정 해제
        _writeView._titleImage.sprite = null;

        _writeView.Hide();
    }
}
