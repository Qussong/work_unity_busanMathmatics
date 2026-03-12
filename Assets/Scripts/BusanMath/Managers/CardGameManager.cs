using BusanMath.Core;
using BusanMath.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BusanMath.Managers
{
    /// <summary>
    /// 카드 매칭 게임 로직 관리
    /// </summary>
    public class CardGameManager : MonoSingleton<CardGameManager>
    {
        [SerializeField] private CardDatabaseSO _cardDatabase;

        private List<CardData> _currentDeck;
        private int _firstSelectedIndex = -1;
        private int _secondSelectedIndex = -1;
        private int _matchCount;
        private const int _MAX_MATCH = 6;
        public bool _isSuccess = true;

        public event Action<int, int> _OnMatchSuccess;
        public event Action<int, int> _OnMatchFail;
        public event Action _OnGameClear;

        protected override void OnSingletonAwake()
        {
        }

        public void StartGame()
        {
            _currentDeck = CreateShuffledCards();
            _firstSelectedIndex = -1;
            _matchCount = 0;
        }

        public List<CardData> CreateShuffledCards()
        {
            List<CardData> selected = _cardDatabase.cards
                .OrderBy(_ => UnityEngine.Random.value)
                .Take(6)
                .ToList();

            List<CardData> deck = new List<CardData>();
            deck.AddRange(selected);
            deck.AddRange(selected);

            return deck.OrderBy(_ => UnityEngine.Random.value).ToList();
        }

        public void SelectCard(int index)
        {
            if (_firstSelectedIndex == index) return;

            if (_firstSelectedIndex == -1)
            {
                _firstSelectedIndex = index;
                return;
            }

            _secondSelectedIndex = index;
            bool isMatch = Compare(_firstSelectedIndex, _secondSelectedIndex);

            if (isMatch)
            {
                _matchCount++;
                StartCoroutine(DelayedCallMatchSucess(_firstSelectedIndex, _secondSelectedIndex, 0.5f));

                if (_matchCount >= _MAX_MATCH)
                {
                    _isSuccess = true;
                    StartCoroutine(DelayedCallGameClear(1f));
                }
            }
            else
            {
                StartCoroutine(DelayedCallMatchFail(_firstSelectedIndex, _secondSelectedIndex, 0.5f));
            }

            _firstSelectedIndex = -1;
            _secondSelectedIndex = -1;
        }

        private IEnumerator DelayedCallMatchSucess(int firstIdx, int secondIdx, float delay)
        {
            yield return new WaitForSeconds(delay);
            _OnMatchSuccess?.Invoke(firstIdx, secondIdx);
        }

        private IEnumerator DelayedCallMatchFail(int firstIdx, int secondIdx, float delay)
        {
            yield return new WaitForSeconds(delay);
            _OnMatchFail?.Invoke(firstIdx, secondIdx);
        }

        private IEnumerator DelayedCallGameClear(float delay)
        {
            yield return new WaitForSeconds(delay);
            _OnGameClear?.Invoke();
        }

        private bool Compare(int index1, int index2)
        {
            if (_currentDeck == null) return false;
            if (index1 < 0 || index1 >= _currentDeck.Count) return false;
            if (index2 < 0 || index2 >= _currentDeck.Count) return false;

            return _currentDeck[index1]._value == _currentDeck[index2]._value
                && _currentDeck[index1]._country == _currentDeck[index2]._country;
        }

        public CardData GetCard(int index) => _currentDeck[index];
        public List<CardData> GetCurrentDeck() => _currentDeck;
        public int GetMatchCount() => _matchCount;

        public void ResetGame()
        {
            StopAllCoroutines();

            _isSuccess = true;
            _currentDeck = null;
            _firstSelectedIndex = -1;
            _secondSelectedIndex = -1;
            _matchCount = 0;
        }

        public void RetryGame()
        {
        }
    }
}
