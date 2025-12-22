using BusanMath.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class NumGameManager : MonoSingleton<NumGameManager>
{
    [Header("DB")]
    [SerializeField] private StringSpritePairContainerSO _egyptNumContainer;
    //[SerializeField] private StringSpritePairContainerSO _chinaNumContainer;
    [SerializeField] private StringSpritePairContainerSO _romaNumContainer;

    //
    private ECountry _country;  // 나라
    private string _rndNum;     // 랜덤값
    public string RndNum => _rndNum;

    //
    private string _answer = "";    // 정답 제출현황
    public string Answer => _answer;

    protected override void OnSingletonAwake()
    {

    }

    /// <summary>
    /// 값 초기화
    /// </summary>
    public void InitGame()
    {
        // 나라 초기화
        _country = ECountry.None;

        // 랜덤값 초기화
        _rndNum = "";

        // 정답 초기화
        InitAnswer();
    }

    /// <summary>
    /// 게임 시작
    /// </summary>
    public void StartGame(ECountry country)
    {
        // 나라설정
        _country = country;

        // 랜덤값 설정
        SetRndNum();
    }

    /// <summary>
    /// 나라별 랜덤값 설정
    /// </summary>
    public void SetRndNum()
    {

        if (_country == ECountry.Egypt)
        {
            _rndNum = _egyptNumContainer.GetRandom().Key;
        }
        else if(_country == ECountry.China)
        {
            _rndNum = GetRandom2To3Digit();
        }
        else if (_country == ECountry.Roma)
        {
            _rndNum = _romaNumContainer.GetRandom().Key;
        }
    }

    /// <summary>
    /// 중국용 랜덤값 생성
    /// 2~3자리 수 생성
    /// </summary>
    private string GetRandom2To3Digit()
    {
        return UnityEngine.Random.Range(10, 1000).ToString();
    }

    /// <summary>
    /// 선정된 랜덤 수에 해당하는 스프라이트 반환
    /// </summary>
    public Sprite GetRndNumSprite()
    {
        Sprite result = null;

        if (_country == ECountry.Egypt)
        {
            result = _egyptNumContainer.GetSprite(_rndNum);
        }
        else if (_country == ECountry.Roma)
        {
            result = _romaNumContainer.GetSprite(_rndNum);
        }

        return result;
    }
    
    /// <summary>
    /// 선정된 랜덤 수에 해당하는 한자 수 반환
    /// </summary>
    public string GetRndNumToHanJa()
    {
        string[] hanja = { "零", "一", "二", "三", "四", "五", "六", "七", "八", "九" };

        int num = int.Parse(_rndNum);

        // 한 자리
        if (num < 10)
            return hanja[num];

        // 두 자리 (10 ~ 99)
        if (num < 100)
        {
            int tens = num / 10;
            int ones = num % 10;

            string result = hanja[tens] + "十";
            if (ones > 0)
                result += hanja[ones];
            return result;
        }

        // 세 자리 (100 ~ 999)
        int hundreds = num / 100;
        int tensDigit = (num % 100) / 10;
        int onesDigit = num % 10;

        string output = hanja[hundreds] + "百";

        if (tensDigit > 0)
            output += hanja[tensDigit] + "十";

        if (onesDigit > 0)
            output += hanja[onesDigit];

        return output;
    }

    /// <summary>
    /// 숫자 타일 선택
    /// </summary>
    public bool SelectNumTile(int num)
    {
        if (_rndNum.Length > _answer.Length)
        {
            _answer += num.ToString();
            return true;
        }
        return false;
    }

    // 정답 초기화
    public void InitAnswer()
    {
        _answer = "";
    }

    public bool CompareAnswerAndRndNum()
    {
        return _rndNum == _answer;
    }


}
