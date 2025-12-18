using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VoteResultView : BaseView
{
    [Header("=== VoteResult View Settings ===")]
    [SerializeField] public Image _titleImage;
    [SerializeField] public Image _boardImage;
    [SerializeField] public List<TMP_Text> _rankCountryList = new List<TMP_Text>();
    [SerializeField] public List<Image> _countryViewList = new List<Image>();
    [SerializeField] public List<TMP_Text> _votePercentList = new List<TMP_Text>();
    [SerializeField] public List<Slider> _voteRateBarList = new List<Slider>();
    [SerializeField] public List<TMP_Text> _voteCountList = new List<TMP_Text>();
    [SerializeField] public Button _homeButton;

    public event Action _OnHomeButtonClicked;

    protected override void Initialize()
    {

    }

    protected override void BindUIEvent()
    {
        _homeButton.onClick.AddListener(() => _OnHomeButtonClicked?.Invoke());
    }
}
