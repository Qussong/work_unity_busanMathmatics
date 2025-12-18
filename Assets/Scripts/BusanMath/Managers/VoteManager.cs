using BusanMath.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

/// <summary>
/// 투표 데이터 저장 클래스
/// </summary>
[Serializable]
public class VoteData
{
    public int voteEgypt;
    public int voteChina;
    public int voteRoma;
}

public class VoteManager : MonoSingleton<VoteManager>
{
    
    private VoteData _data;                         // 투표 데이터
    private string _filePath;                       // 투표 데이터 저장 경로
    public event Action<VoteData> _OnVoteUpdated;   // 투표 갱신 시 호출되는 이벤트

    /// <summary>
    ///  현재 투표 데이터 반환
    /// </summary>
    public VoteData GetData() => _data;

    protected override void OnSingletonAwake()
    {
        Initialize();
    }

    /// <summary>
    /// 파일 경로 설정 및 데이터 로드
    /// </summary>
    private void Initialize()
    {
        _filePath = Path.Combine(Application.persistentDataPath, "vote_data.json");
        Load();
    }

    /// <summary>
    /// 외부 JSON 파일에서 투표 데이터 로드
    /// 파일이 없으면 새 데이터 생성 후 저장
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

    /// <summary>
    /// 현재 투표 데이터를 외부 JSON 파일에 저장
    /// </summary>
    private void Save()
    {
        string json = JsonUtility.ToJson(_data, true);
        File.WriteAllText(_filePath, json);
    }

    /// <summary>
    /// 선택한 국가에 투표
    /// 저장 및 이벤트 호출
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

    /// <summary>
    /// 전체 투표 수 반환
    /// </summary>
    public int GetTotal() => _data.voteEgypt + _data.voteChina + _data.voteRoma;

    /// <summary>
    /// 선택한 투표 비율 반환 (범위 : 0.0 ~ 1.0)
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

    /// <summary>
    /// 투표 초기화
    /// </summary>
    public void Reset()
    {
        _data = new VoteData();
        Save();
        _OnVoteUpdated?.Invoke(_data);
    }

    /// <summary>
    /// 등수 반환
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
