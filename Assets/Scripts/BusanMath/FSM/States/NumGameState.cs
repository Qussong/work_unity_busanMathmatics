using System.Collections.Generic;
using TMPro;
using UnityEngine;
using BusanMath.FSM;
using BusanMath.Views;
using BusanMath.Controllers;
using BusanMath.Managers;
using BusanMath.Models;

namespace BusanMath.FSM.States
{
    public class NumGameState : BaseState<NumGameState, NumGameView>
    {
        private ECountry _country;                                  // 현재 선택된 국가
        private List<GameObject> _answerTiles = new List<GameObject>();  // 생성된 정답 타일 목록

        public ECountry Country
        {
            set { _country = value; }
        }

        public NumGameState(NumGameView view) : base(view) { }

        public override void Init()
        {
            base.Init();

            // ── 버튼 이벤트 구독 (최초 1회) ──────────────

            // 홈 버튼
            _view._OnHomeButtonClicked += () => { NavigationController.Instance.GoToHome(); };

            // 힌트 팝업 열기/닫기
            _view._OnHintButtonClikced += () => { OpenHint(); };
            _view._OnHintCloseButtonClicked += () => { CloseHint(); };

            // 결과 팝업 - 다시하기
            _view._OnRetryButtonClicked += () => {
                RetryNumGame();
                _view._infoText.text = "";
                _view._resultContainer.SetActive(false);
            };

            // 결과 팝업 - 다음 게임(카드게임)으로 이동
            _view._OnMoveNextButtonClicked += () => {
                _view._infoText.text = "";
                _view._resultContainer.SetActive(false);
                NavigationController.Instance.GoToCardGameDescription();
            };

            // 결과 팝업 - 다른 나라 선택 (영상 스킵하여 바로 국가 버튼 표시)
            _view._OnOtherCountryButtonClicked += () => {
                _view._infoText.text = "";
                _view._resultContainer.SetActive(false);
                NavigationController.Instance.GoToSelect(skipToButtons: true);
            };

            // ── 넘패드 버튼 리스너 등록 (최초 1회) ───────

            for (int i = 0; i < _view._numButtons.Count; ++i)
            {
                NumPad numpad = _view._numButtons[i].gameObject.AddComponent<NumPad>();
                numpad._value = i;
                _view._numButtons[i].onClick.AddListener(() => { UpdateAnswerUI(numpad._value); });
                _view._numButtons[i].onClick.AddListener(() => { SoundManager.Instance.PlayButtonSound(); });
            }

            // 정답 초기화 버튼
            _view._initButton.onClick.AddListener(() => {
                InitAnswerTile();
                NumGameManager.Instance.InitAnswer();
            });

            // 정답 비교(완료) 버튼
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

        /// <summary>
        /// 숫자게임 재시작 (정답 초기화 → 타일 삭제 → Enter 재진입)
        /// </summary>
        private void RetryNumGame()
        {
            NumGameManager.Instance.InitAnswer();
            DestroyAnswerTile();
            Enter();
        }

        /// <summary>
        /// 넘패드 입력 시 정답 타일 UI 업데이트
        /// </summary>
        public void UpdateAnswerUI(int value)
        {
            bool result = NumGameManager.Instance.SelectNumTile(value);
            if (false == result) return;
            int curTileIdx = NumGameManager.Instance.Answer.Length - 1;
            _answerTiles[curTileIdx].GetComponentInChildren<TMP_Text>().text = value.ToString();
        }

        /// <summary>
        /// 힌트 팝업 열기
        /// </summary>
        private void OpenHint()
        {
            _view._hintContainer.SetActive(true);
        }

        /// <summary>
        /// 힌트 팝업 닫기
        /// </summary>
        private void CloseHint()
        {
            _view._hintContainer.SetActive(false);
        }

        public override void Enter()
        {
            base.Enter();
            _view.Show();

            // 게임 시작 (랜덤 숫자 생성)
            NumGameManager.Instance.StartGame(_country);

            // 정답 타일 UI 생성
            SetAnswerUI();

            // 국가별 UI 적용 (배경, 타이틀, 힌트, 퀴즈)
            SetCountryUI(_country);
        }

        /// <summary>
        /// 랜덤 숫자 자릿수만큼 정답 타일 생성
        /// </summary>
        private void SetAnswerUI()
        {
            for (int i = 0; i < NumGameManager.Instance.RndNum.Length; ++i)
            {
                GameObject tile = UnityEngine.Object.Instantiate(_view._answerTilePrefab, _view._answerTileContainer.transform);
                _answerTiles.Add(tile);
            }
        }

        /// <summary>
        /// 국가별 UI 세팅 (배경, 타이틀, 힌트 이미지, 퀴즈 문제)
        /// </summary>
        private void SetCountryUI(ECountry country)
        {
            // 배경 이미지
            _view._background.sprite = _view._backGroundList[(int)country];

            // 타이틀 이미지
            _view._title.sprite = _view._titleList[(int)country];
            _view._title.SetNativeSize();
            _view._title.rectTransform.sizeDelta /= 4f;

            // 팝업 힌트 이미지
            _view._popupHint.sprite = _view._hintList[(int)country];
            _view._popupHint.SetNativeSize();
            _view._popupHint.rectTransform.sizeDelta /= 4f;

            // 퀴즈 문제 세팅
            SetQuizUI(country);
        }

        /// <summary>
        /// 퀴즈 문제 UI 세팅 (중국: 한자 텍스트, 이집트/로마: 숫자 이미지)
        /// </summary>
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

        /// <summary>
        /// 퀴즈 UI 초기화 (숫자 이미지/텍스트 숨김, 타일 삭제)
        /// </summary>
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

        /// <summary>
        /// 정답 타일 텍스트를 "0"으로 초기화
        /// </summary>
        public void InitAnswerTile()
        {
            foreach (GameObject obj in _answerTiles)
            {
                obj.GetComponentInChildren<TMP_Text>().text = "0";
            }
        }

        /// <summary>
        /// 정답 타일 오브젝트 전체 삭제
        /// </summary>
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
}
