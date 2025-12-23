using BusanMath.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardGameManager : MonoSingleton<CardGameManager>
{
    [SerializeField] private CardDatabaseSO _cardDatabase;  // 카드 데이터베이스 (총 40장)

    private List<CardData> _currentDeck;        // 현재 게임에 사용중인 카드 덱
    private int _firstSelectedIndex = -1;       // 첫 번째 선택된 카드 인덱스
    private int _secondSelectedIndex = -1;      // 두 번째 선택된 카드 인덱스
    private int _matchCount;                    // 매칭 성공 횟수
    private const int _MAX_MATCH = 6;           // 총 매칭 필요 횟수 (6쌍)
    public bool _isSuccess = true;

    public event Action<int, int> _OnMatchSuccess;  // 매칭 성공 (index1, index2)
    public event Action<int, int> _OnMatchFail;     // 매칭 실패 (index1, index2)
    public event Action _OnGameClear;               // 게임 클리어

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
    /// 랜덤으로 6장 선택 후 복제하여 12장의 섞인 카드 덱 생성
    /// </summary>
    /// <returns>무작위로 섞인 12장의 카드 리스트</returns>
    public List<CardData> CreateShuffledCards()
    {
        // 전체 카드에서 랜덤 6장 선택
        List<CardData> selected = _cardDatabase.cards
            .OrderBy(_ => UnityEngine.Random.value)
            .Take(6)
            .ToList();

        // 선택된 카드 복제 (6 + 6 = 12)
        List<CardData> deck = new List<CardData>();
        deck.AddRange(selected);
        deck.AddRange(selected);

        // 무작위로 섞어서 반환
        return deck.OrderBy(_ => UnityEngine.Random.value).ToList();
    }

    /// <summary>
    /// 카드 선택 처리
    /// </summary>
    /// <param name="index">선택한 카드 인덱스</param>
    public void SelectCard(int index)
    {
        // 같은 카드 선택 방지
        if (_firstSelectedIndex == index) return;

        // 첫 번째 선택
        if (_firstSelectedIndex == -1)
        {
            _firstSelectedIndex = index;
            return;
        }

        // 두 번째 선택 - 비교 진행
        _secondSelectedIndex = index;
        bool isMatch = Compare(_firstSelectedIndex, _secondSelectedIndex);

        if (isMatch)
        {
            _matchCount++;
            //_OnMatchSuccess?.Invoke(_firstSelectedIndex, _secondSelectedIndex);
            StartCoroutine(DelayedCallMatchSucess(_firstSelectedIndex, _secondSelectedIndex, 0.5f));

            // 게임 클리어 체크
            if (_matchCount >= _MAX_MATCH)
            {
                _isSuccess = true;
                //_OnGameClear?.Invoke();
                StartCoroutine(DelayedCallGameClear(1f));
            }
        }
        else
        {
            //_OnMatchFail?.Invoke(_firstSelectedIndex, _secondSelectedIndex);
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
    /// 두 카드가 같은 값인지 비교
    /// </summary>
    /// <param name="index1">첫 번째 카드 인덱스</param>
    /// <param name="index2">두 번째 카드 인덱스</param>
    /// <returns>매칭 여부</returns>
    private bool Compare(int index1, int index2)
    {
        if (_currentDeck == null) return false;
        if (index1 < 0 || index1 >= _currentDeck.Count) return false;
        if (index2 < 0 || index2 >= _currentDeck.Count) return false;

        return _currentDeck[index1]._value == _currentDeck[index2]._value
            && _currentDeck[index1]._country == _currentDeck[index2]._country;
    }

    /// <summary>
    /// 특정 인덱스의 카드 데이터 반환
    /// </summary>
    public CardData GetCard(int index) => _currentDeck[index];

    /// <summary>
    /// 현재 덱 반환
    /// </summary>
    public List<CardData> GetCurrentDeck() => _currentDeck;

    /// <summary>
    /// 현재 매칭 횟수 반환
    /// </summary>
    public int GetMatchCount() => _matchCount;

    /// <summary>
    /// 게임 리셋
    /// </summary>
    public void ResetGame()
    {
        _isSuccess = true;
        _currentDeck = null;
        _firstSelectedIndex = -1;
        _secondSelectedIndex = -1;
        _matchCount = 0;
    }
    
    /// <summary>
    /// 
    /// </summary>
    public void RetryGame()
    {

    }

}
