using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StringSpritePair
{
    public string Key;
    public Sprite Value;
}

[CreateAssetMenu(menuName = "Custom/StringSpritePairContainer")]
public class StringSpritePairContainerSO : ScriptableObject
{
    public List<StringSpritePair> Pairs = new List<StringSpritePair>();

    public Dictionary<string, Sprite> ToDictionary()
    {
        Dictionary<string, Sprite> dict = new Dictionary<string, Sprite>();
        foreach (var pair in Pairs)
        {
            if (!dict.ContainsKey(pair.Key))
                dict.Add(pair.Key, pair.Value);
        }
        return dict;
    }

    // 바로 접근 가능한 메서드 추가
    public Sprite GetSprite(string key)
    {
        return ToDictionary().TryGetValue(key, out var sprite) ? sprite : null;
    }

    public StringSpritePair GetRandom()
    {
        return Pairs.Count > 0 ? Pairs[UnityEngine.Random.Range(0, Pairs.Count)] : null;
    }
}