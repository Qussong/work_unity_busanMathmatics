using BusanMath.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

/// <summary>
/// 투표 데이터 직렬화 클래스
/// </summary>
[Serializable]
public class VoteData
{
    public int voteEgypt;
    public int voteChina;
    public int voteRoma;
}

/// <summary>
/// 투표 관리 싱글톤
/// JSON 파일로 투표 데이터 영속화
/// </summary>
public class VoteManager : MonoSingleton<VoteManager>
{
    private VoteData _data;
    private string _filePath;
    public event Action<VoteData> _OnVoteUpdated;

    public VoteData GetData() => _data;

    protected override void OnSingletonAwake()
    {
        Initialize();
    }

    private void Initialize()
    {
        _filePath = Path.Combine(Application.persistentDataPath, "vote_data.json");
        Load();
    }

    /// <summary>
    /// JSON 파일에서 투표 데이터 로드 (없으면 새로 생성)
    /// </summary>
    private void Load()
    {
        if (File.Exists(_filePath))
        {
            string json = File.ReadAllText(_filePath);
            _data = JsonUtility.FromJson<VoteData>(json);
        }
        else
        {
            _data = new VoteData();
            Save();
        }
    }

    private void Save()
    {
        string json = JsonUtility.ToJson(_data, true);
        File.WriteAllText(_filePath, json);
    }

    /// <summary>
    /// 선택한 국가에 투표 후 이벤트 발행
    /// </summary>
    public void Vote(ECountry choice)
    {
        switch (choice)
        {
            case ECountry.Egypt:
                _data.voteEgypt++;
                break;
            case ECountry.China:
                _data.voteChina++;
                break;
            case ECountry.Roma:
                _data.voteRoma++;
                break;
        }
        Save();
        _OnVoteUpdated?.Invoke(_data);
    }

    public int GetTotal() => _data.voteEgypt + _data.voteChina + _data.voteRoma;

    /// <summary>
    /// 특정 국가의 투표 비율 반환 (0.0 ~ 1.0)
    /// </summary>
    public float GetRate(ECountry choice)
    {
        float rate = 0f;
        int total = GetTotal();
        if (0 == total) return rate;

        switch (choice)
        {
            case ECountry.Egypt:
                rate = (float)_data.voteEgypt / total;
                break;
            case ECountry.China:
                rate = (float)_data.voteChina / total;
                break;
            case ECountry.Roma:
                rate = (float)_data.voteRoma / total;
                break;
        }

        return rate;
    }

    public void Reset()
    {
        _data = new VoteData();
        Save();
        _OnVoteUpdated?.Invoke(_data);
    }

    /// <summary>
    /// 투표 수 기준 내림차순 랭킹 반환
    /// </summary>
    public List<ECountry> GetRanking()
    {
        var ranking = new Dictionary<ECountry, int>
        {
            { ECountry.Egypt, _data.voteEgypt },
            { ECountry.China, _data.voteChina },
            { ECountry.Roma, _data.voteRoma }
        };

        return ranking
            .OrderByDescending(x => x.Value)
            .Select(x => x.Key)
            .ToList();
    }
}
