using TMPro;
using UnityEngine;

public class WriteState : BaseState<WriteState, WriteView>
{
    private ECountry _country;

    private readonly int _MIN_YEAR = 1980;
    private readonly int _MIN_MONTH = 1;
    private readonly int _MIN_DAY = 1;
    private readonly int _MAX_YEAR = 2030;
    private readonly int _MAX_MONTH = 12;
    private readonly int _MAX_DAY = 31;

    public ECountry Country
    {
        set { _country = value; }
    }

    public WriteState(WriteView view) : base(view) { }

    public override void Init()
    {
        base.Init();

        // 이벤트 구독 (최초 1회)
        _view._OnHomeButtonClicked += () => { NavigationController.Instance.GoToHome(); };
        _view._OnOkayButtonClicked += () => {
            _view._okayButton.gameObject.SetActive(false);
            _view._writeBoardContainer.SetActive(true);
            if (ECountry.Egypt == _country)
            {
                _view._yearPreview.sprite = _view._yearEgyptList[_view._yearUI.CurrentPage];
                _view._yearPreview.SetNativeSize();
                _view._yearPreview.rectTransform.sizeDelta /= 3.5f;
                _view._yearPreview.color = new Color(1f, 1f, 1f, 0.5f);

                _view._monthPreview.sprite = _view._monthEgyptList[_view._monthUI.CurrentPage];
                _view._monthPreview.SetNativeSize();
                _view._monthPreview.rectTransform.sizeDelta /= 3.5f;
                _view._monthPreview.color = new Color(1f, 1f, 1f, 0.5f);

                _view._dayPreview.sprite = _view._dayEgyptList[_view._dayUI.CurrentPage];
                _view._dayPreview.SetNativeSize();
                _view._dayPreview.rectTransform.sizeDelta /= 3.5f;
                _view._dayPreview.color = new Color(1f, 1f, 1f, 0.5f);
            }
            else if (ECountry.China == _country)
            {
                _view._yearPreview.sprite = _view._yearChinaList[_view._yearUI.CurrentPage];
                _view._yearPreview.SetNativeSize();
                _view._yearPreview.rectTransform.sizeDelta /= 4f;
                _view._yearPreview.color = new Color(1f, 1f, 1f, 0.5f);

                _view._monthPreview.sprite = _view._monthChinaList[_view._monthUI.CurrentPage];
                _view._monthPreview.SetNativeSize();
                _view._monthPreview.rectTransform.sizeDelta /= 4f;
                _view._monthPreview.color = new Color(1f, 1f, 1f, 0.5f);

                _view._dayPreview.sprite = _view._dayChinaList[_view._dayUI.CurrentPage];
                _view._dayPreview.SetNativeSize();
                _view._dayPreview.rectTransform.sizeDelta /= 4f;
                _view._dayPreview.color = new Color(1f, 1f, 1f, 0.5f);
            }
            else if (ECountry.Roma == _country)
            {
                _view._yearPreview.sprite = _view._yearRomaList[_view._yearUI.CurrentPage];
                _view._yearPreview.SetNativeSize();
                _view._yearPreview.rectTransform.sizeDelta /= 4f;
                _view._yearPreview.color = new Color(1f, 1f, 1f, 0.5f);

                _view._monthPreview.sprite = _view._monthRomaList[_view._monthUI.CurrentPage];
                _view._monthPreview.SetNativeSize();
                _view._monthPreview.rectTransform.sizeDelta /= 4f;
                _view._monthPreview.color = new Color(1f, 1f, 1f, 0.5f);

                _view._dayPreview.sprite = _view._dayRomaList[_view._dayUI.CurrentPage];
                _view._dayPreview.SetNativeSize();
                _view._dayPreview.rectTransform.sizeDelta /= 4f;
                _view._dayPreview.color = new Color(1f, 1f, 1f, 0.5f);
            }
        };
        _view._OnMoveNextButtonClicked += () => {
            NavigationController.Instance.GoToVoteResult();
        };

        // 날짜 선택 텍스트 세팅
        SetDate();
    }

    public override void Enter()
    {
        base.Enter();

        // 선택된 국가에 맞는 배경 이미지 세팅
        _view._backgroundImage.sprite = _view._backgroundSpriteList[(int)_country];

        // 선택된 국가에 맞는 타이틀 이미지 세팅
        _view._titleImage.sprite = _view._titleSpriteList[(int)_country];
        _view._titleImage.SetNativeSize();
        _view._titleImage.rectTransform.sizeDelta /= 4f;

        _view.Show();

        // 스와이프를 0번 페이지로 초기화 (year)
        int backCnt = _view._yearUI.CurrentPage;
        for (int i = 0; i < backCnt; ++i)
        {
            _view._yearUI.AutoSwipe(true);
        }
        // month
        backCnt = _view._monthUI.CurrentPage;
        for (int i = 0; i < backCnt; ++i)
        {
            _view._monthUI.AutoSwipe(true);
        }
        // day
        backCnt = _view._dayUI.CurrentPage;
        for (int i = 0; i < backCnt; ++i)
        {
            _view._dayUI.AutoSwipe(true);
        }
    }

    public override void Exit()
    {
        base.Exit();

        _country = ECountry.None;

        _view._backgroundImage.sprite = null;
        _view._titleImage.sprite = null;

        _view._yearPreview.sprite = null;
        _view._monthPreview.sprite = null;
        _view._dayPreview.sprite = null;

        _view._drawTextureUI.Clear();
        _view._writeBoardContainer.SetActive(false);
        _view._okayButton.gameObject.SetActive(true);

        _view.Hide();
    }

    public void SetDate()
    {
        for (int i = 0; i <= _MAX_YEAR - _MIN_YEAR; ++i)
        {
            _view._years[i].GetComponentInChildren<TMP_Text>().text = (_MIN_YEAR + i).ToString();
        }

        for (int i = 0; i <= _MAX_MONTH - _MIN_MONTH; ++i)
        {
            _view._months[i].GetComponentInChildren<TMP_Text>().text = (_MIN_MONTH + i).ToString();
        }

        for (int i = 0; i <= _MAX_DAY - _MIN_DAY; ++i)
        {
            _view._days[i].GetComponentInChildren<TMP_Text>().text = (_MIN_DAY + i).ToString();
        }
    }
}
