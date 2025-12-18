using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class VoteResultState : BaseState
{
    private VoteResultView _voteResultView;
    private List<ECountry> _rankList;

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

        // 국가별 순위 설정
        _rankList = VoteManager.Instance.GetRanking();

        // 순위에 따른 텍스트 설정
        SetRankCountry();

        // 순위 이미지 설정
        SetCountryView();

        // 순위 투표비율과 투표수 설정
        SetCountryVoteRateAndCount();

        // 순위 투표 Rate bar 설정
        SetCountryVoteRateBar();

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

    /// <summary>
    /// 순위에 따른 텍스트 설정
    /// </summary>
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
            _voteResultView._rankCountryList[i].text = country;
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
                    view = _voteResultView._countryViewSpriteList[(int)ECountry.Egypt];
                    break;
                case ECountry.China:
                    view = _voteResultView._countryViewSpriteList[(int)ECountry.China];
                    break;
                case ECountry.Roma:
                    view = _voteResultView._countryViewSpriteList[(int)ECountry.Roma];
                    break;
            }
            _voteResultView._countryViewList[i].sprite = view;
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

            _voteResultView._votePercentList[i].text = rate.ToString("F1") + "%";
            _voteResultView._voteCountList[i].text = count.ToString() + "표";
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

            _voteResultView._voteRateBarList[i].value = rate;
        }
    }


}
