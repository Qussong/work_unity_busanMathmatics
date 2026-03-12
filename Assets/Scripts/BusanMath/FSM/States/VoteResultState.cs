using System.Collections.Generic;
using UnityEngine;

public class VoteResultState : BaseState<VoteResultState, VoteResultView>
{
    private List<ECountry> _rankList;

    public VoteResultState(VoteResultView view) : base(view) { }

    public override void Init()
    {
        base.Init();

        // 이벤트 구독 (최초 1회)
        _view._OnHomeButtonClicked += () => { NavigationController.Instance.GoToHome(); };
    }

    public override void Enter()
    {
        base.Enter();
        _view.Show();

        // 랭킹 데이터 세팅
        _rankList = VoteManager.Instance.GetRanking();

        SetRankCountry();
        SetCountryView();
        SetCountryVoteRateAndCount();
        SetCountryVoteRateBar();
    }

    public override void Exit()
    {
        base.Exit();
        _view.Hide();
    }

    private void SetRankCountry()
    {
        for (int i = 0; i < (int)ECountry.MAX_CNT; ++i)
        {
            string country = "";

            switch (_rankList[i])
            {
                case ECountry.Egypt:
                    country = "이집트";
                    break;
                case ECountry.China:
                    country = "중국";
                    break;
                case ECountry.Roma:
                    country = "로마";
                    break;
            }
            _view._rankCountryList[i].text = country;
        }
    }

    private void SetCountryView()
    {
        for (int i = 0; i < (int)ECountry.MAX_CNT; ++i)
        {
            Sprite view = null;

            switch (_rankList[i])
            {
                case ECountry.Egypt:
                    view = _view._countryViewSpriteList[(int)ECountry.Egypt];
                    break;
                case ECountry.China:
                    view = _view._countryViewSpriteList[(int)ECountry.China];
                    break;
                case ECountry.Roma:
                    view = _view._countryViewSpriteList[(int)ECountry.Roma];
                    break;
            }
            _view._countryViewList[i].sprite = view;
        }
    }

    private void SetCountryVoteRateAndCount()
    {
        for (int i = 0; i < (int)ECountry.MAX_CNT; ++i)
        {
            float rate = 0f;
            int count = 0;

            switch (_rankList[i])
            {
                case ECountry.Egypt:
                    rate = VoteManager.Instance.GetRate(ECountry.Egypt) * 100;
                    count = VoteManager.Instance.GetData().voteEgypt;
                    break;
                case ECountry.China:
                    rate = VoteManager.Instance.GetRate(ECountry.China) * 100;
                    count = VoteManager.Instance.GetData().voteChina;
                    break;
                case ECountry.Roma:
                    rate = VoteManager.Instance.GetRate(ECountry.Roma) * 100;
                    count = VoteManager.Instance.GetData().voteRoma;
                    break;
            }

            _view._votePercentList[i].text = rate.ToString("F1") + "%";
            _view._voteCountList[i].text = count.ToString() + "표";
        }
    }

    private void SetCountryVoteRateBar()
    {
        for (int i = 0; i < (int)ECountry.MAX_CNT; ++i)
        {
            float rate = 0f;

            switch (_rankList[i])
            {
                case ECountry.Egypt:
                    rate = VoteManager.Instance.GetRate(ECountry.Egypt);
                    break;
                case ECountry.China:
                    rate = VoteManager.Instance.GetRate(ECountry.China);
                    break;
                case ECountry.Roma:
                    rate = VoteManager.Instance.GetRate(ECountry.Roma);
                    break;
            }

            _view._voteRateBarList[i].value = rate;
        }
    }
}
