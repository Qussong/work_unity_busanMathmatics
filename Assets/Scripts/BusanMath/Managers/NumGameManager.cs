using BusanMath.Core;
using UnityEngine;

/// <summary>
/// 숫자 맞추기 게임 로직 관리
/// 국가별 랜덤 숫자 생성 및 정답 비교
/// </summary>
public class NumGameManager : MonoSingleton<NumGameManager>
{
    [Header("DB")]
    [SerializeField] private StringSpritePairContainerSO _egyptNumContainer;
    [SerializeField] private StringSpritePairContainerSO _romaNumContainer;

    private ECountry _country;
    private string _rndNum;
    public string RndNum => _rndNum;

    private string _answer = "";
    public string Answer => _answer;

    protected override void OnSingletonAwake()
    {
    }

    /// <summary>
    /// 게임 초기화 (국가, 랜덤 숫자, 정답 모두 리셋)
    /// </summary>
    public void InitGame()
    {
        _country = ECountry.None;
        _rndNum = "";
        InitAnswer();
    }

    /// <summary>
    /// 게임 시작 - 국가 설정 및 랜덤 숫자 생성
    /// </summary>
    public void StartGame(ECountry country)
    {
        _country = country;
        SetRndNum();
    }

    /// <summary>
    /// 국가별 랜덤 숫자 생성
    /// 이집트/로마: ScriptableObject에서 랜덤 선택
    /// 중국: 2~3자리 랜덤 숫자 생성
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

    private string GetRandom2To3Digit()
    {
        return UnityEngine.Random.Range(10, 1000).ToString();
    }

    /// <summary>
    /// 현재 랜덤 숫자에 해당하는 스프라이트 반환 (이집트, 로마)
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
    /// 현재 랜덤 숫자를 한자 문자열로 변환 (중국)
    /// </summary>
    public string GetRndNumToHanJa()
    {
        string[] hanja = { "\u96f6", "\u58f9", "\u8cb3", "\u53c3", "\u8086", "\u4f0d", "\u9678", "\u67d2", "\u634c", "\u7396" };

        int num = int.Parse(_rndNum);

        if (num < 10)
            return hanja[num];

        if (num < 100)
        {
            int tens = num / 10;
            int ones = num % 10;

            string result = hanja[tens] + "\u62fe";
            if (ones > 0)
                result += hanja[ones];
            return result;
        }

        int hundreds = num / 100;
        int tensDigit = (num % 100) / 10;
        int onesDigit = num % 10;

        string output = hanja[hundreds] + "\u767e";

        if (tensDigit > 0)
            output += hanja[tensDigit] + "\u62fe";

        if (onesDigit > 0)
            output += hanja[onesDigit];

        return output;
    }

    /// <summary>
    /// 숫자 타일 선택 시 정답에 추가
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

    public void InitAnswer()
    {
        _answer = "";
    }

    public bool CompareAnswerAndRndNum()
    {
        return _rndNum == _answer;
    }
}
