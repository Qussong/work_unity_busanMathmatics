using BusanMath.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 카드 매칭 게임 로직 관리
/// 랜덤 덱 생성, 카드 선택/비교, 매칭 결과 이벤트 발행
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

    /// <summary>
    /// 게임 시작 - 덱 생성 및 초기화
    /// </summary>
    public void StartGame()
    {
        _currentDeck = CreateShuffledCards();
        _firstSelectedIndex = -1;
        _matchCount = 0;
    }

    /// <summary>
    /// 랜덤으로 6장 선택 후 복제하여 12장 셔플된 덱 생성
    /// </summary>
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

    /// <summary>
    /// 카드 선택 처리 (첫 번째 -> 두 번째 -> 비교)
    /// </summary>
    public void SelectCard(int index)
    {
        // 같은 카드 중복 선택 방지
        if (_firstSelectedIndex == index) return;

        // 첫 번째 선택
        if (_firstSelectedIndex == -1)
        {
            _firstSelectedIndex = index;
            return;
        }

        // 두 번째 선택 - 쌍 비교
        _secondSelectedIndex = index;
        bool isMatch = Compare(_firstSelectedIndex, _secondSelectedIndex);

        if (isMatch)
        {
            _matchCount++;
            StartCoroutine(DelayedCallMatchSucess(_firstSelectedIndex, _secondSelectedIndex, 0.5f));

            // 게임 클리어 체크
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

        // 선택 초기화
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

    /// <summary>
    /// 두 카드가 같은 종류인지 비교
    /// </summary>
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

    /// <summary>
    /// 게임 리셋 (진행 중인 코루틴 모두 정지)
    /// </summary>
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
