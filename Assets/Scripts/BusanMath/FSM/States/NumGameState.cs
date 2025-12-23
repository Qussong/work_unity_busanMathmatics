using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class NumGameState : BaseState
{
    private NumGameView _numGameView;
    private ECountry _country;
    private List<GameObject> _answerTiles = new List<GameObject>();

    public ECountry Country
    {
        set
        {
            _country = value;
        }
    }

    public NumGameState(NumGameView view)
    {
        _numGameView = view;

        // 이벤트 등록
        _numGameView._OnHomeButtonClicked += () => { NavigationController.Instance.GoToHome(); };
        _numGameView._OnHintButtonClikced += () => { OpenHint(); };
        _numGameView._OnHintCloseButtonClicked += () => { CloseHint(); };
        _numGameView._OnRetryButtonClicked += () => {
            RetryNumGame();
            _numGameView._infoText.text = "";
            _numGameView._resultContainer.SetActive(false);
        };
        _numGameView._OnMoveNextButtonClicked += () => {
            _numGameView._infoText.text = "";
            _numGameView._resultContainer.SetActive(false);
            NavigationController.Instance.GoToCardGameDescription(); 
        };
        _numGameView._OnOtherCountryButtonClicked += () => {
            _numGameView._infoText.text = "";
            _numGameView._resultContainer.SetActive(false);
            NavigationController.Instance.GoToSelect(); 
        };

        // 숫자 타일에 리스너 등록
        for (int i = 0; i < _numGameView._numButtons.Count; ++i)
        {
            NumPad numpad = _numGameView._numButtons[i].AddComponent<NumPad>();
            numpad._value = i;
            _numGameView._numButtons[i].onClick.AddListener(() => { UpdateAnswerUI(numpad._value); });
        }

        // 초기화 버튼 리스너 등록
        _numGameView._initButton.onClick.AddListener(() => {
            InitAnswerTile();
            NumGameManager.Instance.InitAnswer();
        });

        // 완료 버튼 리스너 등록
        _numGameView._compareButton.onClick.AddListener(() => {
            bool result = NumGameManager.Instance.CompareAnswerAndRndNum();
            _numGameView._resultContainer.SetActive(true);
            // 정답
            if(result)
            {
                SoundManager.Instance.PlayCorrectSound();
                _numGameView._infoText.text = "정답입니다.";
            }
            // 오답
            else
            {
                SoundManager.Instance.PlayDisCorrectSound();
                _numGameView._infoText.text = $"오답입니다.\n정답은 {NumGameManager.Instance.RndNum}입니다.";
            }
        });

    }

    private void RetryNumGame()
    {
        // 정답 초기화
        NumGameManager.Instance.InitAnswer();

        // 정답 UI 파괴
        DestroyAnswerTile();

        // 시작
        Enter();
    }

    public void UpdateAnswerUI(int value)
    {
        // 정답에 값 추가
        bool result = NumGameManager.Instance.SelectNumTile(value);
        if (false == result) return;
        // 현재 타일의 idx 
        int curTileIdx = NumGameManager.Instance.Answer.Length - 1;
        // 현재 타일에 선택한 값 반영
        _answerTiles[curTileIdx].GetComponentInChildren<TMP_Text>().text = value.ToString();
    }

    private void OpenHint()
    {
        _numGameView._hintContainer.SetActive(true);
    }

    private void CloseHint()
    {
        _numGameView._hintContainer.SetActive(false);
    }

    public override void Enter()
    {
        Debug.Log("[NumGameState] Enter");
        _numGameView.Show();

        // 랜덤값 설정
        NumGameManager.Instance.StartGame(_country);

        // 정답 UI 업데이트
        SetAnswerUI();

        // 나라별 UI 업데이트
        SetCountryUI(_country);
    }

    private void SetAnswerUI()
    {
        for (int i = 0; i < NumGameManager.Instance.RndNum.Length; ++i)
        {
            GameObject tile = UnityEngine.Object.Instantiate(_numGameView._answerTilePrefab, _numGameView._answerTileContainer.transform);
            _answerTiles.Add(tile);
        }
    }

    private void SetCountryUI(ECountry country)
    {
        //Debug.Log((int)country);

        // 배경화면 설정
        _numGameView._background.sprite = _numGameView._backGroundList[(int)country];

        // 타이틀 설정
        _numGameView._title.sprite = _numGameView._titleList[(int)country];
        _numGameView._title.SetNativeSize();
        _numGameView._title.rectTransform.sizeDelta /= 4f;

        // 힌트 버튼 설정
        _numGameView._hintButton.image.sprite = _numGameView._hintList[(int)country];
        _numGameView._hintButton.image.SetNativeSize();
        if (ECountry.Egypt == _country)
        {
            _numGameView._hintButton.image.rectTransform.sizeDelta /= 12f;
            _numGameView._hintButton.transform.localPosition = new Vector3(-315f, 550f, 0f);
        }
        else if (ECountry.China == _country)
        {
            _numGameView._hintButton.image.rectTransform.sizeDelta /= 12f;
            _numGameView._hintButton.transform.localPosition = new Vector3(-315f, 525f, 0f);
        }
        else if (ECountry.Roma == _country)
        {
            _numGameView._hintButton.image.rectTransform.sizeDelta /= 12f;
            _numGameView._hintButton.transform.localPosition = new Vector3(-315f, 525f, 0f);
        }
        else
        {
            //
        }

        // 팝업 힌트 설정
        _numGameView._popupHint.sprite = _numGameView._hintList[(int)country];
        _numGameView._popupHint.SetNativeSize();
        _numGameView._popupHint.rectTransform.sizeDelta /= 4f;

        // 나라별 퀴즈 설정
        SetQuizUI(country);
    }

    public void SetQuizUI(ECountry country)
    {
        // 중국
        if (ECountry.China == country)
        {
            _numGameView._rndNumText.gameObject.SetActive(true);
            _numGameView._rndNumText.text = NumGameManager.Instance.GetRndNumToHanJa();
        }
        // 이집트, 로마
        else
        {
            _numGameView._rndNumImage.gameObject.SetActive(true);
            _numGameView._rndNumImage.sprite = NumGameManager.Instance.GetRndNumSprite();
            _numGameView._rndNumImage.SetNativeSize();
            _numGameView._rndNumImage.rectTransform.sizeDelta /= 4f;

        }
    }

    public void InitQuizUI()
    {
        // 중국
        if (ECountry.China == _country)
        {
            _numGameView._rndNumText.text = "";
            // 랜덤값을 보여주는 텍스트 객체 off
            _numGameView._rndNumText.gameObject.SetActive(false);

        }
        // 이집트, 로마
        else
        {
            _numGameView._rndNumImage.sprite = null;
            // 랜덤값을 보여주는 이미지 객체 off
            _numGameView._rndNumImage.gameObject.SetActive(false);
        }

        // 정답관련 UI 초기화
        DestroyAnswerTile();
    }

    public void InitAnswerTile()
    {
        foreach(GameObject obj in _answerTiles)
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

    public override void Update()
    {
        //
    }

    public override void Exit()
    {
        Debug.Log("[NumGameState] Eixt");

        // 퀴즈 UI 초기화
        InitQuizUI();

        // 게임의 랜덤 값, 국가, 정답 초기화
        NumGameManager.Instance.InitGame();

        // 국가 정보 초기화
        _country = ECountry.None;

        _numGameView.Hide();
    }

}
