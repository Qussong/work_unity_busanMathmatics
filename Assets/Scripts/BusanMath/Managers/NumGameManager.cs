using BusanMath.Core;
using BusanMath.Models;
using UnityEngine;

namespace BusanMath.Managers
{
    /// <summary>
    /// 숫자 맞추기 게임 로직 관리
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

        public void InitGame()
        {
            _country = ECountry.None;
            _rndNum = "";
            InitAnswer();
        }

        public void StartGame(ECountry country)
        {
            _country = country;
            SetRndNum();
        }

        public void SetRndNum()
        {
            if (_country == ECountry.Egypt)
            {
                _rndNum = _egyptNumContainer.GetRandom().Key;
            }
            else if (_country == ECountry.China)
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
}
