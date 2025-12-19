using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class CardData
{
    public string _country;
    public int _value;
    public Sprite _cardSprite;
}

[CreateAssetMenu(fileName = "CardDatabase", menuName = "Game/Card Database")]
public class CardDatabaseSO : ScriptableObject
{
    public List<CardData> cards = new List<CardData>();
}