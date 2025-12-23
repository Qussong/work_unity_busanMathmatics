using SwipeUI;
using TMPro;
using UnityEngine;

public class WriteState : BaseState
{
    private WriteView _writeView;
    private ECountry _country;

    private readonly int _MIN_YEAR = 1980;
    private readonly int _MIN_MONTH = 1;
    private readonly int _MIN_DAY = 1;
    private readonly int _MAX_YEAR = 2030;
    private readonly int _MAX_MONTH = 12;
    private readonly int _MAX_DAY = 31;


    public ECountry Country
    {
        set
        {
            _country = value;
        }
    }

    public WriteState(WriteView view)
    {
        _writeView = view;

        // 이벤트 등록
        _writeView._OnHomeButtonClicked += () => { NavigationController.Instance.GoToHome(); };
        _writeView._OnOkayButtonClicked += () => {
            // Writing Board 등장
            _writeView._okayButton.gameObject.SetActive(false);
            _writeView._writeBoardContainer.SetActive(true);
            //Debug.Log("year : " + (_writeView._yearUI.CurrentPage + _minYear));
            //Debug.Log("month : " + (_writeView._monthUI.CurrentPage + _minMonth));
            //Debug.Log("day : " + (_writeView._dayUI.CurrentPage + _minDay));
            if (ECountry.Egypt == _country)
            {
                _writeView._yearPreview.sprite = _writeView._yearEgyptList[_writeView._yearUI.CurrentPage];
                _writeView._yearPreview.SetNativeSize();
                _writeView._yearPreview.rectTransform.sizeDelta /= 3.5f;
                _writeView._yearPreview.color = new Color(1f, 1f, 1f, 0.5f);

                _writeView._monthPreview.sprite = _writeView._monthEgyptList[_writeView._monthUI.CurrentPage];
                _writeView._monthPreview.SetNativeSize();
                _writeView._monthPreview.rectTransform.sizeDelta /= 3.5f;
                _writeView._monthPreview.color = new Color(1f, 1f, 1f, 0.5f);

                _writeView._dayPreview.sprite = _writeView._dayEgyptList[_writeView._dayUI.CurrentPage];
                _writeView._dayPreview.SetNativeSize();
                _writeView._dayPreview.rectTransform.sizeDelta /= 3.5f;
                _writeView._dayPreview.color = new Color(1f, 1f, 1f, 0.5f);
            }
            else if(ECountry.China == _country)
            {
                _writeView._yearPreview.sprite = _writeView._yearChinaList[_writeView._yearUI.CurrentPage];
                _writeView._yearPreview.SetNativeSize();
                _writeView._yearPreview.rectTransform.sizeDelta /= 4f;
                _writeView._yearPreview.color = new Color(1f, 1f, 1f, 0.5f);

                _writeView._monthPreview.sprite = _writeView._monthChinaList[_writeView._monthUI.CurrentPage];
                _writeView._monthPreview.SetNativeSize();
                _writeView._monthPreview.rectTransform.sizeDelta /= 4f;
                _writeView._monthPreview.color = new Color(1f, 1f, 1f, 0.5f);

                _writeView._dayPreview.sprite = _writeView._dayChinaList[_writeView._dayUI.CurrentPage];
                _writeView._dayPreview.SetNativeSize();
                _writeView._dayPreview.rectTransform.sizeDelta /= 4f;
                _writeView._dayPreview.color = new Color(1f, 1f, 1f, 0.5f);
            }
            else if(ECountry.Roma == _country)
            {
                _writeView._yearPreview.sprite = _writeView._yearRomaList[_writeView._yearUI.CurrentPage];
                _writeView._yearPreview.SetNativeSize();
                _writeView._yearPreview.rectTransform.sizeDelta /= 4f;
                _writeView._yearPreview.color = new Color(1f, 1f, 1f, 0.5f);

                _writeView._monthPreview.sprite = _writeView._monthRomaList[_writeView._monthUI.CurrentPage];
                _writeView._monthPreview.SetNativeSize();
                _writeView._monthPreview.rectTransform.sizeDelta /= 4f;
                _writeView._monthPreview.color = new Color(1f, 1f, 1f, 0.5f);

                _writeView._dayPreview.sprite = _writeView._dayRomaList[_writeView._dayUI.CurrentPage];
                _writeView._dayPreview.SetNativeSize();
                _writeView._dayPreview.rectTransform.sizeDelta /= 4f;
                _writeView._dayPreview.color = new Color(1f, 1f, 1f, 0.5f);
            }
        };
        _writeView._OnMoveNextButtonClicked += () =>{
            NavigationController.Instance.GoToVoteResult(); 
        };

        // 날짜 보드 텍스트 설정
        SetDate();

    }

    public override void Enter()
    {
        Debug.Log("[WriteState] Enter");

        // 선택된 국가에 맞는 배경 이미지 설정
        _writeView._backgroundImage.sprite = _writeView._backgroundSpriteList[(int)_country];

        // 선택된 국가에 맞는 타이틀 이미지 설정
        _writeView._titleImage.sprite = _writeView._titleSpriteList[(int)_country];
        _writeView._titleImage.SetNativeSize();
        _writeView._titleImage.rectTransform.sizeDelta /= 4f;

        _writeView.Show();

        // 최초 시작할 때 0번 페이지를 볼 수 있도록 설정 (year)
        int backCnt = _writeView._yearUI.CurrentPage;
        for (int i = 0; i < backCnt; ++i)
        {
            _writeView._yearUI.AutoSwipe(true);
        }
        // 최초 시작할 때 0번 페이지를 볼 수 있도록 설정 (month)
        backCnt = _writeView._monthUI.CurrentPage;
        for (int i = 0; i < backCnt; ++i)
        {
            _writeView._monthUI.AutoSwipe(true);
        }
        // 최초 시작할 때 0번 페이지를 볼 수 있도록 설정 (day)
        backCnt = _writeView._dayUI.CurrentPage;
        for (int i = 0; i < backCnt; ++i)
        {
            _writeView._dayUI.AutoSwipe(true);
        }
    }

    public override void Update()
    {
        //
    }

    public override void Exit()
    {
        Debug.Log("[WriteState] Eixt");

        // 국가 선택 기록 초기화
        _country = ECountry.None;

        // 배경 이미지 설정 해제
        _writeView._backgroundImage.sprite = null;

        // 타이틀 이미지 설정 해제
        _writeView._titleImage.sprite = null;

        // Writing Board 의 Year, Month, Day 해제
        _writeView._yearPreview.sprite = null;
        _writeView._monthPreview.sprite = null;
        _writeView._dayPreview.sprite = null;

        // DrawTexture 의 라인 지우기
        _writeView._drawTextureUI.Clear();

        // Writing Board off
        _writeView._writeBoardContainer.SetActive(false);

        // 확인 버튼 on
        _writeView._okayButton.gameObject.SetActive(true);

        _writeView.Hide();
    }

    /// <summary>
    /// 년, 월, 일 설정
    /// </summary>
    public void SetDate()
    {
        for(int i = 0; i <= _MAX_YEAR - _MIN_YEAR; ++i)
        {
            _writeView._years[i].GetComponentInChildren<TMP_Text>().text = (_MIN_YEAR + i).ToString();
        }

        for (int i = 0; i <= _MAX_MONTH - _MIN_MONTH; ++i)
        {
            _writeView._months[i].GetComponentInChildren<TMP_Text>().text = (_MIN_MONTH + i).ToString();
        }

        for (int i = 0; i <= _MAX_DAY - _MIN_DAY; ++i)
        {
            _writeView._days[i].GetComponentInChildren<TMP_Text>().text = (_MIN_DAY + i).ToString();
        }
    }
}
