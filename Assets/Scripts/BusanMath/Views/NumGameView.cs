using System;
using UnityEngine;
using UnityEngine.UI;

public class NumGameView : BaseView
{
    [Header("=== NumGameDescription View Settings ===")]
    [SerializeField] public Button _homeButton;
    [SerializeField] public Button _moveNext;       // numGame -> carGameDescription
    [SerializeField] public Button _otherCountry;    // numGame -> select

    public event Action _OnHomeButtonClicked;
    public event Action _OnMoveNextButtonClicked;
    public event Action _OnOtherCountryButtonClicked;

    protected override void Initialize()
    {

    }

    protected override void BindUIEvent()
    {
        _homeButton.onClick.AddListener(() => _OnHomeButtonClicked?.Invoke());
        _moveNext.onClick.AddListener(() => _OnMoveNextButtonClicked?.Invoke());
        _otherCountry.onClick.AddListener(() => _OnOtherCountryButtonClicked?.Invoke());
    }
}
