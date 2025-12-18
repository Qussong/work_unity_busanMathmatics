using System;
using UnityEngine;
using UnityEngine.UI;

public class CardGameView : BaseView
{
    [Header("=== CardGameDescription View Settings ===")]
    [SerializeField] public Button _homeButton;
    [SerializeField] public Button _moveNext;   // cardGame -> vote

    public event Action _OnHomeButtonClicked;
    public event Action _OnMoveNextClicked;

    protected override void Initialize()
    {

    }

    protected override void BindUIEvent()
    {
        _homeButton.onClick.AddListener(() => _OnHomeButtonClicked?.Invoke());
        _moveNext.onClick.AddListener(() => _OnMoveNextClicked?.Invoke());
    }
}
