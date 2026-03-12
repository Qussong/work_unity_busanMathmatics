using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NumGameState : BaseState<NumGameState, NumGameView>
{
    private ECountry _country;
    private List<GameObject> _answerTiles = new List<GameObject>();

    public ECountry Country
    {
        set { _country = value; }
    }

    public NumGameState(NumGameView view) : base(view) { }

    public override void Init()
    {
        base.Init();

        // 이벤트 구독 (최초 1회)
        _view._OnHomeButtonClicked += () => { NavigationController.Instance.GoToHome(); };
        _view._OnHintButtonClikced += () => { OpenHint(); };
        _view._OnHintCloseButtonClicked += () => { CloseHint(); };
        _view._OnRetryButtonClicked += () => {
            RetryNumGame();
            _view._infoText.text = "";
            _view._resultContainer.SetActive(false);
        };
        _view._OnMoveNextButtonClicked += () => {
            _view._infoText.text = "";
            _view._resultContainer.SetActive(false);
            NavigationController.Instance.GoToCardGameDescription();
        };
        _view._OnOtherCountryButtonClicked += () => {
            _view._infoText.text = "";
            _view._resultContainer.SetActive(false);
            NavigationController.Instance.GoToSelect();
        };

        // 숫자 타일에 리스너 등록 (최초 1회)
        for (int i = 0; i < _view._numButtons.Count; ++i)
        {
            NumPad numpad = _view._numButtons[i].gameObject.AddComponent<NumPad>();
            numpad._value = i;
            _view._numButtons[i].onClick.AddListener(() => { UpdateAnswerUI(numpad._value); });
            _view._numButtons[i].onClick.AddListener(() => { SoundManager.Instance.PlayButtonSound(); });
        }

        // 초기화 버튼
        _view._initButton.onClick.AddListener(() => {
            InitAnswerTile();
            NumGameManager.Instance.InitAnswer();
        });

        // 완료 버튼
        _view._compareButton.onClick.AddListener(() => {
            bool result = NumGameManager.Instance.CompareAnswerAndRndNum();
            _view._resultContainer.SetActive(true);
            if (result)
            {
                SoundManager.Instance.PlayCorrectSound();
                _view._infoText.text = "정답입니다.";
            }
            else
            {
                SoundManager.Instance.PlayDisCorrectSound();
                _view._infoText.text = $"오답입니다.\n정답은 {NumGameManager.Instance.RndNum}입니다.";
            }
        });
    }

    private void RetryNumGame()
    {
        NumGameManager.Instance.InitAnswer();
        DestroyAnswerTile();
        Enter();
    }

    public void UpdateAnswerUI(int value)
    {
        bool result = NumGameManager.Instance.SelectNumTile(value);
        if (false == result) return;
        int curTileIdx = NumGameManager.Instance.Answer.Length - 1;
        _answerTiles[curTileIdx].GetComponentInChildren<TMP_Text>().text = value.ToString();
    }

    private void OpenHint()
    {
        _view._hintContainer.SetActive(true);
    }

    private void CloseHint()
    {
        _view._hintContainer.SetActive(false);
    }

    public override void Enter()
    {
        base.Enter();
        _view.Show();

        // 게임 시작
        NumGameManager.Instance.StartGame(_country);

        // 정답 UI 업데이트
        SetAnswerUI();

        // 국가 UI 업데이트
        SetCountryUI(_country);
    }

    private void SetAnswerUI()
    {
        for (int i = 0; i < NumGameManager.Instance.RndNum.Length; ++i)
        {
            GameObject tile = UnityEngine.Object.Instantiate(_view._answerTilePrefab, _view._answerTileContainer.transform);
            _answerTiles.Add(tile);
        }
    }

    private void SetCountryUI(ECountry country)
    {
        // 배경화면 세팅
        _view._background.sprite = _view._backGroundList[(int)country];

        // 타이틀 세팅
        _view._title.sprite = _view._titleList[(int)country];
        _view._title.SetNativeSize();
        _view._title.rectTransform.sizeDelta /= 4f;

        // 힌트 버튼 세팅
        _view._hintButton.image.sprite = _view._hintList[(int)country];
        _view._hintButton.image.SetNativeSize();
        if (ECountry.Egypt == _country)
        {
            _view._hintButton.image.rectTransform.sizeDelta /= 12f;
            _view._hintButton.transform.localPosition = new Vector3(-315f, 550f, 0f);
        }
        else if (ECountry.China == _country)
        {
            _view._hintButton.image.rectTransform.sizeDelta /= 12f;
            _view._hintButton.transform.localPosition = new Vector3(-315f, 525f, 0f);
        }
        else if (ECountry.Roma == _country)
        {
            _view._hintButton.image.rectTransform.sizeDelta /= 12f;
            _view._hintButton.transform.localPosition = new Vector3(-315f, 525f, 0f);
        }

        // 팝업 힌트 세팅
        _view._popupHint.sprite = _view._hintList[(int)country];
        _view._popupHint.SetNativeSize();
        _view._popupHint.rectTransform.sizeDelta /= 4f;

        // 퀴즈 문제 세팅
        SetQuizUI(country);
    }

    public void SetQuizUI(ECountry country)
    {
        if (ECountry.China == country)
        {
            _view._rndNumText.gameObject.SetActive(true);
            _view._rndNumText.text = NumGameManager.Instance.GetRndNumToHanJa();
        }
        else
        {
            _view._rndNumImage.gameObject.SetActive(true);
            _view._rndNumImage.sprite = NumGameManager.Instance.GetRndNumSprite();
            _view._rndNumImage.SetNativeSize();
            _view._rndNumImage.rectTransform.sizeDelta /= 4f;
        }
    }

    public void InitQuizUI()
    {
        if (ECountry.China == _country)
        {
            _view._rndNumText.text = "";
            _view._rndNumText.gameObject.SetActive(false);
        }
        else
        {
            _view._rndNumImage.sprite = null;
            _view._rndNumImage.gameObject.SetActive(false);
        }

        DestroyAnswerTile();
    }

    public void InitAnswerTile()
    {
        foreach (GameObject obj in _answerTiles)
        {
            obj.GetComponentInChildren<TMP_Text>().text = "0";
        }
    }

    public void DestroyAnswerTile()
    {
        foreach (GameObject tile in _answerTiles)
        {
            UnityEngine.GameObject.Destroy(tile);
        }
        _answerTiles.Clear();
    }

    public override void Exit()
    {
        base.Exit();

        // 힌트/결과 팝업 닫기
        _view._hintContainer.SetActive(false);
        _view._resultContainer.SetActive(false);
        _view._infoText.text = "";

        // 퀴즈 UI 초기화
        InitQuizUI();

        // 게임 매니저 초기화
        NumGameManager.Instance.InitGame();

        // 국가 정보 초기화
        _country = ECountry.None;

        _view.Hide();
    }
}
